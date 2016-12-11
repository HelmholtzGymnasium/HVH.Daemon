/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using System.Runtime.InteropServices;

namespace HVH.Service
{
    /// <summary>
    /// Interface to the Windows API
    /// </summary>
    public class Win32
    {
        // Native Win32 API calls
        [DllImport("Wtsapi32.dll")]
        public static extern Boolean WTSQuerySessionInformation(IntPtr hServer, Int32 sessionId, WtsInfoClass wtsInfoClass, out IntPtr ppBuffer, out Int32 pBytesReturned);
        [DllImport("Wtsapi32.dll")]
        public static extern void WTSFreeMemory(IntPtr pointer);
        [DllImport("kernel32.dll")]
        public static extern Int32 WTSGetActiveConsoleSessionId();

        public enum WtsInfoClass
        {
            WTSUserName = 5,
            WTSDomainName = 7,
        }

        /// <summary>
        /// Returns the username of the current session
        /// </summary>
        public static String GetUsername(Int32 sessionId, Boolean prependDomain = true)
        {
            IntPtr buffer;
            Int32 strLen;
            String username = "SYSTEM";
            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);
                if (prependDomain && WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSDomainName, out buffer, out strLen) && strLen > 1)
                {
                    username = Marshal.PtrToStringAnsi(buffer) + "\\" + username;
                    WTSFreeMemory(buffer);
                }

            }
            return username;
        }
    }
}