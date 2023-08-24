using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Utilities;
using MHLab.PATCH.Downloader;
using MHLab.PATCH.Debugging;
using System.IO;

namespace MHLab.PATCH.Install
{
    public enum InstallationState
    {
        FAILED,
        NOT_NEEDED,
        SUCCESS
    }

    class Installer
    {
        internal List<Version> availableBuilds;

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

        public Installer()
        {
            availableBuilds = new List<Version>();
            
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

        public bool IsToInstall()
        {
            if (!SettingsManager.INSTALL_IN_LOCAL_PATH)
                return (FileManager.FileExists(Path.Combine(SettingsManager.PROGRAM_FILES_DIRECTORY_TO_INSTALL_ABS_PATH, "version")) ? false : true);
            else
                return (FileManager.FileExists(SettingsManager.VERSION_FILE_LOCAL_PATH) ? false : true);

        }

        private bool InstallLatestBuild(string pathToInstall)//, bool replaceShortcut = false, bool replacePatcher = false)
        {
            OnLog("Checking...", "Checking for remote service!");
            if (!Utility.IsRemoteServiceAvailable(SettingsManager.BUILDS_DOWNLOAD_URL + "index"))
            {
                OnFatalError("Error!", "No remote service is responding! Try again!", new Exception("No remote service is responding! Try again!"));
                return false;
            }

            OnLog("Downloading file...", "Downloading index file for installation!");
            string indexContent = FileManager.DownloadFileToString(SettingsManager.BUILDS_DOWNLOAD_URL + "index");
            OnIncreaseMainProgressBar();

            if (ProcessBuildsIndex(indexContent))
            {
                OnLog("Starting files copy...", "");

                foreach (Version buildEntry in this.availableBuilds)
                {
                    string build = buildEntry.ToString();
                    try
                    {
                        string buildIndexContent = FileManager.DownloadFileToString(SettingsManager.BUILDS_DOWNLOAD_URL + "index_" + build + ".bix");
                        List<string> files = ProcessBuildIndex(buildIndexContent);

                        if (files != null)
                        {
                            OnSetDetailProgressBar(0, files.Count + 1); //+ ((replaceShortcut) ? 1 : 0) + ((replacePatcher) ? 2 : 0));

                            foreach (string entry in files)
                            {
                                string[] splittedFile = entry.Split(SettingsManager.PATCHES_SYMBOL_SEPARATOR);
                                string file = splittedFile[0];
                                string hash = splittedFile[1];

                                if (file.Equals("version"))
                                {
                                    OnLog("Skipping...", "Skipping version file...");
                                }
                                else if (FileManager.FileExists(Path.Combine(pathToInstall, file)))
                                {
                                    OnLog("Checking...", "Hash checking for " + file + "...");
                                    string alreadyExistingFileHash = Hashing.SHA1(Path.Combine(pathToInstall, file));
                                    if (!hash.Equals(alreadyExistingFileHash))
                                    {
                                        FileManager.DeleteFile(Path.Combine(pathToInstall, file));
                                        //OnLog("Downloading...", "Downloading " + file + "...");
                                        this.DownloadFile(
                                            SettingsManager.BUILDS_DOWNLOAD_URL + build + "/" + file,
                                            Path.Combine(pathToInstall, file)
                                        );
                                    }
                                }
                                else
                                {
                                    FileManager.CreateDirectory(Path.GetDirectoryName(Path.Combine(pathToInstall, file)));
                                    //OnLog("Downloading...", "Downloading " + file + "...");
                                    this.DownloadFile(
                                        SettingsManager.BUILDS_DOWNLOAD_URL + build + "/" + file,
                                        Path.Combine(pathToInstall, file)
                                    );
                                }
                                OnFileProcessed(file);
                                OnIncreaseDetailProgressBar();
                            }

                            /*if (replacePatcher)
                            {
                                OnLog("Installing patcher!", "Patcher is being installed!");
                                OnIncreaseDetailProgressBar();
                                InstallPatcher(pathToInstall);
                                OnLog("Installed!", "Patcher has been installed!");
                                OnIncreaseDetailProgressBar();
                            }

                            if (replaceShortcut)
                            {
                                InstallShortcut(pathToInstall);
                                OnLog("Shortcut created!", "Shortcut created on your desktop!");
                                OnIncreaseDetailProgressBar();
                            }*/

                            //OnLog("Downloading...", "Downloading version file...");
                            File.Delete(Path.Combine(pathToInstall, "version"));
                            this.DownloadFile(
                                SettingsManager.BUILDS_DOWNLOAD_URL + build + "/version",
                                Path.Combine(pathToInstall, "version")
                            );
                            OnLog("Downloaded!", "Version file was downloaded!");
                            OnIncreaseDetailProgressBar();

                            return true;
                        }
                        else
                        {
                            OnLog("Build " + build + " skipped!", "No files in it!");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        OnLog("Build " + build + " skipped!", ex.Message);
                        continue;
                    }
                }

                if(IsToInstall())
                {
                    OnFatalError("Installation failed!", "Installation failed, no versions installed!", new Exception("Installation failed, no versions installed!"));
                    return false;
                }
            }

            return false;
        }

        public InstallationState Install()
        {
            if (IsToInstall())
            {
                OnTaskStarted("Installation started!");
                OnSetMainProgressBar(0, 2);
                string pathToInstall = GetInstallationPath();
                
                FileManager.CreateDirectory(pathToInstall);

                bool status = this.InstallLatestBuild(pathToInstall);
                
                OnIncreaseMainProgressBar();
                OnTaskCompleted("Installation completed!");

                if (status)
                    return InstallationState.SUCCESS;
                else
                    return InstallationState.FAILED;
            }
            else
            {
                OnLog("Done!", "The application is already installed!");
                return InstallationState.NOT_NEEDED;
            }
        }

        public InstallationState InstallPatcher(string pathToInstall)
        {
            try
            {
                OnLog("Checking...", "Checking for remote service!");
                if (!Utility.IsRemoteServiceAvailable(SettingsManager.PATCHER_DOWNLOAD_URL + "patcher.zip"))
                {
                    OnFatalError("Error!", "No remote service is responding! Try again!", new Exception("No remote service is responding! Try again!"));
                    return InstallationState.FAILED;
                }

                this.DownloadFile(
                    Path.Combine(SettingsManager.PATCHER_DOWNLOAD_URL, "patcher.zip"),
                    Path.Combine(pathToInstall, "patcher.zip")
                );
                Compression.Compressor.Decompress(pathToInstall, Path.Combine(pathToInstall, "patcher.zip"), Compression.CompressionType.ZIP, null);
                File.Delete(Path.Combine(pathToInstall, "patcher.zip"));

                return InstallationState.SUCCESS;
            }
            catch(Exception e)
            {
                Debugger.Log(e.Message);
                return InstallationState.FAILED;
            }
        }

        public InstallationState InstallShortcut(string pathToInstall, bool toPatcher = true)
        {
            try
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCHER_NAME + " - Shortcut.lnk"));
                if (toPatcher)
                {
                    Debugger.Log("Target: " + Path.Combine(pathToInstall, SettingsManager.LAUNCHER_NAME) + " - Dest: " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCHER_NAME + " - Shortcut.lnk"));
                    FileManager.CreateShortcut(Path.Combine(pathToInstall, SettingsManager.LAUNCHER_NAME), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCHER_NAME + " - Shortcut.lnk"));
                }
                else
                {
                    Debugger.Log("Target: " + Path.Combine(pathToInstall, SettingsManager.LAUNCH_APP) + " - Dest: " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCH_APP + " - Shortcut.lnk"));
                    FileManager.CreateShortcut(Path.Combine(pathToInstall, SettingsManager.LAUNCH_APP), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCH_APP + " - Shortcut.lnk"));
                }

                return InstallationState.SUCCESS;
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
                return InstallationState.FAILED;
            }
        }

        public void CheckIntegrity()
        {
            if (IsToInstall())
                Install();
            else
                Repair();
        }

        private string GetCurrentVersion()
        {
            try
            {
                string cypherVersion = File.ReadAllText(SettingsManager.VERSION_FILE_LOCAL_PATH).Replace("\n", "").Replace("\r", "");
                string version = Rijndael.Decrypt(cypherVersion, SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD);
                return version;
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
                OnFatalError("Patch error!", "Version checking error! Please check your version file!", e);
                return null;
            }
        }

        public void Repair(bool replaceShortcut = false, bool replacePatcher = false)
        {
            OnLog("Checking...", "Checking for remote service!");
            if (!Utility.IsRemoteServiceAvailable(SettingsManager.BUILDS_DOWNLOAD_URL + "index"))
            {
                OnFatalError("Error!", "No remote service is responding! Try again!", new Exception("No remote service is responding! Try again!"));
                return;
            }

            OnSetMainProgressBar(0, 5);
            string pathToInstall = GetInstallationPath();

            OnLog("Downloading file...", "Downloading index file for repair process!");
            string indexContent = FileManager.DownloadFileToString(SettingsManager.BUILDS_DOWNLOAD_URL + "index");
            OnIncreaseMainProgressBar();

            if (ProcessBuildsIndex(indexContent))
            {
                OnLog("Checking current version...", "");

                string sCurrentVersion = GetCurrentVersion();
                Version currentVersion = null;
                if (sCurrentVersion != null)
                    currentVersion = new Version(sCurrentVersion);

                OnIncreaseMainProgressBar();

                if (currentVersion == null)
                {
                    OnLog("Warning...", "No current version found, starting installation!");
                    Install();
                    return;
                }

                Version selected = null;
                foreach (Version buildEntry in this.availableBuilds)
                {
                    if (buildEntry.Equals(currentVersion))
                    {
                        selected = buildEntry;
                        break;
                    }
                }

                OnIncreaseMainProgressBar();

                if (selected == null)
                {
                    OnLog("Warning...", "No remote builds available for your current version: aborting repair process...");
                    return;
                }

                string build = selected.ToString();
                try
                {
                    string buildIndexContent = FileManager.DownloadFileToString(SettingsManager.BUILDS_DOWNLOAD_URL + "index_" + build + ".bix");
                    List<string> files = ProcessBuildIndex(buildIndexContent);

                    OnIncreaseMainProgressBar();

                    if (files != null)
                    {
                        OnSetDetailProgressBar(0, files.Count + 1 + ((replaceShortcut) ? 1 : 0) /*+ ((replacePatcher) ? 2 : 0)*/);

                        foreach (string entry in files)
                        {
                            string[] splittedFile = entry.Split(SettingsManager.PATCHES_SYMBOL_SEPARATOR);
                            string file = splittedFile[0];
                            string hash = splittedFile[1];

                            if (file.Equals("version"))
                            {
                                OnLog("Skipping...", "Skipping version file...");
                            }
                            else if (FileManager.FileExists(Path.Combine(pathToInstall, file)))
                            {
                                OnLog("Checking...", "Hash checking for " + file + "...");
                                string alreadyExistingFileHash = Hashing.SHA1(Path.Combine(pathToInstall, file));
                                if (!hash.Equals(alreadyExistingFileHash))
                                {
                                    FileManager.DeleteFile(Path.Combine(pathToInstall, file));
                                    //OnLog("Downloading...", "Downloading " + file + "...");
                                    this.DownloadFile(
                                        SettingsManager.BUILDS_DOWNLOAD_URL + build + "/" + file,
                                        Path.Combine(pathToInstall, file)
                                    );
                                }
                            }
                            else
                            {
                                FileManager.CreateDirectory(Path.GetDirectoryName(Path.Combine(pathToInstall, file)));
                                //OnLog("Downloading...", "Downloading " + file + "...");
                                this.DownloadFile(
                                    SettingsManager.BUILDS_DOWNLOAD_URL + build + "/" + file,
                                    Path.Combine(pathToInstall, file)
                                );
                            }
                            OnFileProcessed(file);
                            OnIncreaseDetailProgressBar();
                        }

                        /*if (replacePatcher)
                        {
                            this.DownloadFile(
                                SettingsManager.PATCHER_DOWNLOAD_URL + "/patcher.zip",
                                Path.Combine(pathToInstall, "patcher.zip")
                            );
                            OnLog("Downloaded!", "Patcher has been downloaded!");
                            OnIncreaseDetailProgressBar();

                            OnLog("Decompressing...", "Decompressing patcher...");
                            Compression.Compressor.Decompress(pathToInstall, Path.Combine(pathToInstall, "patcher.zip"), Compression.CompressionType.ZIP, null);
                            File.Delete(Path.Combine(pathToInstall, "patcher.zip"));
                            OnLog("Decompressed!", "Patcher has been decompressed!");
                            OnIncreaseDetailProgressBar();
                        }*/

                        if (replaceShortcut)
                        {
                            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCHER_NAME + " - Shortcut.lnk"));
                            FileManager.CreateShortcut(Path.Combine(pathToInstall, SettingsManager.LAUNCHER_NAME), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), SettingsManager.LAUNCHER_NAME + " - Shortcut.lnk"));
                            OnLog("Shortcut created!", "Shortcut created on your desktop!");
                            OnIncreaseDetailProgressBar();
                        }

                        //OnLog("Downloading...", "Downloading version file...");
                        this.DownloadFile(
                            SettingsManager.BUILDS_DOWNLOAD_URL + build + "/version",
                            Path.Combine(pathToInstall, "version")
                        );
                        OnLog("Downloaded!", "Version file was downloaded!");
                        OnIncreaseDetailProgressBar();

                        OnIncreaseMainProgressBar();

                        return;
                    }
                    else
                    {
                        OnLog("Build " + build + " skipped!", "No files in it!");
                        OnIncreaseMainProgressBar();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    OnLog("Build " + build + " skipped!", ex.Message);
                    return;
                }
            }

            return;
        }

        public string GetInstallationPath()
        {
            if (!SettingsManager.INSTALL_IN_LOCAL_PATH)
            {
                return SettingsManager.PROGRAM_FILES_DIRECTORY_TO_INSTALL_ABS_PATH;
            }
            else
            {
                return SettingsManager.APP_PATH;
            }
        }

        #region Private stuff
        private bool ProcessBuildsIndex(string content)
        {
            if (content != null)
            {
                if (content != String.Empty)
                {
                    string[] entries = content.Split('\n');
                    foreach (string entry in entries)
                    {
                        if (entry != "")
                        {
                            this.availableBuilds.Add(new Version(entry.Replace("\r", "").Replace("\n", "")));
                        }
                    }
                    this.availableBuilds.Sort(
                        delegate (Version v1, Version v2)
                        {
                            return v2.CompareTo(v1);
                        }
                    );
                    return true;
                }
                else
                {
                    OnLog("No builds available!", "There are no remote builds to install!");
                    return false;
                }
            }
            else
            {
                OnLog("Can't find index file!", "Can't proceed with installation cause of missing build index file!");

                return false;
            }
        }

        private List<string> ProcessBuildIndex(string content)
        {
            List<string> list = new List<string>();
            if (content != null)
            {
                if (content != String.Empty)
                {
                    string[] entries = content.Split('\n');
                    foreach (string entry in entries)
                    {
                        if (entry != "")
                        {
                            list.Add(entry.Replace("\r", ""));
                        }
                    }
                    return list;
                }
                else
                {
                    OnLog("No files available!", "There are no remote builds files to install!");
                    return null;
                }
            }
            else
            {
                OnLog("Can't find specific index file!", "Can't proceed with installation cause of missing build specific index file!");

                return null;
            }
        }

        FileDownloader downloader = null;

        private bool DownloadFile(string remote, string local)
        {
            if (downloader == null)
            {
                downloader = new FileDownloader();
                downloader.ProgressChanged += downloader_ProgressChanged;
                downloader.DownloadComplete += downloader_Completed;
            }
            //OnLog("Checking directories...", "Checking directories and workspace creation!");
            //Directory.CreateDirectory(local);

            OnLog("Downloading file...", "Downloading file " + Path.GetFileName(local) + " from remote server!");
            OnSetDetailProgressBar(0, 100);
            downloader.Download(remote, Path.GetDirectoryName(local));
            OnLog("Downloaded file!", "Downloaded file " + Path.GetFileName(local) + " from remote server!");
            
            return true;
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
