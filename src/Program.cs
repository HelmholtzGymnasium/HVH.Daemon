using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
                new ohasffaso()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
