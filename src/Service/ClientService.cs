/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Helios.Net;
using Helios.Topology;
using HVH.Service.Connection;
using HVH.Service.Encryption;
using HVH.Service.Interfaces;
using HVH.Service.Plugins;
using HVH.Service.Settings;

namespace HVH.Service.Service
{
    /// <summary>
    /// The service class. Here we establish a connection to the server, and listen for commands.
    /// </summary>
    public class ClientService : ServiceBase
    {
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
        }

        /// <summary>
        /// Called when the service is started. Loads settings, and tries to connect to the server
        /// </summary>
        protected override void OnStart(String[] args)
        {
            ConnectionWorker worker = new ConnectionWorker(ConnectionSettings.Instance.server, ConnectionSettings.Instance.port);
            worker.Established = ConnectionEstablished;
            worker.Received = DataReceived;
            SessionCreated = false;
            Threads.Add(Utility.StartThread(LockWorker.Check, true));
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
            
            // Do we have a message cached
            if (buffer.Length == 32 && messageBacklog.Count == 0)
            {
                String message = Encoding.UTF8.GetString(buffer);
                messageBacklog.Add(message);

                // Handle messages who dont have additional parameters  
                if (!SessionCreated && sessionDataPending &&
                    message != Communication.SERVER_SEND_SESSION_CREATED)
                {
                    // Invalid connection
                    Stop();
                }
                else if (message == Communication.SERVER_SEND_HEARTBEAT_CHALLENGE)
                {
                    connection.Send(Communication.CLIENT_SEND_HEARTBEAT, networkData.RemoteHost, encryption);
                    connection.Send(Environment.UserName, networkData.RemoteHost, encryption); // TODO: This doesnt work. We need a Windows API call for that
                    messageBacklog.Clear();
                }
                else if (message == Communication.SERVER_SEND_LOCK)
                {
                    // Lock the screen
                    LockWorker.LockScreen();
                    messageBacklog.Clear();
                }
                else if (message == Communication.SERVER_SEND_UNLOCK)
                {
                    // Unlock the screen
                    LockWorker.UnlockScreen();
                    messageBacklog.Clear();
                }
            }
            else if (messageBacklog.Any())
            {
                if (messageBacklog[0] == Communication.SERVER_SEND_SESSION_KEY)
                {
                    // Load the Encoder Type
                    Type encoderType = PluginManager.GetType<IEncryptionProvider>(SecuritySettings.Instance.encryption);
                    encryption = (IEncryptionProvider) Activator.CreateInstance(encoderType);

                    // Apply session key
                    encryption.ChangeKey(buffer);

                    // Send session Data
                    connection.Send(Communication.CLIENT_SEND_SESSION_DATA, networkData.RemoteHost, encryption);
                    connection.Send(Environment.MachineName, networkData.RemoteHost, encryption);
                    connection.Send(Environment.UserName, networkData.RemoteHost, encryption);
                    connection.Send(Communication.CLIENT_ID, networkData.RemoteHost, encryption);
                    sessionDataPending = true;

                    messageBacklog.Clear();
                }
                else if (!SessionCreated && sessionDataPending && messageBacklog[0] == Communication.SERVER_SEND_SESSION_CREATED)
                {
                    String message = Encoding.UTF8.GetString(buffer);
                    if (message == Communication.SERVER_ID)
                    {
                        sessionDataPending = false;
                        SessionCreated = true;
                    }
                    else
                    {
                        // Invalid connection
                        Stop();
                    }

                    // Clear
                    messageBacklog.Clear();
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_SHUTDOWN)
                {
                    Int32 delay = 0;
                    String message = Encoding.UTF8.GetString(buffer);
                    Int32.TryParse(message, out delay);
                    ShutdownWorker.Shutdown(delay);
                }
                else if (messageBacklog[0] == Communication.SERVER_SEND_REBOOT)
                {
                    Int32 delay = 0;
                    String message = Encoding.UTF8.GetString(buffer);
                    Int32.TryParse(message, out delay);
                    ShutdownWorker.Reboot(delay);
                }
            }
        }

        /// <summary>
        /// Handles the login procedure for the Server
        /// </summary>
        /// <param name="node"></param>
        /// <param name="connection"></param>
        private void ConnectionEstablished(INode node, IConnection connection)
        {
            RSAEncryptionProvider rsa = new RSAEncryptionProvider(SecuritySettings.Instance.keySize);
            encryption = rsa;
            connection.Send(Communication.CLIENT_SEND_PUBLIC_KEY, node, new NoneEncryptionProvider());
            connection.Send(rsa.key.ToXmlString(false), node, new NoneEncryptionProvider());
        }

        /// <summary>
        /// Finalizes the connection
        /// </summary>
        protected override void OnStop()
        {
            Connection.Client.Close();
        }
    }
}