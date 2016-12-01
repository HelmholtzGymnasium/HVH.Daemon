﻿/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System;

namespace HVH.Service.Interfaces
{
    /// <summary>
    /// Provides an interface for encoding data
    /// </summary>
    public interface IEncryptionProvider
    {
        Byte[] Encrypt(Byte[] data);
        Byte[] Decrypt(Byte[] data);
        void ChangeKey(Byte[] newKey);
    }
}