/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License
 */

using System;

namespace HVH.Service.Exceptions
{
    /// <summary>
    /// Exception that indicates an invalid settings file
    /// </summary>
    public class InvalidSettingsFileException : Exception
    {
        public InvalidSettingsFileException() : base("The settings file doesn't match the spec.") { }
    }
}