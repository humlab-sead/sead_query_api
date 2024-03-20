using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SeadQueryInfra.Encryption
{
    public class EncryptUtility
    {
        public static string Encrypt(string text, string encryptKey)
        {
            Aes cryptoService = Aes.Create();

            cryptoService.Key = Encoding.ASCII.GetBytes(encryptKey);
            cryptoService.IV = Encoding.ASCII.GetBytes(encryptKey);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoService.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] array = memoryStream.ToArray();
            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string text, string decryptKey)
        {
            Aes cryptoService = Aes.Create();

            cryptoService.Key = (Encoding.ASCII.GetBytes(decryptKey));
            cryptoService.IV = (Encoding.ASCII.GetBytes(decryptKey));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoService.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] bytes = Convert.FromBase64String(text);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static string ToMD5(string text)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
