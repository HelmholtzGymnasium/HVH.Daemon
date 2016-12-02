/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2016
 * Licensed under the terms of the MIT License (Dorian Stoll)
 */

using System;
using System.Security.Cryptography;
using System.Text;
using HVH.Service.Interfaces;

namespace HVH.Service.Encryptions
{
    /// <summary>
    /// Provides encryption through DES
    /// </summary>
    public class DESEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// The underlying key
        /// </summary>
        private String key;

        public Byte[] Encrypt(Byte[] data)
        {
            return Cryptography.Encrypt<DESCryptoServiceProvider>(data, key);
        }

        public Byte[] Decrypt(Byte[] data)
        {
            return Cryptography.Decrypt<DESCryptoServiceProvider>(data, key);
        }

        public void ChangeKey(Byte[] newKey)
        {
            key = Encoding.UTF8.GetString(newKey);
        }
    }
}
