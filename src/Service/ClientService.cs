/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Helios.Exceptions;
using Helios.Net;
using Helios.Topology;
using HVH.Common.Connection;
using HVH.Common.Encryption;
using HVH.Common.Interfaces;
using HVH.Common.Plugins;
using HVH.Common.Settings;
using log4net;
using log4net.Config;

namespace HVH.Service.Service
{
    /// <summary>
    /// The service class. Here we establish a connection to the server, and listen for commands.
    /// </summary>
    public class ClientService : ServiceBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The currently active ClientService
        /// </summary>
        public static ClientService Instance { get; private set; }

        /// <summary>
        /// The TCP connection to the server
        /// </summary>
        public ConnectionWorker Connection { get; set; }

        /// <summary>
        /// A component that handles encryption of our messages
        /// </summary>
        public IEncryptionProvider encryption { get; set; }

        /// <summary>
        /// Whether we could log into the server
        /// </summary>
        public Boolean SessionCreated { get; set; }

        /// <summary>
        /// The threads created by the application
        /// </summary>
        public List<Thread> Threads { get; set; }

        // The last message received from the server
        private List<String> messageBacklog;

        // Whether we sent data to the server, waiting for an answer
        private Boolean sessionDataPending = false;

        /// <summary>
        /// Create a new Instance of the service
        /// </summary>
        public ClientService()
        {
            // Init log4net
            XmlConfigurator.Configure();

            // Create dirs
            Directory.CreateDirectory("logs/");
            Directory.CreateDirectory("plugins/");

            AutoLog = true;
            CanHandlePowerEvent = true;
            CanHandleSessionChangeEvent = true;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
            ServiceName = "HVH.Service";

            Instance = this;
            PluginManager.LoadPlugins();
            Threads = new List<Thread>();
            messageBacklog = new List<String>();
        }

        /// <summary>
        /// Called when the service is started. Loads settings, and tries to connect to the server
        /// </summary>
        protected override void OnStart(String[] args)
        {
            Connection = new ConnectionWorker(ConnectionSettings.Instance.server, ConnectionSettings.Instance.port);
            Connection.Established = ConnectionEstablished;
            Connection.Received = DataReceived;
            Connection.Terminated = ConnectionTerminated;
            SessionCreated = false;
            Threads.Add(Utility.StartThread(LockWorker.Check, true)); 
            
            // Say hello!
            log.Info("Service Startup");
            log.Info("Connecting to the server");
        }

        /// <summary>
        /// Handles the login procedure for the Server
        /// </summary>
        /// <param name="node"></param>
        /// <param name="connection"></param>
        private void ConnectionEstablished(INode node, IConnection connection)
        {
            log.Info("Connection established. Sending public RSA key.");
            RSAEncryptionProvider rsa = new RSAEncryptionProvider(SecuritySettings.Instance.keySize);
            encryption = rsa;
            connection.Send(Communication.DAEMON_SEND_PUBLIC_KEY, node, new NoneEncryptionProvider());
            connection.Send(rsa.key.ToXmlString(false), node, new NoneEncryptionProvider());
        }

        /// <summary>
        /// Handles new messages by the server
        /// </summary>
        /// <param name="networkData"></param>
        /// <param name="connection"></param>
        private void DataReceived(NetworkData networkData, IConnection connection)
        {
            Byte[] buffer = networkData.Buffer;
            buffer = encryption.Decrypt(buffer);
            log.DebugFormat("Message received. Length: {0}", buffer.Length);

            String message = Encoding.UTF8.GetString(buffer);
            if (message != Communication.SERVER_SEND_WAIT_SIGNAL)
                messageBacklog.Add(message);

            // Do we have a message cached
            if (buffer.Length == 32 && messageBacklog.Count == 0)
            {
                // Handle messages who dont have additional parameters  
                if (!SessionCreated && sessionDataPending &&
                    message != Communication.SERVER_SEND_SESSION_CREATED)
                {
                    // Invalid connection
                    log.Fatal("Server is talking an invalid connection protocol!");
                    connection.Send(Communication.DAEMON_SEND_DISCONNECT, networkData.RemoteHost, encryption);
                    Connection.Client.Close();
                    Stop();
                }
                else if (message == Communication.SERVER_SEND_HEARTBEAT_CHALLENGE)
                {
                    log.Debug("Heartbeat received");
                    connection.Send(Communication.DAEMON_SEND_HEARTBEAT, networkData.RemoteHost, encryption);
                    connection.Send(Win32.GetUsername(Win32.WTSGetActiveConsoleSessionId()), networkData.RemoteHost, encryption);
                    messageBacklog.Clear();
                }
                else if (message == Communication.SERVER_SEND_LOCK)
                {
                    // Lock the screen
                    log.Info("Received Screen lock Signal");
                    LockWorker.LockScreen();
                    messageBacklog.Clear();
                }
                else if (message == Communication.SERVER_SEND_UNLOCK)
                {
                    // Unlock the screen
                    log.Info("Received Screen unlock Signal");
                    LockWorker.UnlockScreen();
                    messageBacklog.Clear();
                }
                else if (message == Communication.SERVER_SEND_DISCONNECT)
                {
                    // Server has gone offline, go and die too
                    log.Fatal("Server went offline.");
                    connection.Send(Communication.DAEMON_SEND_DISCONNECT, networkData.RemoteHost, encryption);
                    Connection.Client.Close();
                    Stop();
                }
            }
            else if (messageBacklog.Any())
            {
                if (messageBacklog[0] == Communication.SERVER_SEND_SESSION_KEY)
                {
                    // Load the Encoder Type
                    try
                    {
                        Type encoderType = PluginManager.GetType<IEncryptionProvider>(SecuritySettings.Instance.encryption);
                        encryption = (IEncryptionProvider) Activator.CreateInstance(encoderType);
                    }
                    catch (Exception e)
                    {
                        log.Error("Invalid Encryption Provider! Falling back to no encryption", e);

                        // Fallback to None
                        encryption = new NoneEncryptionProvider();
                    }

                    // Log
                    log.InfoFormat("Received session key. Used encryption: {0}", encryption.GetType().Name);

                    // Apply session key
                    encryption.ChangeKey(buffer);

                    // Send session Data
                    log.Info("Sending session data");
                    connection.Send(Communication.DAEMON_SEND_SESSION_DATA, networkData.RemoteHost, encryption);
                    connection.Send(Environment.MachineName, networkData.RemoteHost, encryption);
                    connection.Send(Win32.GetUsername(Win32.WTSGetActiveConsoleSessionId()), networkData.RemoteHost, encryption);
                    connection.Send(Communication.DAEMON_ID, networkData.RemoteHost, encryption);
                    log.Info("Sucessfully send session data");
                    sessionDataPending = true;

                    messageBacklog.Clear();
                }
                else if (!SessionCreated && sessionDataPending && messageBacklog[0] == Communication.SERVER_SEND_SESSION_CREATED)
                {
                    if (message == Communication.SERVER_ID)
                    {
                        sessionDataPending = false;
                        SessionCreated = true;
                        log.Info("Session created");
                    }
                    else
                    {
                        // Invalid connection
                        log.Info("Invalid connection");
                        connection.Send(Communication.DAEMON_SEND_DISCONNECT, networkData.RemoteHost, encryption);
                        Connection.Client.Close();
                        Stop();
                    }

                    // Clear
                    messageBacklog.Clear();
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_SHUTDOWN)
                {
                    Int32 delay = 0;
                    Int32.TryParse(message, out delay);
                    log.InfoFormat("Received Shutdown Signal. Delay: {0} seconds", delay);
                    ShutdownWorker.Shutdown(delay);
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_REBOOT)
                {
                    Int32 delay = 0;
                    Int32.TryParse(message, out delay);
                    log.InfoFormat("Received Reboot Signal. Delay: {0} seconds", delay);
                    ShutdownWorker.Reboot(delay);
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_FORBIDDEN_PROCESSES)
                {
                    String[] news = message.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    ProcessWorker.Add(news);
                    log.InfoFormat("Added {0} entries to the list of forbidden processes", news.Length);
                    messageBacklog.Clear();
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_FORBIDDEN_PROCESSES_CLEAR)
                {
                    String[] news = message.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    ProcessWorker.Reset();
                    ProcessWorker.Add(news);
                    log.InfoFormat("Cleared the current data adn added {0} entries to the list of forbidden processes", news.Length);
                    messageBacklog.Clear();
                }
            }
        }

        /// <summary>
        /// Handles an abrupt termination of the connection
        /// </summary>
        /// <param name="heliosConnectionException"></param>
        /// <param name="connection"></param>
        private void ConnectionTerminated(HeliosConnectionException heliosConnectionException, IConnection connection)
        {
            // Server has gone offline, go and die too
            log.FatalFormat("Server went offline. Reason: {0}", heliosConnectionException.Message);
            Connection.Client.Close();
            Stop();
        }

        /// <summary>
        /// Finalizes the connection
        /// </summary>
        protected override void OnStop()
        {
            Connection.Client.Send(Communication.DAEMON_SEND_DISCONNECT, Connection.Client.RemoteHost, encryption);
            Connection.Client.Close();
            log.Info("Connection closed");
            log.Info("Service Shutdown");
        }

        /// <summary>
        /// When the service gets continued
        /// </summary>
        protected override void OnContinue()
        {
            log.Info("Service Continued");
        }

        /// <summary>
        /// When the service gets paused
        /// </summary>
        protected override void OnPause()
        {
            log.Info("Service Paused");
        }

        /// <summary>
        /// When the service gets shut down
        /// </summary>
        protected override void OnShutdown()
        {
            log.Info("Service Shutdown");
        }

        /// <summary>
        /// The session changes (lock, logon, logoff)
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            log.Info("Session Changed: " + changeDescription.Reason + " (ID: " + changeDescription.SessionId + ")");
            log.Info("Current User: " + Win32.GetUsername(changeDescription.SessionId));
        }

        /// <summary>
        /// The service receives a custom command
        /// </summary>
        protected override void OnCustomCommand(Int32 command)
        {
            log.Info("Custom Command received: " + command);
        }

        /// <summary>
        /// The power status of the computer changes
        /// </summary>
        protected override Boolean OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            log.Info("PowerStatus changed: " + powerStatus);
            return base.OnPowerEvent(powerStatus);
        }
    }
}