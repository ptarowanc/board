/* vim: set softtabstop=2 shiftwidth=2 expandtab : */
enum BinkCommands
{
  ProcessOnly          = 0x18888880,
  ProcessOnlyNoWait    = 0x18888881,
  ProcessCloses        = 0x18888882,
  ProcessAndDraw       = 0x18888883,
  ProcessAndDrawNoWait = 0x18888884,
  DrawOnly             = 0x18888885,
  DrawToTexturesOnly   = 0x18888886,
  DrawOverlaysOnly     = 0x18888887
};

#if defined(__RADWIN__) || defined(__RADWINRT__)
  #define UNITYEXPORT __declspec(dllexport) __stdcall 
  #define UNITYINTERFACE __stdcall
#elif defined(__RADLINUX__)
  #define UNITYEXPORT __attribute__((visibility("default")))
  #define UNITYINTERFACE
#elif defined(__RADANDROID__)
  #define UNITYEXPORT __attribute__((visibility("default")))
  #define UNITYINTERFACE
#elif defined(__RADMAC__)
  #define UNITYEXPORT __attribute__((visibility("default")))
  #define UNITYINTERFACE
#elif defined(__RADIPHONE__)
  #define UNITYEXPORT __attribute__((visibility("default")))
  #define UNITYINTERFACE
#elif defined(__RADPS4__)
  #define UNITYEXPORT __declspec(dllexport) __attribute__((visibility("default")))
  #define UNITYINTERFACE
#else
  #error Platform!
#endif

static void handlerenderevent( int eventID );

#if 0 //defined(OLDUNITY)   // use for Unity 5.1 and earlier

void UNITYEXPORT UnityRenderEvent( int eventID );
void UnityRenderEvent( int eventID )
{
  handlerenderevent( eventID );
}

enum GfxDeviceRenderer
{
  kGfxRendererOpenGL = 0,          // OpenGL
  kGfxRendererD3D9,                // Direct3D 9
  kGfxRendererD3D11,               // Direct3D 11
  kGfxRendererGCM,                 // Sony PlayStation 3 GCM
  kGfxRendererNull,                // "null" device (used in batch mode)
  kGfxRendererHollywood,           // Nintendo Wii
  kGfxRendererXenon,               // Xbox 360
  kGfxRendererOpenGLES,            // OpenGL ES 1.1
  kGfxRendererOpenGLES20Mobile,    // OpenGL ES 2.0 mobile variant
  kGfxRendererMolehill,            // Flash 11 Stage3D
  kGfxRendererOpenGLES20Desktop,   // OpenGL ES 2.0 desktop variant (i.e. NaCl)
  kGfxRendererCount
};


// Event types for UnitySetGraphicsDevice
enum GfxDeviceEventType {
  kGfxDeviceEventInitialize = 0,
  kGfxDeviceEventShutdown,
  kGfxDeviceEventBeforeReset,
  kGfxDeviceEventAfterReset,
};

//This gets called when the graphics device is set or changed, changes screen size, shuts down, etc. 
void UNITYEXPORT UnitySetGraphicsDevice( void * device, int deviceType, int eventType );
void UnitySetGraphicsDevice( void * device, int deviceType, int eventType )
{
  bottom_up_render_targets = 1; // flip for unity
  switch (eventType)
  {
    case kGfxDeviceEventInitialize:
      switch(deviceType)
      {
#if defined(PLATFORM_HAS_GL)
        case kGfxRendererOpenGL:
        case kGfxRendererOpenGLES:
        case kGfxRendererOpenGLES20Mobile:
        case kGfxRendererOpenGLES20Desktop:
          BinkPluginInit( device, BinkGL );
          break;
#endif
#if defined(PLATFORM_HAS_D3D9)
        case kGfxRendererD3D9:
          BinkPluginInit( device, BinkD3D9 );
          break;
#endif
#if defined(PLATFORM_HAS_D3D11)
        case kGfxRendererD3D11:
          BinkPluginInit( device, BinkD3D11 );
          break;
#endif
      }
      break;
    case kGfxDeviceEventShutdown:
      BinkPluginShutdown();
      break;
#if defined(PLATFORM_HAS_D3D9)
    case kGfxDeviceEventBeforeReset:
      BinkPluginWindowsD3D9BeginReset();
      break;
    case kGfxDeviceEventAfterReset:
      BinkPluginWindowsD3D9EndReset();
      break;
#endif
    default:
      break;
  };
}

#else

#if defined(__RADANDROID__)

#define RAD_ZIP_MALLOC ourmalloc
#define RAD_ZIP_FREE ourfree
#include "rad_zip.h"

static struct {
  rrZip_t zip;
  char filename[1024];
} s_ZipInfo;

#endif

typedef enum UnityGfxRenderer
{
	kUnityGfxRendererOpenGL            =  0, // Legacy OpenGL, removed
	kUnityGfxRendererD3D9              =  1, // Direct3D 9
	kUnityGfxRendererD3D11             =  2, // Direct3D 11
	kUnityGfxRendererGCM               =  3, // PlayStation 3
	kUnityGfxRendererNull              =  4, // "null" device (used in batch mode)
	kUnityGfxRendererOpenGLES20        =  8, // OpenGL ES 2.0
	kUnityGfxRendererOpenGLES30        = 11, // OpenGL ES 3.0
	kUnityGfxRendererGXM               = 12, // PlayStation Vita
	kUnityGfxRendererPS4               = 13, // PlayStation 4
	kUnityGfxRendererXboxOne           = 14, // Xbox One        
	kUnityGfxRendererMetal             = 16, // iOS Metal
	kUnityGfxRendererOpenGLCore        = 17, // OpenGL core
	kUnityGfxRendererD3D12             = 18, // Direct3D 12
	kUnityGfxRendererVulkan                = 21, // Vulkan
} UnityGfxRenderer;


// Event types for UnitySetGraphicsDevice
typedef enum UnityGfxDeviceEventType
{
  kUnityGfxDeviceEventInitialize     = 0,
  kUnityGfxDeviceEventShutdown       = 1,
  kUnityGfxDeviceEventBeforeReset    = 2,
  kUnityGfxDeviceEventAfterReset     = 3,
} UnityGfxDeviceEventType;

typedef void IUnityInterface;

typedef struct UnityInterfaceGUIDtype
{
  U64 GUIDHigh;
  U64 GUIDLow;
} UnityInterfaceGUIDtype;

#if (defined(__RADNT__) || defined(__RADWINRT__)) && !defined(__RAD64__)
typedef UnityInterfaceGUIDtype UnityInterfaceGUID;
#define GetGUID(guid) guid
#else
typedef UnityInterfaceGUIDtype * UnityInterfaceGUID;
#define GetGUID(guid) &guid
#endif

typedef struct IUnityInterfaces
{
  // Returns an interface matching the guid.
  // Returns nullptr if the given interface is unavailable in the active Unity runtime.
  IUnityInterface* (UNITYINTERFACE * GetInterface)(UnityInterfaceGUID guid);

  // Registers a new interface.
  void (UNITYINTERFACE * RegisterInterface)(UnityInterfaceGUID guid, IUnityInterface* ptr);
} IUnityInterfaces;

typedef void (UNITYINTERFACE * IUnityGraphicsDeviceEventCallback)(UnityGfxDeviceEventType eventType);

typedef struct IUnityGraphics
{
  UnityGfxRenderer (UNITYINTERFACE * GetRenderer)(); // Thread safe

  // This callback will be called when graphics device is created, destroyed, reset, etc.
  // It is possible to miss the kUnityGfxDeviceEventInitialize event in case plugin is loaded at a later time,
  // when the graphics device is already created.
  void (UNITYINTERFACE * RegisterDeviceEventCallback)(IUnityGraphicsDeviceEventCallback callback);
  void (UNITYINTERFACE * UnregisterDeviceEventCallback)(IUnityGraphicsDeviceEventCallback callback);
} IUnityGraphics;

typedef void (UNITYINTERFACE * UnityRenderingEvent)(int eventId);

static IUnityInterfaces* s_UnityInterfaces = 0;
static IUnityGraphics* s_Graphics = 0;

static UnityInterfaceGUIDtype IUnityGraphics_GUID = {0x7CBA0A9CA4DDB544ULL,0x8C5AD4926EB17B11ULL};


typedef struct {
  void *ptr;
  int w,h;
} overlay_data_t;

static overlay_data_t overlay_data[8];
static unsigned overlay_get;
static unsigned overlay_put;
static radMutex overlay_mutex;
static S32 has_overlay_mutex;

#define NUM_OVERLAY_DATA (sizeof(overlay_data)/sizeof(*overlay_data))

static void UNITYINTERFACE renderevent( int eventID )
{
  handlerenderevent( eventID );
}

UnityRenderingEvent UNITYEXPORT BinkGetUnityEventFunc( void )
{
  return renderevent;
}

void UNITYEXPORT BinkSetOverlayRenderBuffer(void *colorBuffer, int width, int height)
{
  int oidx;
  overlay_data_t *o;

  if(!has_overlay_mutex) 
  {
    if(!pBinkUtilMutexCreate) 
      return;
    pBinkUtilMutexCreate( &overlay_mutex, 0 );
    has_overlay_mutex = 1;
  }

  if(pBinkUtilMutexLock)
    pBinkUtilMutexLock( &overlay_mutex );
  oidx = overlay_put++ % NUM_OVERLAY_DATA;
  o = overlay_data + oidx;
  o->ptr = colorBuffer;
  o->w = width;
  o->h = height;
  if(pBinkUtilMutexUnlock)
    pBinkUtilMutexUnlock( &overlay_mutex );
}

U64 UNITYEXPORT BinkGetFileOffset(const char *zipFileName, const char *filename) 
{
#if defined(__RADANDROID__)
  U64 i;
  // Note: Presumably only one of these ever will get opened (as its the APK)
  if(strncmp(zipFileName, s_ZipInfo.filename, sizeof(s_ZipInfo.filename)-1)) {
    rr_zip_free(&s_ZipInfo.zip);
    s_ZipInfo.zip = rr_zip_read(zipFileName);
    strncpy(s_ZipInfo.filename, zipFileName, sizeof(s_ZipInfo.filename)-1);
    s_ZipInfo.filename[sizeof(s_ZipInfo.filename)-1] = 0;
  }
  for(i = 0; i < s_ZipInfo.zip.num_files; ++i) {
    rrZipFile_t *f = s_ZipInfo.zip.files + i;
    if(f->method == 0 && strstr(f->filename, filename)) {
      return f->file_pos;
    }
  }
#endif  
  return 0;
}

#if defined(PLATFORM_HAS_D3D9)

typedef struct IUnityGraphicsD3D9
{
  void* /*IDirect3D9**/ (UNITYINTERFACE * GetD3D)();
  void* /*IDirect3DDevice9**/ (UNITYINTERFACE * GetDevice)();
} IUnityGraphicsD3D9;

static UnityInterfaceGUIDtype IUnityGraphicsD3D9_GUID = { 0xE90746A523D53C4CULL,0xAC825B19B6F82AC3ULL };

static IUnityGraphicsD3D9* s_GraphicsD3D9 = 0;

#endif

#if defined(PLATFORM_HAS_D3D11)

typedef struct IUnityGraphicsD3D11
{
  void* /*ID3D11Device**/ (UNITYINTERFACE * GetDevice)();

  void* /*ID3D11Resource**/ (UNITYINTERFACE * TextureFromRenderBuffer)(void* buffer);
} IUnityGraphicsD3D11;
static UnityInterfaceGUIDtype IUnityGraphicsD3D11_GUID = { 0xAAB37EF87A87D748ULL,0xBF76967F07EFB177ULL };

static IUnityGraphicsD3D11* s_GraphicsD3D11 = 0;

#endif

#if defined(PLATFORM_HAS_D3D12)

typedef struct UnityGraphicsD3D12ResourceState UnityGraphicsD3D12ResourceState;
struct UnityGraphicsD3D12ResourceState
{
    void /*ID3D12Resource*/*       resource; // Resource to barrier.
    U32/*D3D12_RESOURCE_STATES*/ expected; // Expected resource state before this command list is executed.
    U32/*D3D12_RESOURCE_STATES*/ current;  // State this resource will be in after this command list is executed.
};

typedef struct UnityGraphicsD3D12PhysicalVideoMemoryControlValues UnityGraphicsD3D12PhysicalVideoMemoryControlValues;
struct UnityGraphicsD3D12PhysicalVideoMemoryControlValues // all values in bytes
{
    U64 reservation;           // Minimum required physical memory for an application [default = 64MB].
    U64 systemMemoryThreshold; // If free physical video memory drops below this threshold, resources will be allocated in system memory. [default = 64MB]
    U64 residencyThreshold;    // Minimum free physical video memory needed to start bringing evicted resources back after shrunken video memory budget expands again. [default = 128MB]
};

typedef struct IUnityGraphicsD3D12v4
{
    void /*ID3D12Device*/* (UNITYINTERFACE * GetDevice)();
    void /*ID3D12Fence*/* (UNITYINTERFACE * GetFrameFence)();
    U64 (UNITYINTERFACE * GetNextFrameFenceValue)();
    U64 (UNITYINTERFACE * ExecuteCommandList)(void /*ID3D12GraphicsCommandList*/ * commandList, int stateCount, UnityGraphicsD3D12ResourceState * states);
    void (UNITYINTERFACE * SetPhysicalVideoMemoryControlValues)(const UnityGraphicsD3D12PhysicalVideoMemoryControlValues * memInfo);
    void /*ID3D12CommandQueue*/* (UNITYINTERFACE * GetCommandQueue)();
} IUnityGraphicsD3D12v4;
static UnityInterfaceGUIDtype IUnityGraphicsD3D12v4_GUID = { 0X498FFCC13EC94006ULL, 0XB18F8B0FF67778C8ULL };

typedef struct IUnityGraphicsD3D12v5
{
    void /*ID3D12Device*/* (UNITYINTERFACE * GetDevice)();
    void /*ID3D12Fence*/* (UNITYINTERFACE * GetFrameFence)();
    U64 (UNITYINTERFACE * GetNextFrameFenceValue)();
    U64 (UNITYINTERFACE * ExecuteCommandList)(void /*ID3D12GraphicsCommandList*/ * commandList, int stateCount, UnityGraphicsD3D12ResourceState * states);
    void (UNITYINTERFACE * SetPhysicalVideoMemoryControlValues)(const UnityGraphicsD3D12PhysicalVideoMemoryControlValues * memInfo);
    void /*ID3D12CommandQueue*/* (UNITYINTERFACE * GetCommandQueue)();
    void /*ID3D12Resource*/* (UNITYINTERFACE * TextureFromRenderBuffer)(void /*UnityRenderBuffer*/ * rb);
} IUnityGraphicsD3D12v5;
static UnityInterfaceGUIDtype IUnityGraphicsD3D12v5_GUID = { 0xF5C8D8A37D37BC42ULL, 0xB02DFE93B5064A27ULL};

static IUnityGraphicsD3D12v5* s_GraphicsD3D12 = 0;

#endif

#ifdef __RADPS4__

typedef struct IUnityGraphicsPS4
{
  void* (UNITYINTERFACE * GetGfxContext)();

  void *(UNITYINTERFACE * AllocateGPUMemory)(size_t size, int alignment);
  void (UNITYINTERFACE * ReleaseGPUMemory)(void *data);

  void *(UNITYINTERFACE * GetCurrentRenderTarget)(int index);
  void *(UNITYINTERFACE * GetCurrentDepthRenderTarget)();
} IUnityGraphicsPS4;

static UnityInterfaceGUIDtype IUnityGraphicsPS4_GUID = { 0xada62b5d78f14e7ULL,0xa60947365e5561cfULL };

static IUnityGraphicsPS4* s_GraphicsPS4 = 0;

static void *AllocateWcGarlic2(U32 Amt, U32 Align)
{
  if ( s_GraphicsPS4 )
  {
    return s_GraphicsPS4->AllocateGPUMemory(Amt, Align);
  }
  return 0;
}

static void FreeWcGarlic2(void * ptr)
{
  if ( s_GraphicsPS4 )
  {
    s_GraphicsPS4->ReleaseGPUMemory(ptr);
  }
}

#include <kernel.h>
#include <sce_atomic.h>

#define MAXALLOCS 64
static void *addrs[MAXALLOCS];
static U32 amts[MAXALLOCS];
static off_t offs[MAXALLOCS];

static void *AllocateWbOnion2(U32 Amt, U32 Align)
{
  int i;

  Align = (Align + 16383) & ~16383; // round up to multiple of 16k
  Amt = (Amt + 16383) & ~16383; // round up to multiple of 16k

  off_t offset;
  int32_t ret = sceKernelAllocateDirectMemory(0, SCE_KERNEL_MAIN_DMEM_SIZE,
                                              Amt, Align, SCE_KERNEL_WB_ONION, &offset);
  if (ret < 0)
    return 0;

  void *addr = NULL;
  ret = sceKernelMapDirectMemory(&addr, Amt, SCE_KERNEL_PROT_CPU_READ | SCE_KERNEL_PROT_CPU_WRITE | SCE_KERNEL_PROT_GPU_ALL, 0, offset, Align);
  if (ret < 0)
    return 0;

  // find a slot and drop it in
  for(i=0;i<MAXALLOCS;i++)
  {
    if (sceAtomicCompareAndSwap64( (int64_t*)&addrs[i], 0, (int64_t)(UINTa)addr ) == 0 )
    {
      amts[i]=Amt;
      offs[i]=offset;
      break;
    }
  }

  return addr;
}

static void FreeWbOnion2(void * ptr)
{
  int i;

  for(i=0;i<MAXALLOCS;i++)
  {
    U32 amt = amts[i];
    off_t offset = offs[i];
    if (sceAtomicCompareAndSwap64( (int64_t*)&addrs[i], (int64_t)(UINTa)ptr, 0 ) == (int64_t)(UINTa)ptr )
    {
      sceKernelMunmap( ptr, amt );
      sceKernelReleaseDirectMemory( offset, amt );
      break;
    }
  }
}

static BINKPLUGINPS4DEVICE ps4_allocator = { AllocateWcGarlic2, FreeWcGarlic2, AllocateWbOnion2, FreeWbOnion2 };

#endif

#if defined(PLATFORM_HAS_METAL)

void * get_binkplugin_metal_device( void * metal_interface );
void set_binkplugin_perframe_metal( void * buffer );
void * get_metal_guid();
static void* metal_unity_interface = 0;

#endif

static S32 unity_gfx_initialized = 0;

//This gets called when the graphics device is set or changed, changes screen size, shuts down, etc. 
static void UNITYINTERFACE OnGraphicsDeviceEvent(UnityGfxDeviceEventType eventType)
{
  int deviceType;

  if ( s_Graphics == 0 )
    return;

  deviceType =  s_Graphics->GetRenderer();
  
  bottom_up_render_targets = 1; // flip for unity
  switch (eventType)
  {
    case kUnityGfxDeviceEventInitialize:
      unity_gfx_initialized = 1;
      switch(deviceType)
      {
#if defined(PLATFORM_HAS_METAL)
        case kUnityGfxRendererMetal:
          if ( metal_unity_interface == 0 )
          {
            metal_unity_interface = s_UnityInterfaces->GetInterface( get_metal_guid() );
            if ( metal_unity_interface == 0 ) return;
          }
          BinkPluginInit( get_binkplugin_metal_device( metal_unity_interface ), BinkMetal );
          break;
#endif
#if defined(PLATFORM_HAS_GL)
        case kUnityGfxRendererOpenGL:
        case kUnityGfxRendererOpenGLES20:
        case kUnityGfxRendererOpenGLES30:
        case kUnityGfxRendererOpenGLCore:
          BinkPluginInit( 0, BinkGL );
          break;
#endif
#if defined(__RADPS4__)
        case kUnityGfxRendererPS4:
          if ( s_GraphicsPS4 == 0 )
          {
            s_GraphicsPS4 = (IUnityGraphicsPS4*)s_UnityInterfaces->GetInterface( GetGUID(IUnityGraphicsPS4_GUID) );
            if ( s_GraphicsPS4 == 0 ) return;
          }
          BinkPluginInit( &ps4_allocator, BinkPS4 );
          break;
#endif
#if defined(PLATFORM_HAS_D3D9)
        case kUnityGfxRendererD3D9:
          if ( s_GraphicsD3D9 == 0 )
          {
            s_GraphicsD3D9 = (IUnityGraphicsD3D9*)s_UnityInterfaces->GetInterface( GetGUID(IUnityGraphicsD3D9_GUID) );
            if ( s_GraphicsD3D9 == 0 ) return;
          }
          BinkPluginInit( s_GraphicsD3D9->GetDevice(), BinkD3D9 );
          break;
#endif
#if defined(PLATFORM_HAS_D3D11)
        case kUnityGfxRendererD3D11:
          if ( s_GraphicsD3D11 == 0 )
          {
            s_GraphicsD3D11 = (IUnityGraphicsD3D11*)s_UnityInterfaces->GetInterface( GetGUID(IUnityGraphicsD3D11_GUID) );
            if ( s_GraphicsD3D11 == 0 ) return;
          }
          BinkPluginInit( s_GraphicsD3D11->GetDevice(), BinkD3D11 );
          break;
#endif
#if defined(PLATFORM_HAS_D3D12)
        case kUnityGfxRendererD3D12:
          if ( s_GraphicsD3D12 == 0 )
          {
            s_GraphicsD3D12 = (IUnityGraphicsD3D12v5*)s_UnityInterfaces->GetInterface( GetGUID(IUnityGraphicsD3D12v5_GUID) );
            if ( s_GraphicsD3D12 == 0 ) return;
          }
          {
            static BINKPLUGIND3D12DEVICE d3d12;
            d3d12.device = s_GraphicsD3D12->GetDevice();
            d3d12.queue = s_GraphicsD3D12->GetCommandQueue();
            d3d12.render_target_format = 28 /* DXGI_FORMAT_R8G8B8A8_UNORM */;
            BinkPluginInit( &d3d12, BinkD3D12 );
          }
          break;
#endif
      }
      if(!has_overlay_mutex && pBinkUtilMutexCreate)
      {
        pBinkUtilMutexCreate( &overlay_mutex, 0 );
        has_overlay_mutex = 1;
      }
      break;
    case kUnityGfxDeviceEventShutdown:
      if ( pBinkUtilMutexDestroy && has_overlay_mutex )
        pBinkUtilMutexDestroy( &overlay_mutex );
      BinkPluginShutdown();
      break;
#if defined(PLATFORM_HAS_D3D9)
    case kUnityGfxDeviceEventBeforeReset:
      BinkPluginWindowsD3D9BeginReset();
      break;
    case kUnityGfxDeviceEventAfterReset:
      BinkPluginWindowsD3D9EndReset();
      break;
#endif
    default:
      break;
  };
}

#endif

// static unity loading functions

#if (defined(__RADWIN__) || defined(__RADWINRT__)) && !defined(__RADX64__)
#pragma comment(linker, "/EXPORT:UnityPluginLoad=_UnityPluginLoad@4")
#endif

void UNITYEXPORT UnityPluginLoad(IUnityInterfaces* unityInterfaces)
{
  s_UnityInterfaces = unityInterfaces;
  s_Graphics = (IUnityGraphics*)unityInterfaces->GetInterface( GetGUID(IUnityGraphics_GUID) );
  
  s_Graphics->RegisterDeviceEventCallback(OnGraphicsDeviceEvent);

#ifdef __RADPS4__
  // With Unity, when using core-0 for bink decoding causes audio hiccups
  async_core_start = 1;
#endif
  
#ifndef __RADPS4__ // 5.6 workaround, don't do this on PS4
  // Run OnGraphicsDeviceEvent(initialize) manually on plugin load
  // to not miss the event in case the graphics device is already initialized
  OnGraphicsDeviceEvent(kUnityGfxDeviceEventInitialize);
#endif
}

#if (defined(__RADWIN__) || defined(__RADWINRT__)) && !defined(__RADX64__)
#pragma comment(linker, "/EXPORT:UnityPluginUnload=_UnityPluginUnload@0")
#endif

void UNITYEXPORT UnityPluginUnload()
{
}

static void setup_overlay_data() {
  int oidx;
  overlay_data_t *o;

  if(pBinkUtilMutexLock)
    pBinkUtilMutexLock( &overlay_mutex );
  oidx = overlay_get++ % NUM_OVERLAY_DATA;
  o = overlay_data + oidx;

  #if defined(PLATFORM_HAS_METAL)
  set_binkplugin_perframe_metal(o->ptr);
  #endif

  #if defined(PLATFORM_HAS_D3D12)
  if(s_Graphics->GetRenderer() == kUnityGfxRendererD3D12) 
  {
    BINKSCREENGPUDATA screenInfo = {0};
    screenInfo.resource = s_GraphicsD3D12->TextureFromRenderBuffer(o->ptr);
    screenInfo.resource_state = 0x4; // D3D12_RESOURCE_STATE_RENDER_TARGET
    screenInfo.width = o->w;
    screenInfo.height = o->h;
    BinkPluginSetPerFrameInfo(&screenInfo);
  }
  #endif

  if(pBinkUtilMutexUnlock)
    pBinkUtilMutexUnlock( &overlay_mutex );
}

static void handlerenderevent( int eventID )
{
  if(!unity_gfx_initialized) 
    OnGraphicsDeviceEvent(kUnityGfxDeviceEventInitialize);

  if ( !csuccessful_init || !gsuccessful_init )
    return;

  #ifdef __RADPS4__
  BinkPluginSetPerFrameInfo(s_GraphicsPS4->GetGfxContext());
  #endif
  
  switch( eventID )
  {
    case ProcessOnly:
      process_binks( 0, 0 );
      break;
    case ProcessOnlyNoWait:
      process_binks( 0, 0 );
      break;
    case ProcessCloses:
     again: 
      lock_list();
      {
        BINKPLUGIN * bp;
        bp = all;
        while( bp )
        {
          if ( bp->closing )
          {
            unlock_list();
            process_binks( 1, 0 );
            goto again;
          }
          bp = bp->next;
        }
      }
      unlock_list();
      break;
    case ProcessAndDraw:
      setup_overlay_data();
      if(use_gpu) 
      {
        draw_videos( 1, 1 );
        process_binks( 1, 0 );
      }
      else
      {
        process_binks( 1, 0 );
        draw_videos( 1, 1 );
      }
      break;
    case ProcessAndDrawNoWait:
      setup_overlay_data();
      if(use_gpu) 
      {
        draw_videos( 1, 1 );
        process_binks( 0, 0 );
      } 
      else
      {
        process_binks( 0, 0 );
        draw_videos( 1, 1 );
      }
      break;
    case DrawOnly:
      setup_overlay_data();
      draw_videos( 1, 1 );
      break;
    case DrawToTexturesOnly:
      draw_videos( 1, 0 );
      break;
    case DrawOverlaysOnly:
      setup_overlay_data();
      draw_videos( 0, 1 );
      break;
  }
}


