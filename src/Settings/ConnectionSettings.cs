/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using MadMilkman.Ini;

namespace HVH.Service.Settings
{
    [SectionName("connection")]
    public class ConnectionSettings : Settings<ConnectionSettings>
    {
        [IniSerialization("server")]
        public String server { get; set; }

        [IniSerialization("port")]
        public Int32 port { get; set; }
    }
}