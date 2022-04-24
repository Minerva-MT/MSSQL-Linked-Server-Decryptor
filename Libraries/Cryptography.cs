using System;
using System.IO;
using System.Security.Cryptography;

namespace SQL_Decryptor
{
    class Cryptography
    {
        public byte[] Unprotect (byte[] Encrypted, byte[] Entropy, DataProtectionScope Scope)
        {
            try
            {
                return ProtectedData.Unprotect(Encrypted, Entropy, Scope);
            }
            catch (Exception)
            {
                Console.WriteLine("Error Unprotecting Data!");
                return null;
            }
        }

        public string DecryptDES(byte[] Password, byte[] ServiceKey, byte[] InitVector)
        {
            TripleDES DES = TripleDES.Create();

            ICryptoTransform Decryptor = DES.CreateDecryptor(ServiceKey, InitVector);

            CryptoStream Decrypt = new CryptoStream(new MemoryStream(Password), Decryptor, CryptoStreamMode.Read);

            return new StreamReader(Decrypt).ReadToEnd();
        }

        public string DecryptAES(byte[] Password, byte[] ServiceKey, byte[] InitVector)
        {
            Aes AES = Aes.Create();

            ICryptoTransform Decryptor = AES.CreateDecryptor(ServiceKey, InitVector);

            CryptoStream Decrypt = new CryptoStream(new MemoryStream(Password), Decryptor, CryptoStreamMode.Read);

            return new StreamReader(Decrypt).ReadToEnd();
        }

    }
}
