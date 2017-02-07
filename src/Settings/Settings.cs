/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.IO;
using System.Reflection;
using HVH.Service.Exceptions;
using log4net;
using MadMilkman.Ini;

namespace HVH.Service.Settings
{
    /// <summary>
    /// Provides a base class for settings
    /// </summary>
    public class Settings<T> where T : Settings<T>, new()
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The global settings instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Load the settings file
                    IniFile file = new IniFile();
                    file.Load(Directory.GetCurrentDirectory() + "/settings.ini");
                    SectionNameAttribute[] att = typeof(T).GetCustomAttributes(typeof(SectionNameAttribute), false) as SectionNameAttribute[];
                    IniSection section = file.Sections[att[0].name];

                    // Nullcheck
                    if (section == null)
                    {
                        log.Fatal("Settings file (settings.ini) is invalid");
                        throw new InvalidSettingsFileException();
                    }

                    // Serialize the data
                    _instance = section.Deserialize<T>();
                }
                
                // Return the internal representation
                return _instance;
            }
        }

        private static T _instance;
    }

    /// <summary>
    /// Controls the name of the section in the Ini file
    /// </summary>
    public class SectionNameAttribute : Attribute
    {
        public String name;

        public SectionNameAttribute(String name)
        {
            this.name = name;
        }
    }
}