using System;
using System.Collections.Generic;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Utilities;
using System.Xml.Serialization;
using System.IO;

namespace MHLab.PATCH.Install
{
    public class InstallManager
    {
        #region Members
        Installer m_installer;
        #endregion

        #region Constructor
        public InstallManager()
        {
            m_installer = new Installer();
        }
        #endregion

        #region Methods for setting callbacks
        public void SetPatchBuilderOnFileProcessedAction(Action<string> action)
        {
            m_installer.OnFileProcessed = action;
        }

        public void SetOnFileProcessingAction(Action<string> action)
        {
            m_installer.OnFileProcessing = action;
        }

        public void SetOnTaskStartedAction(Action<string> action)
        {
            m_installer.OnTaskStarted = action;
        }

        public void SetOnTaskCompletedAction(Action<string> action)
        {
            m_installer.OnTaskCompleted = action;
        }

        public void SetOnLogAction(Action<string, string> action)
        {
            m_installer.OnLog = action;
        }

        public void SetOnErrorAction(Action<string, string, Exception> action)
        {
            m_installer.OnError = action;
        }

        public void SetOnFatalErrorAction(Action<string, string, Exception> action)
        {
            m_installer.OnFatalError = action;
        }

        public void SetOnSetMainProgressBarAction(Action<int, int> action)
        {
            m_installer.OnSetMainProgressBar = action;
        }

        public void SetOnSetDetailProgressBarAction(Action<int, int> action)
        {
            m_installer.OnSetDetailProgressBar = action;
        }

        public void SetOnIncreaseMainProgressBarAction(Action action)
        {
            m_installer.OnIncreaseMainProgressBar = action;
        }

        public void SetOnIncreaseDetailProgressBarAction(Action action)
        {
            m_installer.OnIncreaseDetailProgressBar = action;
        }

        public void SetOnDownloadProgressAction(Action<long, long, int> action)
        {
            m_installer.OnDownloadProgress = action;
        }

        public void SetOnDownloadCompletedAction(Action action)
        {
            m_installer.OnDownloadCompleted = action;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Checks for installation's integrity, so provides for installation or repair process.
        /// </summary>
        public void CheckIntegrity()
        {
            m_installer.CheckIntegrity();
        }

        public void Repair()
        {
            m_installer.Repair();
        }

        public InstallationState Install()
        {
            return m_installer.Install();
        }

        public InstallationState InstallPatcher()
        {
            return m_installer.InstallPatcher(GetInstallationPath());
        }


        /// <summary>
        /// Create a desktop shortcut to your game
        /// </summary>
        /// <param name="toLauncher">If true, created shortcut will be linked to launcher. If false, created shortcut will be linked to your game.</param>
        public InstallationState CreateShortcut(bool toLauncher = true)
        {
            return m_installer.InstallShortcut(GetInstallationPath(), toLauncher);
        }

        public bool IsToInstall()
        {
            return m_installer.IsToInstall();
        }

        public string GetInstallationPath()
        {
            return m_installer.GetInstallationPath();
        }

        public SettingsOverrider SETTINGS = new SettingsOverrider();
        public void LoadSettings()
        {
            try
            {
                if (FileManager.FileExists(SettingsManager.LAUNCHER_CONFIG_PATH))
                {
                    string file = File.ReadAllText(SettingsManager.LAUNCHER_CONFIG_PATH);
                    file = Rijndael.Decrypt(file, SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD);
                    SETTINGS = SettingsOverrider.XmlDeserializeFromString<SettingsOverrider>(file);

                    SETTINGS.OverrideSettings();
                }
                else
                {
                    throw new FileNotFoundException("Config file cannot be loaded!", SettingsManager.LAUNCHER_CONFIG_PATH);
                }
            }
            catch (Exception ex)
            {
                Debugging.Debugger.Log(ex.Message);
            }
        }
        #endregion
    }
}
