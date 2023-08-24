using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MHLab.PATCH.Utilities;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Debugging;
using MHLab.PATCH.Compression;
using Octodiff.Core;

namespace MHLab.PATCH
{
    internal class PatchBuilder
    {
        #region Members
        List<string> _currentBuildFiles;
        List<string> _buildsVersions;
        List<string> _builtPatches;
        Dictionary<string, string> _newFilesHashes;
        
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
        #endregion

        #region Constructor
        public PatchBuilder(bool createDirectories = true)
        {
            try
            {
                _buildsVersions = new List<string>();
                _currentBuildFiles = new List<string>();
                _builtPatches = new List<string>();

                // Initialize directories
                if (createDirectories)
                {
                    if (!FileManager.DirectoryExists(SettingsManager.CURRENT_BUILD_PATH))
                        FileManager.CreateDirectory(SettingsManager.CURRENT_BUILD_PATH);
                    if (!FileManager.DirectoryExists(SettingsManager.BUILDS_PATH))
                        FileManager.CreateDirectory(SettingsManager.BUILDS_PATH);
                    if (!FileManager.DirectoryExists(SettingsManager.FINAL_PATCHES_PATH))
                        FileManager.CreateDirectory(SettingsManager.FINAL_PATCHES_PATH);
                    if (!FileManager.DirectoryExists(SettingsManager.DEPLOY_PATH))
                        FileManager.CreateDirectory(SettingsManager.DEPLOY_PATH);
                    if (!FileManager.DirectoryExists(SettingsManager.PATCHER_FILES_PATH))
                        FileManager.CreateDirectory(SettingsManager.PATCHER_FILES_PATH);
                }

                // Initialize needed files
                if(!FileManager.FileExists(FileManager.PathCombine(SettingsManager.FINAL_PATCHES_PATH, "versions.txt")))
                {
                    FileManager.CreateFile(FileManager.PathCombine(SettingsManager.FINAL_PATCHES_PATH, "versions.txt"));
                }

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
            }
            catch (Exception e)
            {
                OnFatalError.Invoke("Fatal error", "Something goes terribly wrong during PatchBuilder init process!", e);
                Debugger.Log(e.Message);
            }

        }
        #endregion

        #region Public interface
        /// <summary>
        /// Returns a list of files contained in CURRENT_BUILD_PATH
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentBuildFiles()
        {
            this._currentBuildFiles = FileManager.GetCurrentBuildFiles().ToList();
            return this._currentBuildFiles;
        }

        /// <summary>
        /// Returns a list of versions already processed
        /// </summary>
        /// <returns></returns>
        public List<string> GetVersions()
        {
            List<string> tmpList = FileManager.GetAllBuildsDirectories().ToList().Distinct().ToList();
            this._buildsVersions = new List<string>();
            List<Version> tmpVersions = new List<Version>();
            foreach (string version in tmpList)
            {
                string[] tmp = version.Split(Path.DirectorySeparatorChar);
                //this._buildsVersions.Add(tmp[tmp.Count() - 1]);
                tmpVersions.Add(new Version(tmp[tmp.Count() - 1]));
            }
            tmpVersions.Sort((version1, version2) => { return version1.CompareTo(version2); });
            this._buildsVersions.AddRange(tmpVersions.Select((x) => { return x.ToString(); }));
            return this._buildsVersions;
        }

        /// <summary>
        /// Returns an array of versions already processed
        /// </summary>
        /// <returns></returns>
        public string[] GetCurrentVersions()
        {
            if (this._buildsVersions == null)
                GetVersions();
            return this._buildsVersions.ToArray();
        }

        public string[] GetCurrentPatches()
        {
            List<string> tmpList = FileManager.GetFiles(SettingsManager.FINAL_PATCHES_PATH, "*.pix").ToList();
            this._builtPatches = new List<string>();
            foreach (string patch in tmpList)
            {
                string[] tmp = patch.Split(Path.DirectorySeparatorChar);
                this._builtPatches.Add(tmp[tmp.Count() - 1].Replace(".pix", ""));
            }
            return this._builtPatches.ToArray();
        }

        /// <summary>
        /// Returns the last version built
        /// </summary>
        /// <returns></returns>
        public string GetLastVersion()
        {
            GetVersions();
            if (this._buildsVersions.Count > 0)
                return this._buildsVersions.Last();
            else
                return "none";
        }

        /// <summary>
        /// This function build a new version based on files contained in CURRENT_BUILD_PATH
        /// </summary>
        /// <param name="v"></param>
        public void BuildNewVersion(string v)
        {
            try
            {
                OnTaskStarted("New version creation started...");
                OnSetMainProgressBar(0, 7);
                Version version = new Version(v);

                // Check if choosen version is already built
                OnSetDetailProgressBar(0, 1);
                if (FileManager.DirectoryExists(SettingsManager.BUILDS_PATH + version))
                {
                    OnError.Invoke("Error!", "This version (" + version + ") already exists!", null);
                    throw new Exception("This version (" + version + ") already exists!");
                }
                OnIncreaseDetailProgressBar();
                OnIncreaseMainProgressBar();

                // Check if current build directory contains something
                if (FileManager.IsDirectoryEmpty(SettingsManager.CURRENT_BUILD_PATH))
                {
                    OnError.Invoke("Error!", "There are no current builds!", null);
                    throw new Exception("There are no current builds!");
                }
                OnIncreaseDetailProgressBar();
                OnIncreaseMainProgressBar();

                try
                {
                    // Create new directory for new version
                    OnSetDetailProgressBar(0, 1);
                    FileManager.CreateDirectory(SettingsManager.BUILDS_PATH + version);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();
                    // Copy current build in new directory created
                    OnSetDetailProgressBar(0, 1);
                    FileManager.CopyDirectory(SettingsManager.CURRENT_BUILD_PATH, SettingsManager.BUILDS_PATH + version + Path.DirectorySeparatorChar);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();

                    // Build version file for current build
                    OnSetDetailProgressBar(0, 1);
                    BuildVersionFile(version);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();

                    // Update build indexes
                    OnSetDetailProgressBar(0, 1);
                    UpdateBuildsIndex(version);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();

                    // Clean current build directory
                    OnSetDetailProgressBar(0, 1);
                    FileManager.CleanDirectory(SettingsManager.CURRENT_BUILD_PATH);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();

                    OnLog.Invoke("Build completed!", "A new build version has been processed!");
                    OnTaskCompleted("Build completed! Check your \"builds\" directory to find your " + version + " build!");
                }
                catch (Exception ex)
                {
                    OnError.Invoke("Error!", "Can't complete new version building process!", ex);
                    throw ex;
                }
            }
            catch(Exception ex)
            {
                OnError.Invoke("Error!", "Something goes wrong during new version building process init!", ex);
                throw ex;
            }
        }

        /// <summary>
        /// This function build a patch between two version already built
        /// </summary>
        /// <param name="oldVersion"></param>
        /// <param name="newVersion"></param>
        public void BuildPatch(string sVersionFrom, string sVersionTo, CompressionType type)
        {
            OnTaskStarted("Patch building started...");
            // Check if versions are the same.
            if (sVersionFrom == sVersionTo)
            {
                OnError.Invoke("Error!", "Can't build a patch! Versions are the same!", null);
                throw new Exception("Can't build a patch! Versions are the same!");
            }

            Version versionFrom = new Version(sVersionFrom);
            Version versionTo = new Version(sVersionTo);

            this._newFilesHashes = new Dictionary<string, string>();

            // Get files from versionFrom.
            List<string> oldVersionFiles = FileManager.GetFiles(SettingsManager.BUILDS_PATH + versionFrom).ToList().Select(f => f = f.Replace((SettingsManager.BUILDS_PATH + versionFrom + Path.DirectorySeparatorChar), "")).ToList();
            // Get files from versionTo.
            List<string> newVersionFiles = FileManager.GetFiles(SettingsManager.BUILDS_PATH + versionTo).ToList().Select(f => f = f.Replace((SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar), "")).ToList();

            // Create a new patch object based on two build versions.
            Patch patch = new Patch(versionFrom, versionTo);

            // Set main progress bar
            int mainProgressBarMaximum = oldVersionFiles.Count + newVersionFiles.Count + 5;
            OnSetMainProgressBar(0, mainProgressBarMaximum);

            // Let's iterate on all versionTo files to build patch and add new files.
            foreach (string file in newVersionFiles)
            {
                try
                {
                    if (IsFileOSRelated(file)) continue;
                    OnFileProcessing(SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar + file);
                    OnSetDetailProgressBar(0, 1);

                    // Create directory for patched file, if it doesn't exists.
                    FileManager.CreateDirectory(Path.GetDirectoryName(SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar + file));

                    if (oldVersionFiles.Contains(file))
                    {
                        // Check files hashes. If the same, we don't need to patch anything
                        string newFileHash = Hashing.SHA1(SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar + file);
                        string oldFileHash = Hashing.SHA1(SettingsManager.BUILDS_PATH + versionFrom + Path.DirectorySeparatorChar + oldVersionFiles.Single(f => f == file));

                        if (newFileHash != oldFileHash)
                        {
                            // Get patch file name.
                            string patchName = SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar + file + SettingsManager.PATCH_EXTENSION;

                            // Build patch between old file and new file.
                            BuildOctodiffPatch(
                                (SettingsManager.BUILDS_PATH + versionFrom + Path.DirectorySeparatorChar + oldVersionFiles.Single(f => f == file)),
                                (SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar + file),
                                patchName
                            );

                            // Add hash of versionTo file to files hash list.
                            this._newFilesHashes.Add(file, newFileHash);
                        }
                    }
                    else
                    {
                        // File isn't contained in versionFrom files, so it is a new file.
                        // We need to add it to our patch.
                        FileManager.FileCopy(
                            SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar + file,
                            SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar + file
                        );
                    }
                    
                    OnFileProcessed(SettingsManager.BUILDS_PATH + versionTo + Path.DirectorySeparatorChar + file);
                    OnIncreaseDetailProgressBar();
                    OnIncreaseMainProgressBar();
                }
                catch (Exception ex)
                {
                    OnError("Error!", "An error occurred during patch building process!", ex);
                    throw ex;
                }
            }

            // Let's iterate on all versionFrom file to see if some files need to be deleted when new version will be applied.
            foreach (string file in oldVersionFiles)
            {
                if (IsFileOSRelated(file)) continue;
                OnFileProcessing(SettingsManager.BUILDS_PATH + versionFrom + Path.DirectorySeparatorChar + file);
                OnSetDetailProgressBar(0, 1);

                if (!newVersionFiles.Contains(file))
                {
                    // If an old version file isn't contained in new version file list
                    // we need to delete it when new version will be applied, so we will
                    // add a reminder in our patch.
                    FileManager.CreateDirectory(Path.GetDirectoryName(SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar + file));
                    FileManager.CreateFile(SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar + file + SettingsManager.PATCH_DELETE_FILE_EXTENSION);
                }

                OnFileProcessed(SettingsManager.BUILDS_PATH + versionFrom + Path.DirectorySeparatorChar + file);
                OnIncreaseDetailProgressBar();
                OnIncreaseMainProgressBar();
            }

            OnLog("Cleaning up patch workspace...", "Deleting signature files...");
            OnSetDetailProgressBar(0, 1);
            FileManager.DeleteFiles(SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar, "*.signature");
            OnIncreaseDetailProgressBar();
            OnIncreaseMainProgressBar();

            OnLog("Patch compression...", "Creating a compressed archive for " + patch.PatchName + " patch...");
            OnSetDetailProgressBar(0, 1);
            Compressor.Compress(
                SettingsManager.PATCHES_PATH + patch.PatchName + Path.DirectorySeparatorChar,
                SettingsManager.FINAL_PATCHES_PATH + patch.ArchiveName,
                type,
                null
            );
            OnIncreaseDetailProgressBar();
            OnIncreaseMainProgressBar();

            OnLog("Cleaning up patch workspace...", "Deleting useless files...");
            OnSetDetailProgressBar(0, 1);
            FileManager.DeleteDirectory(SettingsManager.PATCHES_PATH);
            OnIncreaseDetailProgressBar();
            OnIncreaseMainProgressBar();

            OnSetDetailProgressBar(0, 2);
            BuildPatchVersionsFile(patch, versionFrom, versionTo, type);
            OnIncreaseDetailProgressBar();
            OnIncreaseMainProgressBar();


            OnLog("Creating index...", "Creating patch index for clients files validation...");
            GeneratePatchIndex(patch);
            OnIncreaseDetailProgressBar();
            OnIncreaseMainProgressBar();

            OnLog("Patch completed!", "");

            OnTaskCompleted("Patch completed! Check your \"patches\" directory to find your " + versionFrom + "_" + versionTo + " patch!");
        }
        #endregion

        #region Methods
        private void BuildVersionFile(Version v)
        {
            using (FileStream file = new FileStream(SettingsManager.BUILDS_PATH + v + Path.DirectorySeparatorChar + "version", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(Rijndael.Encrypt(v.ToString(), SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD));
                }
            }
        }

        private void UpdateBuildsIndex(Version v)
        {
            string indexFile = SettingsManager.BUILDS_PATH + "index_" + v + ".bix";
            string generalIndexFile = SettingsManager.BUILDS_PATH + "index";

            FileManager.CreateFile(indexFile);
            FileManager.CreateFile(generalIndexFile);

            // Write new version created on versions index file
            using (FileStream file = new FileStream(generalIndexFile, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(v.ToString());
                }
            }

            // Write on new build index file all files contained in it
            IEnumerable<string> files = FileManager.GetFiles(SettingsManager.BUILDS_PATH + v, "*", SearchOption.AllDirectories);
            using (FileStream file = new FileStream(indexFile, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    foreach (string filename in files)
                    {
                        // Get relative file path
                        string relativeFilePath = filename.Replace(SettingsManager.BUILDS_PATH + v + Path.DirectorySeparatorChar, "");

                        OnLog.Invoke("Generate builds index", "Processing b-index for " + relativeFilePath + "...");

                        // Write current file and its hash in build index file
                        writer.WriteLine(relativeFilePath + SettingsManager.PATCHES_SYMBOL_SEPARATOR + Hashing.SHA1(filename));
                    }
                    OnLog.Invoke("Builds index completed", "Builds index is processed successfully!");
                }
            }
        }

        private void BuildOctodiffPatch(string fromFileName, string toFileName, string patchName)
        {
            try
            {
                SignatureBuilder signatureBuilder = new SignatureBuilder();
                DeltaBuilder deltaBuilder = new DeltaBuilder();

                FileStream oldFile = new FileStream(fromFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream newFile = new FileStream(toFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                FileStream patch = new FileStream(patchName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                FileStream sign = new FileStream(patchName + ".signature", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

                signatureBuilder.Build(oldFile, new SignatureWriter(sign));
                sign.Close();
                sign.Dispose();
                deltaBuilder.BuildDelta(newFile, new SignatureReader(sign.Name, deltaBuilder.ProgressReporter), new AggregateCopyOperationsDecorator(new BinaryDeltaWriter(patch)));

                oldFile.Close();
                oldFile.Dispose();
                newFile.Close();
                newFile.Dispose();
                patch.Close();
                patch.Dispose();
            }
            catch (Exception ex)
            {
                OnError("Error!", "An error occurred while building patch between " + fromFileName + " and " + toFileName, ex);
                throw ex;
            }
        }

        private void BuildPatchVersionsFile(Patch patch, Version versionFrom, Version versionTo, CompressionType type)
        {
            using (FileStream zipPatchStream = new FileStream(SettingsManager.FINAL_PATCHES_PATH + patch.ArchiveName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                OnLog("Writing version...", "Writing new version on version.txt file...");

                using (FileStream file = new FileStream(SettingsManager.FINAL_PATCHES_PATH + "versions.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        string archiveHash = Hashing.SHA1(zipPatchStream);
                        writer.WriteLine(versionFrom.ToString() + SettingsManager.PATCHES_SYMBOL_SEPARATOR + versionTo.ToString() + SettingsManager.PATCHES_SYMBOL_SEPARATOR + archiveHash + SettingsManager.PATCHES_SYMBOL_SEPARATOR + type.ToString());
                    }
                }
            }
        }

        private void GeneratePatchIndex(Patch patch)
        {
            StringBuilder fileText = new StringBuilder();
            foreach (KeyValuePair<string, string> entry in this._newFilesHashes)
            {
                fileText.AppendLine(entry.Key.Replace('\\', '/') + SettingsManager.PATCHES_SYMBOL_SEPARATOR + entry.Value);
            }
            string indexFile = SettingsManager.FINAL_PATCHES_PATH + patch.PatchName + ".pix";
            using (FileStream file = File.Create(indexFile))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Write(Rijndael.Encrypt(fileText.ToString(), SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD));
                }
            }
        }

        private bool IsFileOSRelated(string file)
        {
            if (
                file.Contains(".DS_Store") ||
                file.Contains("desktop.ini")
            )
                return true;
            else return false;
        }
        #endregion


















        /*

        
        #endregion

        #region Private
        

        

        public void GenerateBuildsIndexes()
        {
            IEnumerable<string> buildsNames = FileManager.GetAllBuildsDirectories();
            LogEventArgs e;

            if (buildsNames.Count() < 1)
            {
                e = new LogEventArgs("Generation of builds indexes interrupted!", "There are no builds to generate a build index!");
                OnLogEvent(e);
                return;
            }

            foreach (string buildName in buildsNames)
            {
                string buildFixedName = buildName.Replace(SettingsManager.BUILDS_PATH, "");
                string indexFile = SettingsManager.BUILDS_PATH + "index_" + buildFixedName + ".bix";
                string generalIndexFile = SettingsManager.BUILDS_PATH + "index";

                FileManager.DeleteFile(generalIndexFile);
                using (FileStream file = new FileStream(generalIndexFile, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.WriteLine(buildFixedName);
                    }
                }

                IEnumerable<string> builds = FileManager.GetFiles(buildName, "*", SearchOption.AllDirectories);

                using (FileStream file = new FileStream(indexFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        foreach (string build in builds)
                        {
                            string filename = build.Replace(SettingsManager.BUILDS_PATH + buildFixedName + Path.DirectorySeparatorChar, "");
                            e = new LogEventArgs("Generate builds index for " + buildFixedName, "Processing b-index for " + filename + "...");
                            OnLogEvent(e);
                            writer.WriteLine(filename + SettingsManager.PATCHES_SYMBOL_SEPARATOR + Utility.HashFile(filename));

                        }
                        e = new LogEventArgs("Builds index completed for " + buildFixedName, "Builds index is processed successfully!");
                        OnLogEvent(e);
                    }
                }
            }
            e = new LogEventArgs("Builds indexes completed!", "Builds indexes are processed successfully!");
            OnLogEvent(e);
        }

        

        */
    }
}
