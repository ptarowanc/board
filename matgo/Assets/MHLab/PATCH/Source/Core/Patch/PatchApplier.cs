using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MHLab.PATCH.Utilities;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Debugging;
using MHLab.PATCH.Compression;
using MHLab.PATCH.Downloader;
using Octodiff.Core;

namespace MHLab.PATCH
{
    internal class PatchApplier
    {
        #region Members
        //List<string> _clientFiles;
        List<Patch> _patches;
        private Dictionary<string, string> _downloadedArchiveFilesHashes;

        public Action<string> OnFileProcessed;
        public Action<string> OnFileProcessing;
        public Action<string, string> OnLog;
        public Action<string, string, Exception> OnError;
        public Action<string, string, Exception> OnFatalError;
        public Action<string> OnTaskStarted;
        public Action<string> OnTaskCompleted;
        public Action<int, int> OnSetMainProgressBar;
        public Action<int, int> OnSetDetailProgressBar;
        public Action OnIncreaseMainProgressBar;
        public Action OnIncreaseDetailProgressBar;
        public Action<long, long, int> OnDownloadProgress;
        public Action OnDownloadCompleted;

        public bool IsDirtyWorkspace = false;
        #endregion

        #region Constructor
        public PatchApplier()
        {
            //_clientFiles = new List<string>();
            _patches = new List<Patch>();

            // Initialize callbacks
            OnFileProcessed = delegate (string s) { };
            OnFileProcessing = delegate (string s) { };
            OnLog = delegate (string s, string s1) { };
            OnError = delegate (string s, string s1, Exception e) { };
            OnFatalError = delegate (string s, string s1, Exception e) { };
            OnTaskStarted = delegate (string s) { };
            OnTaskCompleted = delegate (string s) { };
            OnSetMainProgressBar = delegate (int i1, int i2) { };
            OnSetDetailProgressBar = delegate (int i1, int i2) { };
            OnIncreaseMainProgressBar = delegate () { };
            OnIncreaseDetailProgressBar = delegate () { };
            OnDownloadProgress = delegate (long l1, long l2, int i1) { };
            OnDownloadCompleted = delegate () { };
        }
        #endregion

        #region Public interface
        public void CheckForUpdates()
        {
            OnTaskStarted("Patching process is started!");
            OnSetMainProgressBar(0, 4);

            // Clear workspace, delete all temp directories that eventually exist
            ClearPatchWorkspace();

            //Installer.Install();

            // Get current version stored in "version" file
            string sCurrentVersion = GetCurrentVersion();
            OnLog("Checking version...", "Found version " + sCurrentVersion);
            OnIncreaseMainProgressBar();

            if (sCurrentVersion != null)
            {
                Version currentVersion = new Version(sCurrentVersion);

                // Check for remote versions.txt file that contains all patches list, hashes and compression type
                if (UpdateVersions())
                {
                    OnIncreaseMainProgressBar();
                    //Debugger.Log("_patches: " + this._patches.Count);
                    List<Patch> patches = this._patches.Where(p => p.From.Equals(currentVersion)).ToList();
                    patches.Sort(
                        delegate (Patch p1, Patch p2)
                        {
                            return p2.To.CompareTo(p1.To);
                        }
                    );

                    //Debugger.Log("patches: " + patches.Count);

                    while (patches.Count > 0)
                    {
                        ClearPatchWorkspace();

                        PerformUpdate(patches[0]);

                        currentVersion = new Version(GetCurrentVersion());

                        patches = this._patches.Where(p => p.From.Equals(currentVersion)).ToList();
                        patches.Sort(
                            delegate (Patch p1, Patch p2)
                            {
                                return p2.To.CompareTo(p1.To);
                            }
                        );
                    }
                    OnIncreaseMainProgressBar();

                    ClearPatchWorkspace();
                    OnIncreaseMainProgressBar();

                    OnTaskCompleted("Patching process is now completed! Press LAUNCH button!");
                }
                else
                {
                    OnIncreaseMainProgressBar();
                    OnIncreaseMainProgressBar();
                    ClearPatchWorkspace();
                    OnIncreaseMainProgressBar();
                    OnTaskCompleted("No remote patches available!");
                }
            }
            else
            {
                Debugger.Log("Local version checking error! Please check your version file!");
                OnFatalError("Patch error!", "Local version checking error! Please check your version file!", new Exception("Local version checking error! Please check your version file!"));
            }
        }

        public string GetCurrentVersion()
        {
            try
            {
                if (!FileManager.FileExists(SettingsManager.PATCH_VERSION_PATH)) return null;
                string cypherVersion = File.ReadAllText(SettingsManager.PATCH_VERSION_PATH).Replace("\n", "").Replace("\r", "");
                string version = Rijndael.Decrypt(cypherVersion, SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD);
                return version;
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
                OnFatalError("Patch error!", "Local version checking error! Please check your version file!", e);
                return null;
            }
        }
        #endregion

        #region Methods
        private void ClearPatchWorkspace()
        {
            OnLog("Cleaning!", "Cleaning P.A.T.C.H. workspace!");
            OnSetDetailProgressBar(0, 2);

            if (FileManager.DirectoryExists(SettingsManager.PATCHES_TMP_FOLDER))
            {
                FileManager.DeleteDirectory(SettingsManager.PATCHES_TMP_FOLDER);
            }
            OnIncreaseDetailProgressBar();

            if (FileManager.DirectoryExists(SettingsManager.PATCH_SAFE_BACKUP))
            {
                FileManager.DeleteDirectory(SettingsManager.PATCH_SAFE_BACKUP);
            }
            OnIncreaseDetailProgressBar();

            CleanSelfDeletingFiles();
        }

        private void CleanSelfDeletingFiles()
        {
            /*IEnumerable<string> files = FileManager.GetFiles(SettingsManager.APP_PATH);
            foreach(string file in files)
            {
                if(Path.GetExtension(file) == ".delete_me")
                {
                    FileManager.DeleteFiles(SettingsManager.APP_PATH, "*.delete_me");
                }
            }*/
            FileManager.DeleteFiles(SettingsManager.APP_PATH, "*.delete_me");
        }

        private bool UpdateVersions()
        {
            OnLog("Checking...", "Checking for remote service!");
            if (!Utility.IsRemoteServiceAvailable(SettingsManager.VERSIONS_FILE_DOWNLOAD_URL))
            {
                OnFatalError("Error!", "No remote service is responding! Try again!", new Exception("No remote service is responding! Try again!"));
                return false;
            }

            OnSetDetailProgressBar(0, 2);
            string versions = null;
            try
            {
                versions = FileManager.DownloadFileToString(SettingsManager.VERSIONS_FILE_DOWNLOAD_URL);
            }
            catch
            {
                OnFatalError("Error!", "Can't find remote versions list!", new Exception("Can't find remote versions list!"));
            }
            OnIncreaseDetailProgressBar();
            //Debugger.Log(versions);
            if (!String.IsNullOrEmpty(versions))
            {
                string[] entries = versions.Split('\n');
                foreach (string entry in entries)
                {
                    if (entry != "")
                    {
                        this._patches.Add(new Patch(entry));
                    }
                }
                OnIncreaseDetailProgressBar();
                return true;
            }
            else
            {
                OnIncreaseDetailProgressBar();
                return false;
            }
        }

        private void PerformUpdate(Patch p)
        {
            bool downloadResponse = DownloadPatchArchiveFile(p);
            bool archiveHashChecking = false;

            // Retry to download patch archive if it fails first time.
            // If download success, calculate archive hash and set flag as true.
            #region Download retrying
            if (!downloadResponse)
            {
                for (int i = 1; i <= SettingsManager.PATCH_DOWNLOAD_RETRY_ATTEMPTS; i++)
                {
                    downloadResponse = DownloadPatchArchiveFile(p);
                    if (downloadResponse)
                    {
                        OnLog("Hash checking!", "Comparing hashes for " + p.ArchiveName);
                        string hash = Hashing.SHA1(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName);
                        if (hash == p.Hash)
                        {
                            //Debug.Log("Archive " + v.ArchiveName + " hash: " + hash);
                            archiveHashChecking = true;
                            break;
                        }
                        else
                        {
                            Debugger.Log("Archive " + p.ArchiveName + " is corrupted, hash: " + hash);
                            downloadResponse = false;
                            archiveHashChecking = false;
                            OnLog("Issue detected!", "An archive is corrupted, retrying..." + i);
                        }
                    }
                    else
                    {
                        Debugger.Log("Can't download archive " + p.ArchiveName);
                        downloadResponse = false;
                        archiveHashChecking = false;
                        OnLog("Issue detected!", "Can't download an archive, retrying..." + i);
                    }
                }
            }
            else
            {
                OnLog("Hash checking!", "Comparing hashes for " + p.ArchiveName);
                string hash = Hashing.SHA1(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName);

                archiveHashChecking = hash == p.Hash;
            }
            #endregion

            if (downloadResponse)
            {
                if (archiveHashChecking)
                {
                    OnSetDetailProgressBar(0, 3);
                    OnLog("Patch decompression!", "Unzipping pack " + p.ArchiveName);

                    Directory.CreateDirectory(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName);
                    OnIncreaseDetailProgressBar();

                    Compressor.Decompress(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName, SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName, p.Type, null);
                    OnIncreaseDetailProgressBar();

                    IEnumerable<string> files = FileManager.GetFiles(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName);
                    OnIncreaseDetailProgressBar();

                    OnLog("Processing index!", "Computing indexer for " + p.ArchiveName);
                    
                    ProcessPatchIndexer(p);

                    ProcessFiles(files, p);

                    ClearPatchWorkspace();
                }
                else
                {
                    Debugger.Log("Hash checking failed! Try to reopen the launcher! Archive: " + p.ArchiveName);
                    OnFatalError("Patch failed!", "Hash checking failed! Try to reopen the launcher!", new Exception("Hash checking failed! Try to reopen the launcher!"));
                }
            }
            else
            {
                OnFatalError("Patch failed!", "Download failed! Try to reopen the launcher!", new Exception("Download failed! Try to reopen the launcher!"));
            }
        }

        private void ProcessFiles(IEnumerable<string> files, Patch p)
        {
            OnSetDetailProgressBar(0, files.Count());
            foreach (string file in files)
            {
                OnFileProcessing(file.Replace(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName, ""));
                string currentBuildFile = file.Replace(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName, SettingsManager.APP_PATH);
                
                ProcessFile(currentBuildFile, file);

                OnIncreaseDetailProgressBar();
                OnFileProcessed(file.Replace(SettingsManager.PATCHES_TMP_FOLDER + p.PatchName, ""));
            }
        }

        private void ProcessFile(string oldFile, string newFile)
        {
            try
            {
                if (!FileManager.DirectoryExists(oldFile.Replace(SettingsManager.APP_PATH, SettingsManager.PATCH_SAFE_BACKUP)))
                {
                    FileManager.CreateDirectory(Path.GetDirectoryName(oldFile.Replace(SettingsManager.APP_PATH, SettingsManager.PATCH_SAFE_BACKUP)));
                }

                switch (Path.GetExtension(newFile))
                {
                    case ".delete":
                        OnLog("File processing - Deleting", "Deleting " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");
                        
                        //FileManager.DeleteFile(oldFile.Replace(".delete", ""));
                        FileManager.FileMove(oldFile.Replace(".delete", ""), oldFile.Replace(".delete", "").Replace(SettingsManager.APP_PATH, SettingsManager.PATCH_SAFE_BACKUP));

                        OnLog("File processed - Deleted", "Deleted " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");
                        break;
                    case ".patch":
                        OnLog("File processing - Patching", "Patching " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");


                        if (FileManager.IsFileLocked(oldFile.Replace(".patch", "")))
                        {
                            string newName = oldFile.Replace(".patch", "") + ".delete_me";
                            FileManager.FileRename(oldFile.Replace(".patch", ""), newName);
                            FileManager.FileCopy(FileManager.PathCombine(Path.GetDirectoryName(oldFile.Replace(".patch", "")), newName), oldFile.Replace(".patch", ""));
                            this.IsDirtyWorkspace = true;
                        }

                        FileManager.FileCopy(oldFile.Replace(".patch", ""), oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, SettingsManager.PATCH_SAFE_BACKUP));
                        ApplyOctodiffPatch(oldFile.Replace(".patch", ""), newFile);

                        string patchedHash = Hashing.SHA1(oldFile.Replace(".patch", ""));
                        string keyToFind = oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH + Path.DirectorySeparatorChar, "").Replace('\\', '/');

                        //Debugger.Log(keyToFind);

                        string computedPatchHash = this._downloadedArchiveFilesHashes[keyToFind];

                        Debugger.Log("Computed hash: " + patchedHash + " - Expected hash: " + computedPatchHash);

                        if (computedPatchHash == patchedHash)
                        {
                            OnLog("File processing - Hash checking", "Hash checking " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");
                        }
                        else
                        {
                            OnLog("Patch failed!", "Hash checking on " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file failed!");

                            RollbackPatch();
                            ClearPatchWorkspace();

                            // Patch failed
                            OnFatalError("Patch failed!", "A file (" + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + ") is corrupted, restored previous version! Try to reopen the launcher!", new Exception("A file (" + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + ") is corrupted, restored previous version! Try to reopen the launcher!"));
                        }

                        OnLog("File processed - Patched", "Patched " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");
                        break;
                    default:
                        OnLog("File processing - Creating", "Creating new " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");

                        FileManager.CreateFile(oldFile.Replace(SettingsManager.APP_PATH, SettingsManager.PATCH_SAFE_BACKUP));
                        FileManager.CreateDirectory(Path.GetDirectoryName(oldFile));
                        FileManager.FileCopy(newFile, oldFile, true);

                        OnLog("File processed - Created", "Created new " + oldFile.Replace(".patch", "").Replace(SettingsManager.APP_PATH, "") + " file");
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLog("Patch failed!", ex.Message);

                RollbackPatch();
                ClearPatchWorkspace();

                // Patch failed
                OnFatalError("Patch failed!", "Something goes wrong, restored previous version! Try to reopen the launcher!", new Exception("Something goes wrong, restored previous version! Try to reopen the launcher!"));
            }
        }

        private void ProcessPatchIndexer(Patch p)
        {
            OnSetDetailProgressBar(0, 1);
            this._downloadedArchiveFilesHashes = new Dictionary<string, string>();
            string text = Rijndael.Decrypt(File.ReadAllText(SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName), SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD);
            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                if (line != "")
                {
                    string[] splitted = line.Split(SettingsManager.PATCHES_SYMBOL_SEPARATOR);
                    this._downloadedArchiveFilesHashes.Add(splitted[0], splitted[1].Replace("\r", ""));
                }
            }
            OnIncreaseDetailProgressBar();
        }

        private void ApplyOctodiffPatch(string oldFile, string patchFile)
        {
            try
            {
                var delta = new DeltaApplier
                {
                    SkipHashCheck = true
                };

                if(FileManager.FileExists(oldFile + ".bak"))
                    FileManager.DeleteFile(oldFile + ".bak");
                File.Move(oldFile, oldFile + ".bak");

                using (var basisStream = new FileStream(oldFile + ".bak", FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var deltaStream = new FileStream(patchFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var newFileStream = new FileStream(oldFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    delta.Apply(basisStream, new BinaryDeltaReader(deltaStream, null), newFileStream);
                }
                FileManager.DeleteFile(oldFile + ".bak");
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
            }
        }

        private void RollbackPatch()
        {
            OnLog("Rollback!", "Rollingback to previous version!");

            if (!FileManager.DirectoryExists(SettingsManager.PATCH_SAFE_BACKUP))
                return;

            IEnumerable<string> files = FileManager.GetFiles(SettingsManager.PATCH_SAFE_BACKUP);
            OnSetDetailProgressBar(0, files.Count());

            foreach (string file in files)
            {
                switch (Path.GetExtension(file))
                {
                    case ".delete":
                        FileManager.DeleteFile(file.Replace(".delete", "").Replace(SettingsManager.PATCH_SAFE_BACKUP, SettingsManager.APP_PATH));
                        break;
                    default:
                        FileManager.DeleteFile(file.Replace(SettingsManager.PATCH_SAFE_BACKUP, SettingsManager.APP_PATH));
                        FileManager.FileMove(file, file.Replace(SettingsManager.PATCH_SAFE_BACKUP, SettingsManager.APP_PATH));
                        break;
                }
                OnIncreaseDetailProgressBar();
            }
        }

        /*private bool DownloadPatchArchiveFile(Patch p)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    if (SettingsManager.ENABLE_FTP)
                        client.Credentials = new NetworkCredential(SettingsManager.FTP_USERNAME, SettingsManager.FTP_PASSWORD);

                    OnLog("Downloading file...", "Downloading patch archive from remote server!");

                    Directory.CreateDirectory(SettingsManager.PATCHES_TMP_FOLDER);
                    if(FileManager.FileExists(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName))
                    {
                        FileManager.DeleteFile(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName);
                    }
                    if (FileManager.FileExists(SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName))
                    {
                        FileManager.DeleteFile(SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName);
                    }
                    client.DownloadFile(SettingsManager.PATCHES_DOWNLOAD_URL + p.ArchiveName, SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName);
                    client.DownloadFile(SettingsManager.PATCHES_DOWNLOAD_URL + p.IndexerName, SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName);

                    OnLog("Downloaded file!", "Downloaded patch archive from remote server!");

                    return true;
                }
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
                return false;
            }
        }*/

        private bool DownloadPatchArchiveFile(Patch p)
        {
            /*try
            {*/
                FileDownloader downloader = new FileDownloader();
                downloader.ProgressChanged += downloader_ProgressChanged;
                downloader.DownloadComplete += downloader_Completed;
                OnLog("Checking directories...", "Checking directories and workspace creation!");

                Directory.CreateDirectory(SettingsManager.PATCHES_TMP_FOLDER);
                if (FileManager.FileExists(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName))
                {
                    FileManager.DeleteFile(SettingsManager.PATCHES_TMP_FOLDER + p.ArchiveName);
                }
                if (FileManager.FileExists(SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName))
                {
                    FileManager.DeleteFile(SettingsManager.PATCHES_TMP_FOLDER + p.IndexerName);
                }

                OnLog("Downloading file...", "Downloading patch " + p.PatchName + " indexer from remote server!");
                OnSetDetailProgressBar(0, 100);
                downloader.Download(SettingsManager.PATCHES_DOWNLOAD_URL + p.IndexerName, SettingsManager.PATCHES_TMP_FOLDER);// + p.IndexerName);
                OnLog("Downloaded file!", "Downloaded patch " + p.PatchName + " indexer from remote server!");

                OnLog("Downloading file...", "Downloading patch " + p.PatchName + " archive from remote server!");
                OnSetDetailProgressBar(0, 100);
                downloader.Download(SettingsManager.PATCHES_DOWNLOAD_URL + p.ArchiveName, SettingsManager.PATCHES_TMP_FOLDER);// + p.ArchiveName);
                OnLog("Downloaded file!", "Downloaded patch " + p.PatchName + " archive from remote server!");
                                
                return true;
            /*}
            catch (Exception e)
            {
                Debugger.Log(e.Message);
                return false;
            }*/
        }

        private void downloader_ProgressChanged(object sender, DownloadEventArgs e)
        {
            OnDownloadProgress(e.CurrentFileSize, e.TotalFileSize, e.PercentDone/*, e.DownloadSpeed*/);
        }

        private void downloader_Completed(object sender, EventArgs e)
        {
            OnDownloadCompleted();
        }

        #endregion
    }
}
