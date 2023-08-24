using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MHLab.PATCH.Settings;

namespace MHLab.PATCH.Downloader
{
    /// <summary>
    /// Downloads and resumes files from HTTP, FTP, and File (file://) URLS
    /// </summary>
    internal class FileDownloader
    {
        // Block size to download is by default 8M.
        private int downloadBlockSize = SettingsManager.DOWNLOAD_BUFFER_SIZE;//1048576;

        // Determines whether the user has canceled or not.
        private bool canceled = false;

        private string downloadingTo;

        /// <summary>
        /// This is the name of the file we get back from the server when we
        /// try to download the provided url. It will only contain a non-null
        /// string when we've successfully contacted the server and it has started
        /// sending us a file.
        /// </summary>
        public string DownloadingTo
        {
            get { return downloadingTo; }
        }

        public void Cancel()
        {
            this.canceled = true;
        }

        /// <summary>
        /// Progress update
        /// </summary>
        public event DownloadProgressHandler ProgressChanged;

        private IWebProxy proxy = null;

        /// <summary>
        /// Proxy to be used for http and ftp requests.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        /// <summary>
        /// Fired when progress reaches 100%.
        /// </summary>
        public event EventHandler DownloadComplete;

        private void OnDownloadComplete()
        {
            if (this.DownloadComplete != null)
                this.DownloadComplete(this, new EventArgs());
        }

        /// <summary>
        /// Begin downloading the file at the specified url, and save it to the current folder.
        /// </summary>
        public void Download(string url)
        {
            Download(url, "");
        }
        /// <summary>
        /// Begin downloading the file at the specified url, and save it to the given folder.
        /// </summary>
        public void Download(string url, string destFolder)
        {
            DownloadData data = null;
            this.canceled = false;

            try
            {
                // get download details                
                data = DownloadData.Create(url, destFolder, this.proxy);
                // Find out the name of the file that the web server gave us.
                string destFileName = Path.GetFileName(data.Response.ResponseUri.ToString());


                // The place we're downloading to (not from) must not be a URI,
                // because Path and File don't handle them...
                destFolder = destFolder.Replace("file:///", "").Replace("file://", "");
                this.downloadingTo = Path.Combine(destFolder, destFileName);

                // Create the file on disk here, so even if we don't receive any data of the file
                // it's still on disk. This allows us to download 0-byte files.
                if (!File.Exists(downloadingTo))
                {
                    using (FileStream fs = File.Create(downloadingTo))
                    {
                        fs.Dispose();
                        fs.Close();
                    }
                }

                // create the download buffer
                byte[] buffer = new byte[downloadBlockSize];

                int readCount;

                // update how many bytes have already been read
                long totalDownloaded = data.StartPoint;

                bool gotCanceled = false;


                using (FileStream fs = File.Open(downloadingTo, FileMode.Append, FileAccess.Write, FileShare.Write | FileShare.Delete))
                {
                    while ((int)(readCount = data.DownloadStream.Read(buffer, 0, downloadBlockSize)) > 0)
                    {
                        // break on cancel
                        if (canceled)
                        {
                            gotCanceled = true;
                            data.Close();
                            break;
                        }

                        // update total bytes read
                        totalDownloaded += readCount;

                        // save block to end of file
                        SaveToFile(buffer, readCount, fs);

                        // send progress info
                        if (data.IsProgressKnown)
                            RaiseProgressChanged(totalDownloaded, data.FileSize);

                        // break on cancel
                        if (canceled)
                        {
                            gotCanceled = true;
                            data.Close();
                            break;
                        }
                    }
                    fs.Dispose();
                    fs.Close();
                }

                if (!gotCanceled)
                    OnDownloadComplete();
            }
            catch (UriFormatException e)
            {
                throw new ArgumentException(
                    String.Format("Could not parse the URL \"{0}\" - it's either malformed or is an unknown protocol.", url), e);
            }
            finally
            {
                if (data != null)
                    data.Close();
            }
        }

        /// <summary>
        /// Download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void Download(List<string> urlList)
        {
            this.Download(urlList, "");
        }
        /// <summary>
        /// Download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void Download(List<string> urlList, string destFolder)
        {
            // validate input
            if (urlList == null)
                throw new ArgumentException("Url list not specified.");

            if (urlList.Count == 0)
                throw new ArgumentException("Url list empty.");

            // try each url in the list.
            // if one succeeds, we are done.
            // if any fail, move to the next.
            Exception ex = null;
            foreach (string s in urlList)
            {
                ex = null;
                try
                {
                    Download(s, destFolder);
                }
                catch (Exception e)
                {
                    ex = e;
                }
                // If we got through that without an exception, we found a good url
                if (ex == null)
                    break;
            }
            if (ex != null)
                throw ex;
        }

        /// <summary>
        /// Asynchronously download a file from the url.
        /// </summary>
        public void AsyncDownload(string url)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new string[] { url, "" });
        }
        /// <summary>
        /// Asynchronously download a file from the url to the destination folder.
        /// </summary>
        public void AsyncDownload(string url, string destFolder)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new string[] { url, destFolder });
        }
        /// <summary>
        /// Asynchronously download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void AsyncDownload(List<string> urlList, string destFolder)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new object[] { urlList, destFolder });
        }
        /// <summary>
        /// Asynchronously download a file from a list or URLs. If downloading from one of the URLs fails,
        /// another URL is tried.
        /// </summary>
        public void AsyncDownload(List<string> urlList)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                new System.Threading.WaitCallback(this.WaitCallbackMethod), new object[] { urlList, "" });
        }
        /// <summary>
        /// A WaitCallback used by the AsyncDownload methods.
        /// </summary>
        private void WaitCallbackMethod(object data)
        {
            // Can either be a string array of two strings (url and dest folder),
            // or an object array containing a list<string> and a dest folder
            if (data is string[])
            {
                String[] strings = data as String[];
                this.Download(strings[0], strings[1]);
            }
            else
            {
                Object[] list = data as Object[];
                List<String> urlList = list[0] as List<String>;
                String destFolder = list[1] as string;
                this.Download(urlList, destFolder);
            }
        }
        private void SaveToFile(byte[] buffer, int count, FileStream file)
        {
            /*f = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Write | FileShare.Delete);
            if (f != null)
            {
                lock (f)
                {
                    f.Write(buffer, 0, count);
                }
                f.Dispose();
                f.Close();
            }*/
            try
            {
                file.Write(buffer, 0, count);
            }
            catch(Exception e)
            {
                throw new Exception(
                    String.Format("Error trying to save file \"{0}\": {1}", file.Name, e.Message), e);
            }
        }

        //private DateTime _lastTime = DateTime.Now;
        //private long _lastFileSize = 0;
        private void RaiseProgressChanged(long current, long target)
        {
            //TimeSpan timeDiff = _lastTime - DateTime.Now;
            if (this.ProgressChanged != null)
                this.ProgressChanged(this, new DownloadEventArgs(target, current/*, timeDiff, (current - _lastFileSize)*/));
            //_lastFileSize = current;
            //_lastTime = DateTime.Now;
        }
    }

    /// <summary>
    /// Constains the connection to the file server and other statistics about a file
    /// that's downloading.
    /// </summary>
    class DownloadData
    {
        private WebResponse response;

        private Stream stream;
        private long size;
        private long start;

        private IWebProxy proxy = null;

        public static DownloadData Create(string url, string destFolder)
        {
            return Create(url, destFolder, null);
        }

        public static DownloadData Create(string url, string destFolder, IWebProxy proxy)
        {

            // This is what we will return
            DownloadData downloadData = new DownloadData();
            downloadData.proxy = proxy;

            long urlSize = downloadData.GetFileSize(url);
            downloadData.size = urlSize;

            WebRequest req = downloadData.GetRequest(url);
            try
            {
                downloadData.response = (WebResponse)req.GetResponse();
            }
            catch (Exception e)
            {
                throw new ArgumentException(String.Format(
                    "Error downloading \"{0}\": {1}", url, e.Message), e);
            }

            // Check to make sure the response isn't an error. If it is this method
            // will throw exceptions.
            ValidateResponse(downloadData.response, url);

            // Take the name of the file given to use from the web server.
            String fileName = System.IO.Path.GetFileName(downloadData.response.ResponseUri.ToString());

            String downloadTo = Path.Combine(destFolder, fileName);

            // If we don't know how big the file is supposed to be,
            // we can't resume, so delete what we already have if something is on disk already.
            if (!downloadData.IsProgressKnown && File.Exists(downloadTo))
                File.Delete(downloadTo);

            if (downloadData.IsProgressKnown && File.Exists(downloadTo))
            {
                // We only support resuming on http requests
                if (!(downloadData.Response is HttpWebResponse))
                {
                    File.Delete(downloadTo);
                }
                else
                {
                    // Try and start where the file on disk left off
                    downloadData.start = new FileInfo(downloadTo).Length;

                    // If we have a file that's bigger than what is online, then something 
                    // strange happened. Delete it and start again.
                    if (downloadData.start > urlSize)
                        File.Delete(downloadTo);
                    else if (downloadData.start < urlSize)
                    {
                        // Try and resume by creating a new request with a new start position
                        downloadData.response.Close();
                        req = downloadData.GetRequest(url);
                        ((HttpWebRequest)req).AddRange((int)downloadData.start);
                        downloadData.response = req.GetResponse();

                        if (((HttpWebResponse)downloadData.Response).StatusCode != HttpStatusCode.PartialContent)
                        {
                            // They didn't support our resume request. 
                            File.Delete(downloadTo);
                            downloadData.start = 0;
                        }
                    }
                }
            }
            return downloadData;
        }

        // Used by the factory method
        private DownloadData()
        {
        }

        private DownloadData(WebResponse response, long size, long start)
        {
            this.response = response;
            this.size = size;
            this.start = start;
            this.stream = null;
        }

        /// <summary>
        /// Checks whether a WebResponse is an error.
        /// </summary>
        /// <param name="response"></param>
        private static void ValidateResponse(WebResponse response, string url)
        {
            if (response is HttpWebResponse)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                // If it's an HTML page, it's probably an error page. Comment this
                // out to enable downloading of HTML pages.
                if (httpResponse.ContentType != null)
                {
                    if (/*httpResponse.ContentType.Contains("text/html") ||*/ httpResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        /*throw new ArgumentException(
                            String.Format("Could not download \"{0}\" - a web page was returned from the web server.",
                            url));*/
                        return;
                    }
                }
            }
            else if (response is FtpWebResponse)
            {
                FtpWebResponse ftpResponse = (FtpWebResponse)response;
                if (ftpResponse.StatusCode == FtpStatusCode.ConnectionClosed)
                    throw new ArgumentException(
                        String.Format("Could not download \"{0}\" - FTP server closed the connection.", url));
            }
            // FileWebResponse doesn't have a status code to check.
        }

        /// <summary>
        /// Checks the file size of a remote file. If size is -1, then the file size
        /// could not be determined.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="progressKnown"></param>
        /// <returns></returns>
        private long GetFileSize(string url)
        {
            WebResponse response = null;
            long size = -1;
            try
            {
                response = GetRequest(url).GetResponse();
                size = response.ContentLength;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return size;
        }

        private bool RemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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

        private WebRequest GetRequest(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;
            WebRequest request = WebRequest.Create(url);
            if (request is HttpWebRequest)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy.GetProxy(new Uri("http://www.google.com"));
            }
            if(request is FtpWebRequest)
            {
                request.Credentials = new NetworkCredential(SettingsManager.FTP_USERNAME, SettingsManager.FTP_PASSWORD);
            }

            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            return request;
        }

        public void Close()
        {
            this.response.Close();
        }

#region Properties
        public WebResponse Response
        {
            get { return response; }
            set { response = value; }
        }
        public Stream DownloadStream
        {
            get
            {
                if (this.start == this.size)
                    return Stream.Null;
                if (this.stream == null)
                    this.stream = this.response.GetResponseStream();
                return this.stream;
            }
        }
        public long FileSize
        {
            get
            {
                return this.size;
            }
        }
        public long StartPoint
        {
            get
            {
                return this.start;
            }
        }
        public bool IsProgressKnown
        {
            get
            {
                // If the size of the remote url is -1, that means we
                // couldn't determine it, and so we don't know
                // progress information.
                return this.size > -1;
            }
        }
#endregion
    }

    /// <summary>
    /// Progress of a downloading file.
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        private int percentDone;
        private string downloadState;
        private long totalFileSize;

        /*private TimeSpan timeDiff;
        private long sizeDiff;

        public uint DownloadSpeed
        {
            get
            {
                return (uint)Math.Floor((double)(sizeDiff) / timeDiff.TotalSeconds);
            }
        }*/

        public long TotalFileSize
        {
            get { return totalFileSize; }
            set { totalFileSize = value; }
        }
        private long currentFileSize;

        public long CurrentFileSize
        {
            get { return currentFileSize; }
            set { currentFileSize = value; }
        }

        public DownloadEventArgs(long totalFileSize, long currentFileSize/*, TimeSpan timeDiff, long sizeDiff*/)
        {
            this.totalFileSize = totalFileSize;
            this.currentFileSize = currentFileSize;

            this.percentDone = (int)((((double)currentFileSize) / totalFileSize) * 100);

            /*this.timeDiff = timeDiff;
            this.sizeDiff = sizeDiff;*/
        }

        public DownloadEventArgs(string state)
        {
            this.downloadState = state;
        }

        public DownloadEventArgs(int percentDone, string state)
        {
            this.percentDone = percentDone;
            this.downloadState = state;
        }

        public int PercentDone
        {
            get
            {
                return this.percentDone;
            }
        }

        public string DownloadState
        {
            get
            {
                return this.downloadState;
            }
        }
    }
    public delegate void DownloadProgressHandler(object sender, DownloadEventArgs e);
}