﻿/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Kai Münch, Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll), or All Rights Reserved (Kai Münch)
 */

using System;
using System.Security.Cryptography;
using HVH.Service.Interfaces;

namespace HVH.Service.Encryption
{
    /// <summary>
    /// Encrypts data using RSA keys - internal use only
    /// </summary>
    internal class RSAEncryptionProvider : IEncryptionProvider 
    {
        /// <summary>
        /// A RSA that is used to encrypt the 
        /// </summary>
        public RSACryptoServiceProvider key;

        public RSAEncryptionProvider(Int32 keySize)
        {
            key = new RSACryptoServiceProvider(keySize);
        }

        public Byte[] Encrypt(Byte[] data)
        {
            return key.Encrypt(data, false);
        }

        public Byte[] Decrypt(Byte[] data)
        {
            return key.Decrypt(data, false);
        }

        public void ChangeKey(Byte[] newKey)
        {
            // ignored
        }
    }
}