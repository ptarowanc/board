using System;
using System.IO;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Utilities;

namespace MHLab.PATCH
{
    public class LauncherManager
    {
        #region Members
        PatchApplier m_patchApplier;
        #endregion

        #region Constructor
        public LauncherManager()
        {
            m_patchApplier = new PatchApplier();
        }
        #endregion

        #region Methods for setting callbacks
        public void SetPatchBuilderOnFileProcessedAction(Action<string> action)
        {
            m_patchApplier.OnFileProcessed = action;
        }

        public void SetOnFileProcessingAction(Action<string> action)
        {
            m_patchApplier.OnFileProcessing = action;
        }

        public void SetOnTaskStartedAction(Action<string> action)
        {
            m_patchApplier.OnTaskStarted = action;
        }

        public void SetOnTaskCompletedAction(Action<string> action)
        {
            m_patchApplier.OnTaskCompleted = action;
        }

        public void SetOnLogAction(Action<string, string> action)
        {
            m_patchApplier.OnLog = action;
        }

        public void SetOnErrorAction(Action<string, string, Exception> action)
        {
            m_patchApplier.OnError = action;
        }

        public void SetOnFatalErrorAction(Action<string, string, Exception> action)
        {
            m_patchApplier.OnFatalError = action;
        }

        public void SetOnSetMainProgressBarAction(Action<int, int> action)
        {
            m_patchApplier.OnSetMainProgressBar = action;
        }

        public void SetOnSetDetailProgressBarAction(Action<int, int> action)
        {
            m_patchApplier.OnSetDetailProgressBar = action;
        }

        public void SetOnIncreaseMainProgressBarAction(Action action)
        {
            m_patchApplier.OnIncreaseMainProgressBar = action;
        }

        public void SetOnIncreaseDetailProgressBarAction(Action action)
        {
            m_patchApplier.OnIncreaseDetailProgressBar = action;
        }

        public void SetOnDownloadProgressAction(Action<long, long, int> action)
        {
            m_patchApplier.OnDownloadProgress = action;
        }

        public void SetOnDownloadCompletedAction(Action action)
        {
            m_patchApplier.OnDownloadCompleted = action;
        }
        #endregion

        #region Public methods
        public void CheckForUpdates()
        {
            m_patchApplier.CheckForUpdates();
        }

        public string GetCurrentVersion()
        {
            return m_patchApplier.GetCurrentVersion();
        }

        public bool IsDirty()
        {
            return m_patchApplier.IsDirtyWorkspace;
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
            }
            catch (Exception ex)
            {
                Debugging.Debugger.Log(ex.Message);
            }
        }
        #endregion
    }
}
