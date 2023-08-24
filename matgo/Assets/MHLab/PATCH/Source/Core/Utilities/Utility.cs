using System.Reflection;
using System;
//using System.Net.NetworkInformation;

namespace MHLab.PATCH.Utilities
{
    public class Utility
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string FormatSizeBinary(Int64 size, Int32 decimals)
        {
            String[] sizes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };
            Double formattedSize = size;
            Int32 sizeIndex = 0;
            while (formattedSize >= 1024 && sizeIndex < sizes.Length)
            {
                formattedSize /= 1024;
                sizeIndex += 1;
            }
            return Math.Round(formattedSize, decimals) + sizes[sizeIndex];
        }
        public static string FormatSizeDecimal(Int64 size, Int32 decimals)
        {
            String[] sizes = { "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            Double formattedSize = size;
            Int32 sizeIndex = 0;
            while (formattedSize >= 1000 && sizeIndex < sizes.Length)
            {
                formattedSize /= 1000;
                sizeIndex += 1;
            }
            return Math.Round(formattedSize, decimals) + sizes[sizeIndex];
        }

        public static bool IsRemoteServiceAvailable(string url)
        {
            try
            {
                // For now seems that this raises problems on certain situations.
                // TODO: test this.
                /*if (url.ToLower().Contains("ftp://"))
                {
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url);
                    request.Timeout = 3000;
                    //request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    if(Settings.SettingsManager.ENABLE_FTP)
                        request.Credentials = new NetworkCredential(Settings.SettingsManager.FTP_USERNAME, Settings.SettingsManager.FTP_PASSWORD);

                    using (var response = request.GetResponse())
                    {
                        return true;
                    }
                }
                else
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Timeout = 3000;
                    request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                    request.Method = "HEAD";

                    using (var response = request.GetResponse())
                    {
                        return true;
                    }
                }*/

                /*Ping sender = new Ping();
                Uri uri = new Uri(url);
                PingReply reply = sender.Send(uri.Host);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }*/
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
