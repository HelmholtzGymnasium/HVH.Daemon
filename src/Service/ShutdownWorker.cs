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