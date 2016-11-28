/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System;
using MadMilkman.Ini;

namespace HVH.Service.Settings
{
    public class ConnectionSettings : Settings<ConnectionSettings>
    {
        [IniSerialization("server")]
        public String server { get; set; }

        [IniSerialization("port")]
        public Int32 port { get; set; }
    }
}