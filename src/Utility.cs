/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using Helios.Net;
using Helios.Topology;
using HVH.Common.Interfaces;
using log4net;

namespace HVH.Service
{
    /// <summary>
    /// Provides common methods, so we dont have to write that much
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Starts a new thread
        /// </summary>
        public static Thread StartThread(ThreadStart start, Boolean background)
        {
            Thread t = new Thread(start);
            t.IsBackground = background;
            t.Start();
            log.DebugFormat("Started new thread: {0}", start.Method.Name);
            return t;
        }

        /// <summary>
        /// Abstraction layer over connection.Send()
        /// </summary>
        public static void Send(this IConnection connection, String data, INode node, IEncryptionProvider encryption = null)
        {
            Byte[] buffer = Encoding.UTF8.GetBytes(data);
            if (encryption != null)
            {
                buffer = encryption.Encrypt(buffer);
            }
            log.DebugFormat("Message sent. Length: {0}", buffer.Length);
            connection.Send(buffer, 0, buffer.Length, node);
        }

        /// <summary>
        /// Runs an executable in the background
        /// </summary>
        public static Process Run(String exe, String args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(exe, args);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = true;
            log.InfoFormat("Starting process: {0}", exe + " " + args);
            return Process.Start(psi);
        }
    }
}