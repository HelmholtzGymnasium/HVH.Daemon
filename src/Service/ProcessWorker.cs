/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;

namespace HVH.Service.Service
{
    public class ProcessWorker
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Locks access to make the object threadsafe
        private static Object lockObject = new Object();

        // A list of forbidden process names
        private static List<String> forbidden = new List<String>();

        /// <summary>
        /// Resets the list of forbidden processes
        /// </summary>
        public static void Reset()
        {
            lock (lockObject)
                forbidden = new List<String>();
        }

        /// <summary>
        /// Adds a range of process names
        /// </summary>
        public static void Add(params String[] names)
        {
            lock (lockObject)
                forbidden.AddRange(names);
        }

        /// <summary>
        /// Checks the running processes
        /// </summary>
        public static void Check()
        {
            while (true)
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (forbidden.Contains(p.ProcessName))
                    {
                        p.Close();
                        log.InfoFormat("User {0} tried to launch the forbidden process {1}", Win32.GetUsername(Win32.WTSGetActiveConsoleSessionId()), p.ProcessName);
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}