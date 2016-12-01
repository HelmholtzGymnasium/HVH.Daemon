﻿/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;
using MadMilkman.Ini;

namespace HVH.Service.Settings
{
    [SectionName("security")]
    public class SecuritySettings : Settings<SecuritySettings>
    {
        [IniSerialization("keySize")]
        public Int32 keySize { get; set; }

        [IniSerialization("encryption")]
        public String encryption { get; set; }
    }
}