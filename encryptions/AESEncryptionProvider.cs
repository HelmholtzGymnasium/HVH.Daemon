/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.Security.Cryptography;
using System.Text;
using HVH.Service.Interfaces;

namespace HVH.Service.Encryptions
{
    /// <summary>
    /// Provides encryption through AES
    /// </summary>
    public class AESEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// The underlying key
        /// </summary>
        private String key;

        public Byte[] Encrypt(Byte[] data)
        {
            return Cryptography.Encrypt<AesCryptoServiceProvider>(data, key);
        }

        public Byte[] Decrypt(Byte[] data)
        {
            return Cryptography.Decrypt<AesCryptoServiceProvider>(data, key);
        }

        public void ChangeKey(Byte[] newKey)
        {
            key = Encoding.UTF8.GetString(newKey);
        }
    }
}
