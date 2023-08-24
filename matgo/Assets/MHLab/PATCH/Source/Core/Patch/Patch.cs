using System;
using MHLab.PATCH.Compression;
using MHLab.PATCH.Debugging;

namespace MHLab.PATCH
{
    internal class Patch
    {
        public Version From;
        public Version To;
        public string Hash;
        public CompressionType Type;

        public string PatchName
        {
            get
            {
                return From + "_" + To;
            }
        }

        public string ArchiveName
        {
            get
            {
                return From + "_" + To + ".archive";
            }
        }

        public string IndexerName
        {
            get
            {
                return From + "_" + To + ".pix";
            }
        }

        public Patch(Version from, Version to)
        {
            From = from;
            To = to;
        }

        public Patch(string entry)
        {
            try
            {
                string[] entries = entry.Split(Settings.SettingsManager.PATCHES_SYMBOL_SEPARATOR);
                From = new Version(entries[0]);
                To = new Version(entries[1]);
                Hash = entries[2];
                Type = (CompressionType)Enum.Parse(typeof(CompressionType), entries[3].Replace("\n", "").Replace("\r", ""));
            }
            catch (Exception e)
            {
                Debugger.Log(e.Message);
            }
        }
    }
}
