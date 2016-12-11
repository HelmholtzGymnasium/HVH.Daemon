/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

namespace HVH.Service.Service
{
    /// <summary>
    /// Shuts down the computer
    /// </summary>
    public class ShutdownWorker
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Shutdown(Int32 delay)
        {
            log.Info("Shutting down client host");
            Utility.Run("shutdown", "/s /t " + delay);
        }

        public static void Reboot(Int32 delay)
        {
            log.Info("Restarting client host");
            Utility.Run("shutdown", "/r /t " + delay);
        }
    }
}