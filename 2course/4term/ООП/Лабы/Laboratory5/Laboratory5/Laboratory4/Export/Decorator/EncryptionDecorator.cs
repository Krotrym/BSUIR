using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory4.Export.Decorator
{
    using System.Security.Cryptography;
    using System.Text;

    public class EncryptSaver : IDataSaver
    {
        private readonly IDataSaver _inner;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptSaver(IDataSaver inner, string password)
        {
            _inner = inner;

            using var sha = SHA256.Create();
            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            _iv = _key.Take(16).ToArray();
        }

        public void Save(string data, Stream output)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var crypto = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            _inner.Save(data, crypto);
        }
    }



}
