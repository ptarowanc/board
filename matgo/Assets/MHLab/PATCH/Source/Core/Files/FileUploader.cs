using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MHLab.PATCH.Utilities;
using System.Net;
using System.IO;
using System.Net.FtpClient;

namespace MHLab.PATCH.Uploader
{
    public enum Protocol
    {
        FTP,
        SFTP
    }

    public class FileUploader
    {
        string hostname;
        int port;
#pragma warning disable 414
        Protocol protocol;
#pragma warning restore 414
        string username;
        string password;
        

        public FileUploader(Protocol protocol, string host, int port, string username, string password)
        {
            this.hostname = host;
            this.port = port;
            this.protocol = protocol;
            this.username = username;
            this.password = password;
        }

        public void UploadFile(string localFile, string remotePath)
        {
            try
            {
                using (FtpClient conn = new FtpClient())
                {
                    conn.Host = this.hostname;
                    conn.Port = this.port;
                    conn.Credentials = new NetworkCredential(this.username, this.password);
                    conn.Connect();

                    conn.CreateDirectory(remotePath);

                    using (Stream s = conn.OpenWrite(Path.Combine(remotePath, Path.GetFileName(localFile))))
                    {
                        try
                        {
                            using (FileStream fileStream = File.OpenRead(localFile))
                            {
                                fileStream.CopyTo(s);
                            }
                        }
                        finally
                        {
                            s.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CreateFTPDirectory(string directory)
        {

            try
            {
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(this.username, this.password);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}