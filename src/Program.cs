/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System.ServiceProcess;
using HVH.Service.Service;

namespace HVH.Service
{
    /// <summary>
    /// The class that registers the services in windows
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        public static void Main()
        {
            ServiceBase[] ServicesToRun = 
            {
                new ClientService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
