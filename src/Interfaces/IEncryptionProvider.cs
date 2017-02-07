/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
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