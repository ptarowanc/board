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
#elif (UNITY_STANDALONE_LINUX)
#define BINK_LNX
#elif (UNITY_ANDROID)
#define BINK_ANDROID
#elif (UNITY_IOS)
#define BINK_IOS
#elif (UNITY_PS4)
#define BINK_PS4
#elif (UNITY_WSA_10_0)
#define BINK_WIN_UNIVERSAL
#elif (UNITY_XBOXONE)
#define BINK_XB1
#else
// if not detected, assume C# on Windows
#define BINK_WIN
#endif

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

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
#elif (BINK_LNX)
#if (UNITY_64) || (UNITY_EDITOR_64)
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
#if (UNITY_64)
const string PLUGINNAME = "binkpluginandroidarm64";
#else
const string PLUGINNAME = "binkpluginandroid";
#endif
#elif (BINK_IOS)
const string PLUGINNAME = "__Internal";
#endif

// ==========================================================================

// for windows only, use DirectSound instead of Xaudio, call before
//   the first open call
#if (BINK_WIN)
[DllImport(PLUGINNAME, EntryPoint="BinkPluginWindowsUseDirectSound")]
public static extern void UseDirectSound();
#endif

// turn on and off IO for all Binks - if the buffer runs *completely* out, we still hit thea disc
[DllImport(PLUGINNAME, EntryPoint="BinkPluginIOPause")]
public static extern void IOPause( int IO_on );  // 1 = on, 0 = off

// Tell Bink to try to use GPU-assisted mode (once on, always on)
// This turns on GPU-assisted decoding - it is much faster, but GPU drivers
// are still really buggy with compute shaders. Only turn this on if you 
// have a way to let the user force it back off, or if you control the 
// hardware you are running on (in an embedded case, for example).
[DllImport(PLUGINNAME, EntryPoint="BinkPluginTurnOnGPUAssist")]
public static extern void TurnOnGPUAssist();


// ==========================================================================
//  Per-Bink functions

public enum SoundTrackTypes
{
SndNone               = 0, // don't open any sound tracks snd_track_start not used
SndSimple             = 1, // based on filename, OR simply mono or stereo sound in track snd_track_start (default speaker spread)
SndLanguageOverride   = 2, // mono or stereo sound in track 0, language track at snd_track_start
Snd51                 = 3, // 6 mono tracks in tracks snd_track_start[0..5]
Snd51LanguageOverride = 4, // 6 mono tracks in tracks 0..5, center language track at snd_track_start
Snd71                 = 5, // 8 mono tracks in tracks snd_track_start[0..7]
Snd71LanguageOverride = 6, // 8 mono tracks in tracks 0..7, center language track at snd_track_start
};

// used to specify the how the video should be buffered
public enum BufferingTypes
{
Stream                = 0, // stream the movie off the media during playback (caches about 1 second of video)
PreloadAll            = 1, // loads the whole movie into memory at Open time (will block)
StreamUntilResident   = 2, // streams the movie into a memory buffer as big as the movie, so it will be preloaded eventually)
};

// open a Bink file
[DllImport(PLUGINNAME, EntryPoint="BinkPluginOpen")]
public static extern IntPtr Open( [MarshalAs(UnmanagedType.LPStr)] string name, SoundTrackTypes snd_type, int snd_track_start, BufferingTypes buffering_type, ulong file_byte_offset );

// for finding bink files within an android jar/zip
[DllImport(PLUGINNAME, EntryPoint="BinkGetFileOffset")]
public static extern ulong GetFileOffset(  [MarshalAs(UnmanagedType.LPStr)] string datapath,   [MarshalAs(UnmanagedType.LPStr)] string binkname );

// close
[DllImport(PLUGINNAME, EntryPoint="BinkPluginClose")]
public static extern void Close( IntPtr bink );

// lowlevel way to get any errors on the bink
[DllImport(PLUGINNAME, EntryPoint="BinkPluginError")][return: MarshalAs(UnmanagedType.LPStr)]
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
public uint  Width;
public uint  Height;
public uint  Frames;
public uint  FrameNum;
public uint  TotalFrames;
public uint  FrameRate;
public uint  FrameRateDiv;
public uint  LoopsRemaining;
public int   ReadError;
public int   TextureError;
public SoundTrackTypes SndTrackType;
public uint  NumTracksRequested;
public uint  NumTracksOpened;
public uint  SoundDropOuts;
public uint  SkippedFrames;
public uint  PlaybackState;         // 0 = playing, 1 = paused, 2 = gotoing, 3 = at end (stopped)
public float ProcessBinksFrameRate;
};

// get playback info
[DllImport(PLUGINNAME, EntryPoint="BinkPluginInfo")]
public static extern void GetInfo( IntPtr bink, ref Info sum );

// call one of these functions every frame to tell Bink where to draw the bink
//   lower depths are draw first (so larger depths are on top)
[DllImport(PLUGINNAME, EntryPoint="BinkPluginScheduleToTexture")]
public static extern int ScheduleToTexture( IntPtr bink, float x0, float y0, float x1, float y1, int depth, IntPtr render_target_texture, uint render_target_width, uint render_target_height );

[DllImport(PLUGINNAME, EntryPoint="BinkPluginScheduleOverlay")]
public static extern int ScheduleOverlay( IntPtr bink, float x0, float y0, float x1, float y1, int depth ); // depth is to order videos (>depth on top)

[DllImport(PLUGINNAME, EntryPoint="BinkPluginAllScheduled")]
public static extern int AllScheduled( ); // call when all schedules for this frame are down


// pause playback
[DllImport(PLUGINNAME, EntryPoint="BinkPluginPause")]
public static extern void Pause( IntPtr bink, int pause_frame ); // frame to pause on, -1=pause on current frame, 0 = resume

// goto a new frame - ms_per_process is how long inside ProcessBinks we are allowed
//   to use for this goto
[DllImport(PLUGINNAME, EntryPoint="BinkPluginGoto")]
public static extern void Goto( IntPtr bink, int goto_frame, int ms_per_process ); 

// set overall volume
[DllImport(PLUGINNAME, EntryPoint="BinkPluginVolume")]
public static extern void Volume( IntPtr bink, float volume ); // 0.0 to 1.0

// set speaker volumes
//   BinkSndSimple = count must be 2 (l/r) - OR match filename
//   BinkSndLanguageOverride = count must be 3 (l,r)/language
//   BinkSnd51 = count must be 6 (front l/r),center,sub,(rear l/r),
//   BinkSnd51LanguageOverride = 7 (front l/r),center,sub,(rear l/r),language
//   BinkSnd71 = count must be 8 (front l/r),center,sub,(read l/r),(surr l/r)
//   BinkSnd71LanguageOverride = 9 (front l/r),center,sub,(read l/r),(surr l/r), lang
[DllImport(PLUGINNAME, EntryPoint="BinkPluginSpeakerVolumes")]
public static extern void SpeakerVolumes( IntPtr bink, float[] volumes, uint count ); // 0.0 to 1.0

// turn on and off looping, loops = 0, infinite
[DllImport(PLUGINNAME, EntryPoint="BinkPluginLoop")]
public static extern void Loop( IntPtr bink, uint loops );

// ==========================================================================
//  startup and shutdown functions, and functions that hit the GPU
//    on unity, all of these functions are called automatically

public enum GraphicsAPI
{
GL        = 0,
D3D9      = 1,
D3D11     = 2,
D3D12     = 3,
Metal     = 4,
PS4       = 5,
};

// turn the plug in system on and off (these functions touch the graphics API)
[DllImport(PLUGINNAME, EntryPoint="BinkPluginInit")]
public static extern int Init( IntPtr Graphics_Device, GraphicsAPI Graphics_API );

[DllImport(PLUGINNAME, EntryPoint="BinkPluginShutdown")]
public static extern void Shutdown();

// for D3D9 windows only (call before and after device reset to reset GPU video textures)
#if (BINK_WIN)
[DllImport(PLUGINNAME, EntryPoint="BinkPluginWindowsD3D9BeginReset")]
public static extern void D3D9BeginReset();

[DllImport(PLUGINNAME, EntryPoint="BinkPluginWindowsD3D9EndReset")]
public static extern void D3D9EndReset();
#endif

// spins through all binks, advancing frames and using gpu 
//   this function will hit the gpu on D3D9 and GPUAssisted (not plain GL and D3D11)
//   ms_to_process is the length of time to wait for a background decompress
//   to finish (usually pass 1ms here). when doing gotos, this function 
//   can take longer!
[DllImport(PLUGINNAME, EntryPoint="BinkPluginProcessBinks")]
public static extern void ProcessBinks( int ms_to_process );


// spins through all binks, either overlays or to render targets
//   generally call, just before flip and before drawing subtitles
//   this function will hit the gpu
[DllImport(PLUGINNAME, EntryPoint="BinkPluginDraw")]
public static extern void Draw( int draw_overlays, int draw_to_render_textures ); 

// only used with Unity
[DllImport(PLUGINNAME, EntryPoint="BinkGetUnityEventFunc")]
public static extern IntPtr GetUnityEventFunc(); 

// only used with Unity
[DllImport(PLUGINNAME, EntryPoint="BinkSetOverlayRenderBuffer")]
public static extern void SetOverlayRenderBuffer(IntPtr colorBuffer); 
}
}


