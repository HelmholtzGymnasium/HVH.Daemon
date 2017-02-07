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