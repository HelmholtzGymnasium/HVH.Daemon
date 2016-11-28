/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System.ServiceProcess;

namespace HVH.Service
{
    /// <summary>
    /// The service class. Here we establish a connection to the server, and listen for commands.
    /// </summary>
    public class ClientService : ServiceBase
    {
        /// <summary>
        /// Create a new Instance of the service
        /// </summary>
        public ClientService()
        {
            AutoLog = true;
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
            this.ServiceName = "HVH.Service";
        }
    }
}