using System;
using System.IO;
using System.Security.Cryptography;

namespace MHLab.PATCH.Utilities
{
    internal class Hashing
    {
        /// <summary>
        /// Gets a hash of the file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string SHA1(string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(filePath, sha1);
        }

        /// <summary>
        /// Gets a hash of the file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string SHA1(Stream s)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(s, sha1);
        }

        /// <summary>
        /// Gets a hash of the file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string MD5(string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(filePath, md5);
        }

        /// <summary>
        /// Gets a hash of the file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string MD5(Stream s)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(s, md5);
        }

        /// <summary>
        /// Gets a hash of the file using CRC32.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string CRC32(string filename)
        {
            CRC32 crc32 = new CRC32();
            String hash = String.Empty;

            using (FileStream fs = File.Open(filename, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

            return hash;
        }

        /// <summary>
        /// Gets a hash of the file using CRC32.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string CRC32(FileStream file)
        {
            CRC32 crc32 = new CRC32();
            String hash = String.Empty;

            using (FileStream fs = file)
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

            return hash;
        }

        private static string GetHash(string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return GetHash(fs, hasher);
        }

        private static string GetHash(Stream s, HashAlgorithm hasher)
        {
            var hash = hasher.ComputeHash(s);
            var hashStr = Convert.ToBase64String(hash);
            return hashStr.TrimEnd('=');
        }
    }
}
