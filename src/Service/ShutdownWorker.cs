/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Diagnostics;

namespace HVH.Service.Service
{
    /// <summary>
    /// Shuts down the computer
    /// </summary>
    public class ShutdownWorker
    {
        public static void Shutdown(Int32 delay)
        {
            Utility.Run("shutdown", "/s /t " + delay);
        }

        public static void Reboot(Int32 delay)
        {
            Utility.Run("shutdown", "/r /t " + delay);
        }
    }
}