using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBLIB.Service
{

    public class CryptoHelper
    {
        static Encoding defaultStringEncoding = Encoding.UTF8;

        static string convertToHex(byte[] src)
        {
            StringBuilder sb = new StringBuilder(src.Length * 2);
            foreach (var ch in src)
                sb.AppendFormat("{0:X2}", ch);
            return sb.ToString();
        }

        public static string makePassword(string src)
        {
            return makeSHA1("YOLO" + src);  // YOLO 를 붙여서 같은 비밀번호라도 타 사이트와 다르게
        }
        public static string makePassword2(string src)
        {
            return makeSHA1(src);
        }
        public static string makeMD5(string src)
        {
            var instance = MD5.Create();
            byte[] raw = instance.ComputeHash(defaultStringEncoding.GetBytes(src));
            return convertToHex(raw);
        }

        static SHA1 sha1Instance = SHA1.Create();
        static MD5 md5Instance = MD5.Create();

        public static string makeSHA1(string src)
        {
            byte[] raw = sha1Instance.ComputeHash(defaultStringEncoding.GetBytes(src));
            return convertToHex(raw);
        }

        private static int _iterations = 2;
        private static int _keySize = 256;

        private static string _hash = "SHA1";
        private static string _salt = "aselrias38490a32"; // Random
        private static string _vector = "8947az34awl34kjq"; // Random

        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }

        public static string Encrypt(object value, string password)
        {
            string jsoned = JsonConvert.SerializeObject(value);
            return Encrypt(jsoned, password);
        }

        public static byte[] GetBytes<T>(string src)
            where T : Encoding, new()
        {
            T t = new T();
            return t.GetBytes(src);
        }

        public static string Encrypt<T>(string value, string password)
                where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = GetBytes<ASCIIEncoding>(_vector);
            byte[] saltBytes = GetBytes<ASCIIEncoding>(_salt);
            byte[] valueBytes = GetBytes<UTF8Encoding>(value);

            byte[] encrypted;
            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes =
                    new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

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
            return Convert.ToBase64String(encrypted);
        }



        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }
        public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {
            byte[] vectorBytes = GetBytes<ASCIIEncoding>(_vector);
            byte[] saltBytes = GetBytes<ASCIIEncoding>(_salt);
            byte[] valueBytes = Convert.FromBase64String(value);

            byte[] decrypted;
            int decryptedByteCount = 0;

            using (T cipher = new T())
            {
                PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
                byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

                cipher.Mode = CipherMode.CBC;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger(typeof(CryptoHelper)).Error("Decrypting error ", ex);
                    return String.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GetBytes<ASCIIEncoding>("ABCDEFGHABCDEFGHABCDEFGHABCDEFGH");
                aesAlg.IV = GetBytes<ASCIIEncoding>("0123456701234567");

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
    }
}
