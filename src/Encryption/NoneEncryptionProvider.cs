/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Security.Cryptography;
using HVH.Service.Interfaces;

namespace HVH.Service.Encryption
{
    /// <summary>
    /// IEncryptionProvider shim for no encryption
    /// </summary>
    public class NoneEncryptionProvider  : IEncryptionProvider
    {
        public Byte[] Encrypt(Byte[] data)
        {
            return data;
        }

        public Byte[] Decrypt(Byte[] data)
        {
            return data;
        }

        public void ChangeKey(Byte[] newKey)
        {
            // ignored
        }
    }
}