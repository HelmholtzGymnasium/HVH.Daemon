/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HVH.Service.Plugins
{
    /// <summary>
    /// Loads plugins from a plugins directory, and manages access to the types inside of the assemblies
    /// </summary>
    public class PluginManager
    {
        /// <summary>
        /// All assemblies, including the service assembly
        /// </summary>
        private static List<Assembly> assemblies;

        public static void LoadPlugins()
        {
            // Create the list and add the own assembly
            assemblies = new List<Assembly>();
            assemblies.Add(typeof(PluginManager).Assembly);

            // Does the directory exist?
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/plugins/"))
                return;

            // Get all Assembly files
            foreach (String file in Directory.GetFiles(Directory.GetCurrentDirectory() + "/plugins/", "*.dll", SearchOption.AllDirectories))
            {
                assemblies.Add(Assembly.Load(File.ReadAllBytes(file)));
            }
        }

        /// <summary>
        /// Queries all assemblies for a type with a given name, and that extends T
        /// </summary>
        public static Type GetType<T>(String name)
        {
            foreach (Type t in assemblies.SelectMany(a => a.GetTypes()))
            {
                if (t.Name == name && t.IsSubclassOf(typeof(T)) && t.IsPublic)
                    return t;
            }

            // Nothing was found
            return null;
        }
    }
}