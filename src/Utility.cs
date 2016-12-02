/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Text;
using System.Threading;
using Helios.Net;
using Helios.Topology;
using HVH.Service.Interfaces;

namespace HVH.Service
{
    /// <summary>
    /// Provides common methods, so we dont have to write that much
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Starts a new thread
        /// </summary>
        public static Thread StartThread(ThreadStart start, Boolean background)
        {
            Thread t = new Thread(start);
            t.IsBackground = background;
            t.Start();
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
            connection.Send(buffer, 0, buffer.Length, node);
        }
    }
}