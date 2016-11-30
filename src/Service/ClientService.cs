/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System;
using System.ServiceProcess;
using Helios.Net;
using Helios.Topology;
using HVH.Service.Connection;
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
        }

        /// <summary>
        /// Called when the service is started. Loads settings, and tries to connect to the server
        /// </summary>
        protected override void OnStart(String[] args)
        {
            ConnectionWorker worker = new ConnectionWorker(ConnectionSettings.Instance.server, ConnectionSettings.Instance.port);
            worker.Established = ConnectionEstablished;
        }

        /// <summary>
        /// Handles the login procedure for the Server
        /// </summary>
        /// <param name="node"></param>
        /// <param name="connection"></param>
        private void ConnectionEstablished(INode node, IConnection connection)
        {
            
        }
    }
}