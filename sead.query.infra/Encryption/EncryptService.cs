using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SeadQueryCore;

namespace SeadQueryInfra.Encryption
{
    public class EncryptService : IEncryptService
    {
        public string Encrypt(string text, string encryptKey)
        {
            return EncryptUtility.Encrypt(text, encryptKey);
        }

        public string Decrypt(string text, string decryptKey)
        {
            return EncryptUtility.Decrypt(text, decryptKey);
        }

        public string ToMD5(string text)
        {
            return EncryptUtility.ToMD5(text);
        }
    }
}
