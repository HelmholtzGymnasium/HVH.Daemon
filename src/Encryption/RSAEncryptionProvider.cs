using System;
using System.Security.Cryptography;
using HVH.Service.Interfaces;

namespace HVH.Service.Encryption
{
    /// <summary>
    /// Encrypts data using RSA keys
    /// </summary>
    public class RSAEncryptionProvider /* : IEncryptionProvider - commented out. This is not meant to be used for the actual session encryption */
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