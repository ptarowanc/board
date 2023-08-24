using System.Collections.Generic;
using System.IO;
using MHLab.PATCH.Settings;
using System;
using System.Net;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MHLab.PATCH
{
    public class FileManager
    {
        public static IEnumerable<string> GetFiles()
        {
            return Directory.GetFiles(SettingsManager.APP_PATH, "*", SearchOption.AllDirectories);
        }

        public static IEnumerable<string> GetFiles(string path)
        {
            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }

        public static IEnumerable<string> GetFiles(string path, string pattern)
        {
            return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
        }

        public static IEnumerable<string> GetFiles(string path, string pattern, SearchOption option)
        {
            return Directory.GetFiles(path, pattern, option);
        }

        public static IEnumerable<string> GetCurrentBuildFiles()
        {
            return GetFiles(SettingsManager.CURRENT_BUILD_PATH);
        }

        public static IEnumerable<string> GetAllBuildsDirectories()
        {
            DirectoryInfo[] tmp = new DirectoryInfo(SettingsManager.BUILDS_PATH).GetDirectories("*", SearchOption.TopDirectoryOnly);
            List<string> tmpList = new List<string>();
            foreach(DirectoryInfo entry in tmp)
            {
                tmpList.Add(entry.FullName);
            }
            return tmpList;
        }

        public static void CleanDirectory(string directory)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                DirectoryInfo dir = new DirectoryInfo(directory);

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                    file.Delete();
                }

                foreach (DirectoryInfo subDirectory in dir.GetDirectories())
                {
                    /*subDirectory.Attributes &= ~FileAttributes.ReadOnly;
                    subDirectory.Delete(true);*/
                    DeleteRecursiveFolder(subDirectory.FullName);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DeleteRecursiveFolder(string pFolderPath)
        {
            foreach(string Folder in Directory.GetDirectories(pFolderPath))
            {
                DeleteRecursiveFolder(Folder);
            }

            foreach(string file in Directory.GetFiles(pFolderPath))
            {
                var pPath = Path.Combine(pFolderPath, file);
                FileInfo fi = new FileInfo(pPath);
                File.SetAttributes(fi.FullName, FileAttributes.Normal);
                File.Delete(fi.FullName);
            }
            Directory.Delete(pFolderPath);
        }

        public static void DeleteFiles(string directory, string pattern)
        {
            IEnumerable<string> files = GetFiles(directory, pattern);
            foreach (string file in files) DeleteFile(file);
        }

        public static bool DeleteFile(string file)
        {
            try
            {
                if (!_deleteFile(file))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return _deleteFile(file);
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static bool _deleteFile(string file)
        {
            try
            {
                File.Delete(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteDirectory(string directory)
        {
            try
            {
                CleanDirectory(directory);
                Directory.Delete(directory);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            string[] dirs = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);

            if (dirs.Length == 0 && files.Length == 0)
                return true;
            else
                return false;
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool CreateDirectory(string path)
        {
            try
            {
                if(!DirectoryExists(path))
                    Directory.CreateDirectory(path);
                return true;
            }
            catch(Exception e)
            {
                Debugging.Debugger.Log(e.Message);
                return false;
            }
        }

        public static void CopyDirectory(string sourceFolder, string destFolder)
        {
            if (!DirectoryExists(destFolder))
                CreateDirectory(destFolder);

            IEnumerable<string> files = GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string newFile = file.Replace(sourceFolder, destFolder);
                
                if (!DirectoryExists(Path.GetDirectoryName(newFile)))
                    CreateDirectory(Path.GetDirectoryName(newFile));
                File.Copy(file, newFile);
            }
        }

        public static string PathCombine(string dir, string fileName)
        {
            return Path.Combine(dir, Path.GetFileName(fileName));
        }

        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
                using (File.Create(filePath)) { }
        }

        public static bool IsFileLocked(string file)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete | FileShare.ReadWrite);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                    stream.Close();
                }
            }
            
            return false;
        }

        public static void FileCopy(string source, string dest, bool overwrite = true)
        {
            try
            {
                File.Copy(source, dest, overwrite);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void FileMove(string source, string dest)
        {
            try
            {
                File.Move(source, dest);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static void FileRename(string file, string newName)
        {
            File.Move(file, Path.Combine(Path.GetDirectoryName(file), newName));
        }

        private static bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }

        public static void DownloadFile(string url, string localPath)
        {
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            using (WebClient client = new WebClient())
            {
                if (SettingsManager.ENABLE_FTP)
                    client.Credentials = new NetworkCredential(SettingsManager.FTP_USERNAME, SettingsManager.FTP_PASSWORD);
                client.DownloadFile(url, localPath);
            }
        }

        public static string DownloadFileToString(string url)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
                using (WebClient client = new WebClient())
                {
                    if (SettingsManager.ENABLE_FTP)
                        client.Credentials = new NetworkCredential(SettingsManager.FTP_USERNAME, SettingsManager.FTP_PASSWORD);
                    return Encoding.UTF8.GetString(client.DownloadData(url));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CreateShortcut(string targetFile, string shortcutFile, bool asAdmin = true)
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            object shell = Activator.CreateInstance(t);
            try
            {
                object lnk = t.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell, new object[] { shortcutFile });
                try
                {
                    t.InvokeMember("TargetPath", BindingFlags.SetProperty, null, lnk, new object[] { @targetFile });
                    t.InvokeMember("Arguments", BindingFlags.SetProperty, null, lnk, new object[] { "-popupwindow" });
                    t.InvokeMember("IconLocation", BindingFlags.SetProperty, null, lnk, new object[] { "shell32.dll, 2" });
                    t.InvokeMember("Save", BindingFlags.InvokeMethod, null, lnk, null);

                    if (asAdmin)
                    {
                        using (FileStream fs = new FileStream(shortcutFile, FileMode.Open, FileAccess.ReadWrite))
                        {
                            fs.Seek(21, SeekOrigin.Begin);
                            fs.WriteByte(0x22);
                        }
                    }
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }
        
    }
}