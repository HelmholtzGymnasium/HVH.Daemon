﻿/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System;
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
            CanHandlePowerEvent = true;
            CanHandleSessionChangeEvent = true;
            CanPauseAndContinue = true;
            CanShutdown = true;
            CanStop = true;
            ServiceName = "HVH.Service";
        }

        /// <summary>
        /// Called when the service is started. Loads settings, and tries to connect to the server
        /// </summary>
        protected override void OnStart(String[] args)
        {

        }
    }
}