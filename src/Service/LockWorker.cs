/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace HVH.Service.Service
{
    public class LockWorker
    {
        // the process of the app that locks the screen
        private static Process locker;

        // Locks access to make the object threadsafe
        private static Object lockObject = new Object();

        /// <summary>
        /// Creates the lockfile and the locking process
        /// </summary>
        public static void LockScreen()
        {
            lock (lockObject)
            {
                if (locker == null)
                {
                    locker = Utility.Run("HVH.Service.Lock.exe", "");
                    File.Create(Directory.GetCurrentDirectory() + "/screen.lock");
                }
            }
        }

        /// <summary>
        /// Removes the lockfile and the locking process
        /// </summary>
        public static void UnlockScreen()
        {
            lock (lockObject)
            {
                if (locker != null)
                {
                    locker.Close();
                    locker = null;
                    File.Delete(Directory.GetCurrentDirectory() + "/screen.lock");
                }
            }
        }

        /// <summary>
        /// Checks every 0.1 seconds if we need to take action
        /// </summary>
        public static void Check()
        {
            while (true)
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "/screen.lock") && locker == null)
                    LockScreen();
                else if (!File.Exists(Directory.GetCurrentDirectory() + "/screen.lock") && locker != null)
                    UnlockScreen();
                Thread.Sleep(100);
            }
        }

    }
}