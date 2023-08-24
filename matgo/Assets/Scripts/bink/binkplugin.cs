#if (UNITY_EDITOR_WIN)
#define BINK_WIN
#elif (UNITY_EDITOR_OSX)
#define BINK_MAC
#elif (UNITY_EDITOR_LINUX)
#define BINK_LNX
#elif (UNITY_STANDALONE_WIN)
#define BINK_WIN
#elif (UNITY_STANDALONE_OSX)
#define BINK_MAC
#elif (UNITY_STADIA)
#define BINK_STADIA
#elif (UNITY_STANDALONE_LINUX)
#define BINK_LNX
#elif (UNITY_ANDROID)
#define BINK_ANDROID
#elif (UNITY_IOS)
#define BINK_IOS
#elif (UNITY_TVOS)
#define BINK_TVOS
#elif (UNITY_PS4)
#define BINK_PS4
#elif (UNITY_WSA_10_0)
#define BINK_WIN_UNIVERSAL
#elif (UNITY_XBOXONE)
#define BINK_XB1
#elif (UNITY_SWITCH)
#define BINK_NX
#elif (UNITY_WEBGL)
#define BINK_WEBGL
#else
  // if not detected, assume C# on Windows
#define BINK_WIN
#endif

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
#if (UNITY_5_3_OR_NEWER)
using UnityEngine;
#endif

namespace BinkPlugin
{
    public static class Bink
    {
#if (BINK_WIN)
#if (UNITY_64) || (UNITY_EDITOR_64)
        const string PLUGINNAME = "BinkPluginW64";
#else
        const string PLUGINNAME = "BinkPluginW32";
#endif
#elif (BINK_MAC)
      const string PLUGINNAME = "libBinkPluginMac";
#elif (BINK_STADIA)
      const string PLUGINNAME = "BinkPluginStadia";
#elif (BINK_LNX)
#if (UNITY_64) || (UNITY_EDITOR_64) || (PLATFORM_ARCH_64)
        const string PLUGINNAME = "BinkPluginLnx64";
#else
        const string PLUGINNAME = "BinkPluginLnx32";
#endif
#elif (BINK_WIN_UNIVERSAL)
#if (UNITY_64) || (UNITY_EDITOR_64)
        const string PLUGINNAME = "BinkPluginWinRT_x64.uni10.dll";
#else
        const string PLUGINNAME = "BinkPluginWinRT_x86.uni10.dll";
#endif
#elif (BINK_PS4)
      const string PLUGINNAME = "BinkPluginPS4";
#elif (BINK_XB1)
      const string PLUGINNAME = "BinkPluginXboxOne";
#elif (BINK_ANDROID)
      const string PLUGINNAME = "binkpluginandroid";
#elif (BINK_IOS) || (BINK_NX) || (BINK_TVOS) || (BINK_WEBGL)
      const string PLUGINNAME = "__Internal";
#endif

        // ==========================================================================

        // for windows only, use DirectSound instead of Xaudio, call before
        //   the first open call
#if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR_WIN)
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginWindowsUseDirectSound")]
        public static extern void UseDirectSound();

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginWindowsUseXAudioDevice")]
        public static extern void UseXAudioDevice([MarshalAs(UnmanagedType.LPStr)] string strstr_device_name);
#endif

        // turn on and off IO for all Binks - if the buffer runs *completely* out, we still hit thea disc
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginIOPause")]
        public static extern void IOPause(int IO_on);  // 1 = on, 0 = off

        // Limit the speakers to output to.  By default we limit to stereo (most compatible).
        // set to 1=mono max, 2=stereo max, 3=2.1 max, 4=4.0 max, 6=5.1 max, 8=7.1 max
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginLimitSpeakers")]
        public static extern void LimitSpeakers(int speaker_count);


        // Tell Bink to try to use GPU-assisted mode (once on, always on)
        // This turns on GPU-assisted decoding - it is much faster, but GPU drivers
        // are still really buggy with compute shaders. Only turn this on if you 
        // have a way to let the user force it back off, or if you control the 
        // hardware you are running on (in an embedded case, for example).
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginTurnOnGPUAssist")]
        public static extern void TurnOnGPUAssist();


        // ==========================================================================
        //  Per-Bink functions

        public enum SoundTrackTypes
        {
            SndNone = 0, // don't open any sound tracks snd_track_start not used
            SndSimple = 1, // based on filename, OR simply mono or stereo sound in track snd_track_start (default speaker spread)
            SndLanguageOverride = 2, // mono or stereo sound in track 0, language track at snd_track_start
            Snd51 = 3, // 6 mono tracks in tracks snd_track_start[0..5]
            Snd51LanguageOverride = 4, // 6 mono tracks in tracks 0..5, center language track at snd_track_start
            Snd71 = 5, // 8 mono tracks in tracks snd_track_start[0..7]
            Snd71LanguageOverride = 6, // 8 mono tracks in tracks 0..7, center language track at snd_track_start
        };

        // used to specify the how the video should be buffered
        public enum BufferingTypes
        {
            Stream = 0, // stream the movie off the media during playback (caches about 1 second of video)
            PreloadAll = 1, // loads the whole movie into memory at Open time (will block)
            StreamUntilResident = 2, // streams the movie into a memory buffer as big as the movie, so it will be preloaded eventually)
        };

        // open a Bink file
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginOpen")]
        public static extern IntPtr Open([MarshalAs(UnmanagedType.LPStr)] string name, SoundTrackTypes snd_type, int snd_track_start, BufferingTypes buffering_type, ulong file_byte_offset);

        // for finding bink files within an android jar/zip
        [DllImport(PLUGINNAME, EntryPoint = "BinkGetFileOffset")]
        public static extern ulong GetFileOffset([MarshalAs(UnmanagedType.LPStr)] string datapath, [MarshalAs(UnmanagedType.LPStr)] string binkname);

        // close
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginClose")]
        public static extern void Close(IntPtr bink);

        // set Bink library binaries path
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginSetPath")]
        public static extern IntPtr SetPath([MarshalAs(UnmanagedType.LPStr)] string path);

        // lowlevel way to get any errors on the bink
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginError")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern IntPtr GetErrorIP();

        // get any errors on the bink
        public static string GetError()
        {
            IntPtr s = GetErrorIP();
            return Marshal.PtrToStringAnsi(s);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Info
        {
            public ulong BufferSize;
            public ulong BufferUsed;
            public uint Width;
            public uint Height;
            public uint Frames;
            public uint FrameNum;
            public uint TotalFrames;
            public uint FrameRate;
            public uint FrameRateDiv;
            public uint LoopsRemaining;
            public int ReadError;
            public int TextureError;
            public SoundTrackTypes SndTrackType;
            public uint NumTracksRequested;
            public uint NumTracksOpened;
            public uint SoundDropOuts;
            public uint SkippedFrames;
            public uint PlaybackState;         // 0 = playing, 1 = paused, 2 = gotoing, 3 = at end (stopped)
            public float ProcessBinksFrameRate;
        };

        // get playback info
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginInfo")]
        public static extern void GetInfo(IntPtr bink, ref Info sum);

        // call one of these functions every frame to tell Bink where to draw the bink
        //   lower depths are draw first (so larger depths are on top)
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginScheduleToTexture")]
        public static extern int ScheduleToTexture(IntPtr bink, float x0, float y0, float x1, float y1, int depth, IntPtr render_target_texture, uint render_target_width, uint render_target_height);

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginScheduleOverlay")]
        public static extern int ScheduleOverlay(IntPtr bink, float x0, float y0, float x1, float y1, int depth); // depth is to order videos (>depth on top)

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginAllScheduled")]
        public static extern int AllScheduled(); // call when all schedules for this frame are down


        // pause playback
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginPause")]
        public static extern void Pause(IntPtr bink, int pause_frame); // frame to pause on, -1=pause on current frame, 0 = resume

        // goto a new frame - ms_per_process is how long inside ProcessBinks we are allowed
        //   to use for this goto
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginGoto")]
        public static extern void Goto(IntPtr bink, int goto_frame, int ms_per_process);

        // set overall volume
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginVolume")]
        public static extern void Volume(IntPtr bink, float volume); // 0.0 to 1.0

        // set speaker volumes
        //   BinkSndSimple = count must be 2 (l/r) - OR match filename
        //   BinkSndLanguageOverride = count must be 3 (l,r)/language
        //   BinkSnd51 = count must be 6 (front l/r),center,sub,(rear l/r),
        //   BinkSnd51LanguageOverride = 7 (front l/r),center,sub,(rear l/r),language
        //   BinkSnd71 = count must be 8 (front l/r),center,sub,(read l/r),(surr l/r)
        //   BinkSnd71LanguageOverride = 9 (front l/r),center,sub,(read l/r),(surr l/r), lang
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginSpeakerVolumes")]
        public static extern void SpeakerVolumes(IntPtr bink, float[] volumes, uint count); // 0.0 to 1.0

        // turn on and off looping, loops = 0, infinite
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginLoop")]
        public static extern void Loop(IntPtr bink, uint loops);

        // sets HDR settings state used by BinkPluginScheduleToTexture and BinkPluginScheduleOverlay.
        //   tonemap = 0 (disabled), 1 (enabled)
        //   exposure is a scaling factor that happens before tonemapping (1.0=normal, <1.0 darken, >1.0 brighten)
        //   output_nits = scales the tonemapped output to output this value as its maximum. 
        //     For HDR displays, set this to the max nits of the display. Typically 1000 nits to 2000 nits.
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginSetHdrSettings")]
        public static extern void SetHdrSettings(IntPtr bink, uint do_tonemap, float exposure, int output_nits);

        // sets Alpha settings state used by BinkPluginScheduleToTexture and BinkPluginScheduleOverlay.
        //   alpha_value is just a constant blend value for entire video frame. 1 (default) opaque, 0 fully transparent.
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginSetAlphaSettings")]
        public static extern void SetAlphaSettings(IntPtr bink, float alpha_value);

        // ==========================================================================
        //  startup and shutdown functions, and functions that hit the GPU
        //    on unity, all of these functions are called automatically

        public enum GraphicsAPI
        {
            GL = 0,
            D3D9 = 1,
            D3D11 = 2,
            D3D12 = 3,
            Metal = 4,
            PS4 = 5,
        };

        // turn the plug in system on and off (these functions touch the graphics API)
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginInit")]
        public static extern int Init(IntPtr Graphics_Device, GraphicsAPI Graphics_API);

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginShutdown")]
        public static extern void Shutdown();

        // for D3D9 windows only (call before and after device reset to reset GPU video textures)
#if (BINK_WIN)
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginWindowsD3D9BeginReset")]
        public static extern void D3D9BeginReset();

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginWindowsD3D9EndReset")]
        public static extern void D3D9EndReset();
#endif

        // spins through all binks, advancing frames and using gpu 
        //   this function will hit the gpu on D3D9 and GPUAssisted (not plain GL and D3D11)
        //   ms_to_process is the length of time to wait for a background decompress
        //   to finish (usually pass 1ms here). when doing gotos, this function 
        //   can take longer!
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginProcessBinks")]
        public static extern void ProcessBinks(int ms_to_process);


        // spins through all binks, either overlays or to render targets
        //   generally call, just before flip and before drawing subtitles
        //   this function will hit the gpu
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginDraw")]
        public static extern void Draw(int draw_overlays, int draw_to_render_textures);

        // only used with Unity
        [DllImport(PLUGINNAME, EntryPoint = "BinkGetUnityEventFunc")]
        public static extern IntPtr GetUnityEventFunc();

        // only used with Unity
        [DllImport(PLUGINNAME, EntryPoint = "BinkSetOverlayRenderBuffer")]
        public static extern void SetOverlayRenderBuffer(IntPtr colorBuffer, int width, int height);

        // NX/iOS only
        [DllImport(PLUGINNAME, EntryPoint = "RegisterPlugin")]
        public static extern void RegisterPlugin();

        public enum BinkImageFormat
        {
            BinkSurface32BGRA = 5,
            BinkSurface32RGBA = 6,
            BinkSurface565 = 10,
            BinkSurface32ARGB = 12,
        };

        // open a Bink file as an image
        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginOpenImage")]
        public static extern IntPtr OpenImage([MarshalAs(UnmanagedType.LPStr)] string name, ref uint width, ref uint height, ulong file_byte_offset);

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginReadImage")]
        public static extern void ReadImage(IntPtr binkimage, IntPtr dest, int destpitch, BinkImageFormat format);

        [DllImport(PLUGINNAME, EntryPoint = "BinkPluginCloseImage")]
        public static extern void CloseImage(IntPtr binkimage);

//#if (UNITY_5_3_OR_NEWER)
//        public static unsafe byte[] LoadRawImage(string name, ref uint width, ref uint height, bool mips, bool vertical_flip)
//        {
//            String path;
//            ulong fileOffset = 0;
//            if (Application.platform == RuntimePlatform.Android)
//            {
//                // jar:file:///data/app/com.rad-1/base.apk!/assets/alphatst.bik 
//                path = Application.dataPath;
//                fileOffset = Bink.GetFileOffset(path, name);
//            }
//            else
//            {
//                path = System.IO.Path.Combine(Application.streamingAssetsPath, name);
//            }

//            IntPtr h = Bink.OpenImage(path, ref width, ref height, fileOffset);
//            if (h == IntPtr.Zero)
//            {
//                string s = Bink.GetError();
//                UnityEngine.Debug.Log(s);
//                return null;
//            }
//            uint size = width * height * 4;
//            if (mips) size = (uint)Mathf.CeilToInt((size * 4.0f) / 3.0f);
//            byte[] out_data = new byte[size];

//            fixed (byte* out_data_ptr = out_data)
//            {
//                Bink.ReadImage(h, (IntPtr)out_data_ptr, (int)(width) * (vertical_flip ? -4 : 4), Bink.BinkImageFormat.BinkSurface32RGBA);
//            }
//            Bink.CloseImage(h);
//            return out_data;
//        }

//        public static unsafe Texture2D CreateImage(string name, bool mips, bool vertical_flip)
//        {
//            uint width = 0, height = 0;
//            byte[] out_data = LoadRawImage(name, ref width, ref height, mips, vertical_flip);

//            if (width == 0 || height == 0)
//            {
//                return null;
//            }

//            Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, mips);
//            tex.LoadRawTextureData(out_data);
//            tex.Apply(mips, false);
//            return tex;
//        }

//        public static unsafe void LoadImage(this Texture2D tex, string name, bool vertical_flip)
//        {
//            bool mips = tex.mipmapCount != 1;
//            uint width = 0, height = 0;
//            byte[] out_data = LoadRawImage(name, ref width, ref height, mips, vertical_flip);

//            if (width == 0 || height == 0)
//            {
//                return;
//            }

//            tex.Resize((int)width, (int)height, TextureFormat.RGBA32, mips);
//            tex.LoadRawTextureData(out_data);
//            tex.Apply(mips, false);
//        }
//#endif
    }
}


