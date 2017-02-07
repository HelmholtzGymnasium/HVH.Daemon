/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HVH.Service.Service
{
    /// <summary>
    /// The class that registers our management service in windows (or linux)
    /// </summary>
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        /// <summary>
        /// The service process installer
        /// </summary>
        private ServiceProcessInstaller serviceProcessInstaller;

        /// <summary>
        /// The service installer
        /// </summary>
        private ServiceInstaller serviceInstaller;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ProjectInstaller()
        {
            // ServiceProcessInstaller
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            // ServiceInstaller
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "HVH.Service";
            serviceInstaller.Description = "Management service for client computers";
            serviceInstaller.DisplayName = "HVH.Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // Add them
            Installers.AddRange(new Installer[] {serviceProcessInstaller, serviceInstaller});
        }
    }
}
