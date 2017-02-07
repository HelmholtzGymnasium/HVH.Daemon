/**
 * HVH.Service - Service that can manage client computers
 * Copyright (c) Dorian Stoll 2017
 * Licensed under the terms of the MIT License
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/**
 * Taken from: http://stackoverflow.com/questions/273452/using-aes-encryption-in-c-sharp
 */

namespace HVH.Service.Encryptions
{
    /// <summary>
    /// Provides basic methods for working with the .NET crypto API
    /// </summary>
    public static class Cryptography
    {
        #region Settings

        private static Int32 _iterations = 2;
        private static Int32 _keySize = 256;

        private static String _hash = "SHA1";
        private static String _salt = "aselrias38490a32"; // Random
        private static String _vector = "8947az34awl34kjq"; // Random

        #endregion

        public static Byte[] Encrypt<T>(Byte[] value, String password)
                where T : SymmetricAlgorithm, new()
        {
            Byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            Byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            Byte[] valueBytes = value;

            Byte[] encrypted;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes =
                    new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                Byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream to = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = to.ToArray();
                        }
                    }
                }
                cipher.Clear();
            }
            return encrypted;
        }

        public static Byte[] Decrypt<T>(Byte[] value, String password) where T : SymmetricAlgorithm, new()
        {
            Byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
            Byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
            Byte[] valueBytes = value;

            Byte[] decrypted;
            Int32 decryptedByteCount = 0;

            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                Byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new Byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new Byte[0];
                }

                cipher.Clear();
            }
            Array.Resize(ref decrypted, decryptedByteCount);
            return decrypted;
        }
    }
}
