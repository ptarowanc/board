/* vim: set softtabstop=2 shiftwidth=2 expandtab : */
#include "binkplugin.h"
#include <stddef.h> // size_t

#include "bink.h"

#include "binkpluginos.h"
#include "binktextures.h"
#include "binkpluginGPUAPIs.h"
#include "telemetryplugin.h"

enum STATE
{
  ALLOC_TEXTURES = 0,
  NO_TEXTURES = 1,  // textures couldn't be allocated
  IDLE = 2,
  LOCKED = 3,
  DECOMPRESSING = 4,
  DO_NEXT = 5,      
  GOTOING = 6,
  PAUSED = 7,
  IN_RESET = 8, // dx9 only
  AT_END = 9,
};

#define TRIPLE_BUFFERING 3
#define MAX_OVERLAYS_PER_FRAME 16
#define MAX_OVERLAY_DRAWS (TRIPLE_BUFFERING*MAX_OVERLAYS_PER_FRAME)  // triple buffering and 16 draws each
#define MAX_RTEXTURES_CACHE 64 // cache N views, before reuse

typedef struct DRAWINFO
{
  U64 frame;
  void * to_texture;

  F32 x0,y0,x1,y1;
  S32 depth;

  U32 to_texture_width;
  U32 to_texture_height;
} DRAWINFO;

typedef struct radMutex
{
  U8 data[ 192 ];
} radMutex;

struct BINKPLUGIN
{
  radMutex mutex;  
  HBINK b;
  BINKTEXTURES * textures;
  struct BINKPLUGIN * next;
  U32 state;
  U32 loops;
  S32 closing;  
  U32 skips;
  S32 paused_frame;
  U32 num_tracks;
  S32 tracks[ 9 ];
  S32 finished_one;
  U32 start_goto_frame;
  U32 goto_frame;
  U32 goto_time;
  U32 snd_track_type;
  F32 alpha;
 
  DRAWINFO draws[MAX_OVERLAY_DRAWS]; 
};

static S32 ginit = 0;
static S32 gsuccessful_init = 0;

static S32 cinit = 0;
static S32 csuccessful_init = 0;

static S32 cdo_async = 1;
static S32 async_core_start = 0;

static BINKPLUGIN * all = 0;
static char pierr[256];
char BinkPIPath[512];
static void * gdevice;
void * gcontext;
#define NOGRAPHICSAPI 32
static U32 gapitype = NOGRAPHICSAPI; //
static S32 use_gpu = 0;
static S32 check_gpu = 0;
static S32 currently_reset = 0;
static S32 bottom_up_render_targets = 0;

static U32 limit_speakers_to = 2;

#define PB_WINDOW 32
static U32 processbinks_cnt;
static U32 processbinks_pos;
static U32 processbinks_times[PB_WINDOW];

static U64 drawvideo_frame = 1; // 0 is special, start after it

static U32 num_cpus = 0;
static U32 threads[8];
static S32 thread_count;
static radMutex listlock;

#if defined(PLATFORM_HAS_D3D12) || defined(PLATFORM_HAS_METAL)
BINKSCREENGPUDATA screen_data; // only used on d3d12,metal right now
#endif

#if defined(__RADWIN__)
static S32 using_xaudio = 0;
#endif

//========================================================
// render target cache stuff

struct RENDERTARGETVIEW
{
  U64 frame; // last drawvideo_frame this was used
  void * ptr;
  void * view;
  U32 within_frame; // draw count *within* the frame this was used
};

static RENDERTARGETVIEW renderview_cache[MAX_RTEXTURES_CACHE];

#define ALL_RENDER_TARGET_VIEWS 0xffffffffffffffffULL
typedef RENDERTARGETVIEW * allocaterendertargetview_func( void * render_texture_target );
typedef void freerendertargetview_func( RENDERTARGETVIEW * render_view );


static void free_cached_rendertargetview( S32 view_index, freerendertargetview_func * free_rtv )
{
  if ( renderview_cache[ view_index ].frame )
  {
    free_rtv( renderview_cache[ view_index ].view );
    ourmemset( &renderview_cache[ view_index ], 0, sizeof( renderview_cache[ 0 ] ) );
  }
}


static void * get_cached_rendertargetview( void * texture, U64 frame, U32 within_frame, allocaterendertargetview_func * alloc_rtv, freerendertargetview_func * free_rtv )
{
  S32 vi, ovi;

  for( vi = 0 ; vi < MAX_RTEXTURES_CACHE ; vi++ )
  {
    if ( renderview_cache[ vi ].ptr == texture )
    {
      return renderview_cache[ vi ].view; // success, return cached index
    }
  }

  // look for unused entry and fill it
  for( vi = 0 ; vi < MAX_RTEXTURES_CACHE ; vi++ )
  {
    if (renderview_cache[ vi ].frame == 0 ) // is entry unused?
    {
     use: 
      renderview_cache[ vi ].within_frame = within_frame;
      renderview_cache[ vi ].ptr = texture;
      renderview_cache[ vi ].view = alloc_rtv( texture );
      renderview_cache[ vi ].frame = frame;
      return renderview_cache[ vi ].view; // success, return newly cached index
    }
  }

  // if we got here, there were no empties, so find the oldest
  ovi = 0;
  for( vi = 1 ; vi < MAX_RTEXTURES_CACHE ; vi++ )
  {
    if ( ( renderview_cache[ vi ].frame < renderview_cache[ ovi ].frame ) ||
         ( ( renderview_cache[ vi ].frame == renderview_cache[ ovi ].frame ) &&
           ( renderview_cache[ vi ].within_frame < renderview_cache[ ovi ].within_frame ) ) )
    {
      ovi = vi;
    }
  }
  
  // now free oldest
  free_cached_rendertargetview( ovi, free_rtv );
  
  // now jump back up to find and use it
  vi = ovi;
  goto use;
}


static void free_cached_rendertargetviews_before( U64 before )
{
  S32 i;
  switch( gapitype )
  {
#if defined(PLATFORM_HAS_D3D12)
    case BinkD3D12:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_d3d12 );
      break;
#endif
#if defined(PLATFORM_HAS_D3D11)
    case BinkD3D11:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_d3d11 );
      break;
#endif
#if defined(PLATFORM_HAS_D3D9)
    case BinkD3D9:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_d3d9 );
      break;
#endif
#if defined(__RADPS4__)
    case BinkPS4:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_ps4 );
      break;
#endif
#if defined(PLATFORM_HAS_GL)
    case BinkGL:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_gl );
      break;
#endif
#if defined(PLATFORM_HAS_METAL)
    case BinkMetal:
      for( i = 0 ; i < MAX_RTEXTURES_CACHE ; i++ )
        if ( ( renderview_cache[i].frame ) && ( renderview_cache[i].frame < before ) )
          free_cached_rendertargetview( i, freerendertargetview_metal );
      break;
#endif
  }
}

//========================================================


static void alloc_list_lock( void )
{
  pBinkUtilMutexCreate( &listlock, 0 );
}


static void free_list_lock( void )
{
  if ( pBinkUtilMutexDestroy )
    pBinkUtilMutexDestroy( &listlock );
}

static void lock_list( void )
{
  pBinkUtilMutexLock( &listlock );
}


static void unlock_list( void )
{
  pBinkUtilMutexUnlock( &listlock );
}


static void * RADLINK ourmalloc( U64 bytes )
{
  return osmalloc( bytes );
}

static void RADLINK ourfree( void * ptr )
{
  osfree( ptr );
}

void RADLINK ourstrcpy(char * dest, char const * src )
{
  for(;;)
  {
    char c = *src++;
    *dest++=c;
    if (c==0) break;
  }
}

// only small memcpys
void RADLINK ourmemcpy(void * dest, void const * src, int bytes )
{
  unsigned char *d=(unsigned char*)dest;
  unsigned char *s=(unsigned char*)src;
  while(bytes)
  {
    *d++=*s++;
    --bytes;
  }
}

// only small memsets
void RADLINK ourmemset(void * dest, char val, int bytes )
{
  unsigned char *d=(unsigned char*)dest;
  while(bytes)
  {
    *d++=val;
    --bytes;
  }
}

PLUG_IN_FUNC_DEF( void ) BinkPluginSetError( char const * err )
{
  oserr( err );
  ourstrcpy( pierr, err );
}

PLUG_IN_FUNC_DEF( void ) BinkPluginSetPath( char const * path )
{
  ourstrcpy( BinkPIPath, path );
}

PLUG_IN_FUNC_DEF( void ) BinkPluginAddError( char const * err )
{
  char * pe;
  oserr( err );
  pe = pierr;
  while( pe[0] ) ++pe;
  ourstrcpy( pe, err );
}

extern void SetPS4Allocators( void * ptr );

static S32 init_graphics( void )
{
  void * context;

  context = 0;
  switch( gapitype )
  {
#if defined(PLATFORM_HAS_D3D12)
    case BinkD3D12:
      if ( !setup_d3d12( gdevice, use_gpu, &context ) )
        goto err;
      break;
#endif
#if defined(PLATFORM_HAS_D3D11)
    case BinkD3D11:
      if ( !setup_d3d11( gdevice, use_gpu, &context ) )
        goto err;
      break;
#endif
#if defined(PLATFORM_HAS_D3D9)
    case BinkD3D9:
      if ( !setup_d3d9( gdevice ) )
        goto err;
      break;
#endif
#if defined(__RADPS4__)
    case BinkPS4:
      SetPS4Allocators( gdevice );
      if ( !setup_ps4( use_gpu ) )
        goto err;
      break;
#endif
#if defined(PLATFORM_HAS_GL)
    case BinkGL:
      if ( !setup_gl( use_gpu ) )
        goto err;
      break;
#endif
#if defined(PLATFORM_HAS_METAL)
    case BinkMetal:
      if ( !setup_metal( gdevice, use_gpu ) )
        goto err;
      break;
#endif
    default:
    err:
      BinkPluginSetError( "Could not create shaders." );
      return 0;
  }

  if ( context )
    gcontext = context;
  return 1;
}  


static S32 plugin_init()
{
  S32 i,j;

  if ( cinit )
  {
    //set err to already init
    return csuccessful_init;
  }
  cinit = 1;
  csuccessful_init = 0;

  plugin_load_funcs();

  alloc_list_lock();

  pBinkSetMemory( ourmalloc, ourfree );

  // create decompression threads
  num_cpus = pBinkUtilCPUs();
  thread_count = ( num_cpus > 4 ) ? 4 : num_cpus;

  j = 0;
  for( i = async_core_start ; i < thread_count+async_core_start ; i++ )
  {
    if ( pBinkStartAsyncThread( i, 0 ) )
      threads[ j++ ] = i;
  }
  thread_count = j;
  
  if ( j == 0 )
  {
    BinkPluginSetError( pBinkGetError() );
    return 0;
  }

#if defined(__RADWIN__)  // windows, xenon, xboxone, winrt
  if ( pBinkSetSoundSystem2(pBinkOpenXAudio2,0,0) )
  {
    using_xaudio = 1;
  }
  else
  {
#if !defined(__RADWINRT__) && !defined(__RADXBOXONE__)
    BinkPluginSetError("XAudio failed, switching to DirectSound.");
    if ( pBinkSetSoundSystem(pBinkOpenDirectSound,0) == 0 )
      BinkPluginSetError("XAudio and DirectSound both failed.");
#endif
  }
#endif

#if defined(__RADANDROID__)
  pBinkSetSoundSystem2(pBinkOpenRADSS2,0,0);
#endif

#if defined(__RADLINUX__) || defined(__RADMAC__) || defined(__RADIPHONE__)
  pBinkSetSoundSystem(pBinkOpenRADSS,0);
#endif

#ifdef __RADPS4__
  pBinkSetSoundSystem(pBinkOpenRADSS,8);
#endif

  csuccessful_init = 1;
  return 1;
}

PLUG_IN_FUNC_DEF( S32 ) BinkPluginInit( void * device, U32 graphics_api )
{
  if ( ginit )
  {
    //set err to already init
    return gsuccessful_init;
  }
  ginit = 1;
  gsuccessful_init = 0;

  gapitype = graphics_api;
  gdevice = device;

  if ( init_graphics() == 0 )
    return 0;

  gsuccessful_init = 1;
  return 1;
}


PLUG_IN_FUNC_DEF( void ) BinkPluginTurnOnGPUAssist( void )
{
  use_gpu = 1;
  check_gpu = 1;
}


PLUG_IN_FUNC_DEF( void ) BinkPluginShutdown( void )
{
  S32 i;

  if ( ( cinit == 0 ) && ( ginit == 0 ) )
  {
    //set err to not inited
    return;
  }
 
  // mark as closed
  while( all )
  {
    lock_list();
    {
      BINKPLUGIN * bp;
      bp = all;
      while( bp )
      {
        bp->closing = 1;
        bp = bp->next;
      }
    }
    unlock_list();
    // close
    BinkPluginProcessBinks( -1 );
  }

  // empty out the render target view cache
  free_cached_rendertargetviews_before( ALL_RENDER_TARGET_VIEWS );

  if ( all != 0 )
  {
    //set err to plugins still alloced
    return;
  }

  cinit = ginit = 0;

  // signal them all to stop
  if (pBinkRequestStopAsyncThread)
    for( i = 0 ; i < thread_count ; i++ )
      pBinkRequestStopAsyncThread( i );

  // now wait
  if (pBinkWaitStopAsyncThread)
    for( i = 0 ; i < thread_count ; i++ )
      pBinkWaitStopAsyncThread( i );

  thread_count = 0;

  switch( gapitype )
  {
#if defined(PLATFORM_HAS_D3D12)
    case BinkD3D12:
      shutdown_d3d12();
      break;
#endif
#if defined(PLATFORM_HAS_D3D11)
    case BinkD3D11:
      shutdown_d3d11();
      break;
#endif
#if defined(PLATFORM_HAS_D3D9)
    case BinkD3D9:
      shutdown_d3d9();
      break;
#endif
#if defined(__RADPS4__)
    case BinkPS4:
      shutdown_ps4();
      break;
#endif
#if defined(PLATFORM_HAS_GL)
    case BinkGL:
      shutdown_gl();
      break;
#endif
#if defined(PLATFORM_HAS_METAL)
    case BinkMetal:
      shutdown_metal();
      break;
#endif
  }
  
  gapitype = NOGRAPHICSAPI;
  use_gpu = 0;

#if defined(__RADWIN__)
  if ( using_xaudio )
  {
    if ( pBinkSetSoundSystem )
      pBinkSetSoundSystem2(pBinkOpenXAudio2,(UINTa)-1,0);
    using_xaudio = 0;
  }
#endif

  if (pBinkFreeGlobals)
    pBinkFreeGlobals();

  free_list_lock();

  osmemoryreset();
}


PLUG_IN_FUNC_DEF( char const * ) BinkPluginError( void )
{
  return pierr;
}


static void alloc_bink_lock( BINKPLUGIN * bp )
{
  pBinkUtilMutexCreate( &bp->mutex, 1 );
}


static void free_bink_lock( BINKPLUGIN * bp )
{
  pBinkUtilMutexDestroy( &bp->mutex );
}


static void lock_bink( BINKPLUGIN * bp )
{
  pBinkUtilMutexLock( &bp->mutex );
}


static void unlock_bink( BINKPLUGIN * bp )
{
  pBinkUtilMutexUnlock( &bp->mutex );
}


static S32 try_lock_bink( BINKPLUGIN * bp, S32 timeout )
{
  return pBinkUtilMutexLockTimeOut( &bp->mutex, timeout ) ? 1 : 0;
}


static void start_bink_frame( BINKPLUGIN * bp )
{
  if(pTmPluginEnter) pTmPluginEnter("Start_Bink_texture_update");
  Start_Bink_texture_update( bp->textures );
  if(pTmPluginLeave) pTmPluginLeave();
  bp->state = LOCKED;
  if(pTmPluginEnter) pTmPluginEnter("BinkDoFrame");
  if(cdo_async)
    pBinkDoFrameAsyncMulti( bp->b, threads, thread_count ); 
  else
    pBinkDoFrame( bp->b ); 
  if(pTmPluginLeave) pTmPluginLeave();
  bp->state = DECOMPRESSING;
}


static S32 time_left( U32 start, S32 ms, U32 wait_amt )
{
  S32 wait;
  if ( ms <= 0 )
    wait = ms;
  else
  {
    U32 d = pRADTimerRead() - start;
    if ( d >= (U32) ms )
      wait = 0;
    else
      wait = wait_amt;
  }
  return wait;
}

static void process_binks( S32 ms, S32 when_idle_reset )
{
  BINKPLUGIN * bp;  
  U32 start;

  if(pTmPluginEnter) pTmPluginEnter("process_binks");

  if ( currently_reset )
  {
    if(pTmPluginLeave) pTmPluginLeave();
    return;
  }

  // if gpu is flipped on, do it
  if ( check_gpu )
  {
    check_gpu = 0;
    init_graphics();
  }

  start = pRADTimerRead();

  lock_list();

  // Save state
  if(pTmPluginEnter) pTmPluginEnter("begindraw");
  switch ( gapitype )
  {
#if defined(PLATFORM_HAS_D3D11)
    case BinkD3D11:
      begindraw_d3d11();
      break;
#endif
#if defined(PLATFORM_HAS_D3D9)
    case BinkD3D9:
      begindraw_d3d9();
      break;
#endif
#if defined(PLATFORM_HAS_GL)
    case BinkGL:
      begindraw_gl();
      break;
#endif
  }
  if(pTmPluginLeave) pTmPluginLeave();

  // time the process call rate - only record a new frame if it is 
  //   at least 8 ms since the last one (half 60 Hz rate)
  if ( ( start - processbinks_times[ processbinks_pos ] ) > 8 )
  {
    ++processbinks_cnt;
    processbinks_pos = ( processbinks_pos + 1 ) & ( PB_WINDOW - 1 );
    processbinks_times[ processbinks_pos ] = start;
  }

  bp = all;
  while( bp )
  {
    U32 wait;

    wait = time_left( start, ms, 1 );
    if(pTmPluginEnter) pTmPluginEnter("try_lock_bink");
    if ( try_lock_bink( bp, wait ) )
    {
      if(pTmPluginLeave) pTmPluginLeave();
      // if this is closing, shut it down
      if ( bp->closing )
      {
        BINKPLUGIN * par;
        // remove bp from the all list (while it is locked)
        if ( all == bp )
        {
          all = bp->next;
          par = 0;
        }
        else
        {
          par = all;
          while ( par )
          {
            if ( par->next == bp )
            {
              par->next = bp->next;
              break;
            }
            par = par->next;
          }
        }
        if(pTmPluginEnter) pTmPluginEnter("unlock_list");
        unlock_list();  // unlock now that it's removed from the list
        if(pTmPluginLeave) pTmPluginLeave();

        // wait until compress
        if ( bp->state == DECOMPRESSING )
        {
          // if there is a async going on and it's not finished, close it the next frame
          wait = time_left( start, ms, 1000 );
          if(pTmPluginEnter) pTmPluginEnter("BinkDoFrameAsyncWait");
          if ( cdo_async && !pBinkDoFrameAsyncWait( bp->b, wait ) )
            goto cont;
          if(pTmPluginLeave) pTmPluginLeave();
          bp->state = LOCKED;
        }

        // unlock the bink textures
        if ( bp->state == LOCKED )
        {
          if(pTmPluginEnter) pTmPluginEnter("Finish_Bink_texture_update");
          Finish_Bink_texture_update( bp->textures );
          if(pTmPluginLeave) pTmPluginLeave();
          bp->finished_one = 1;
          bp->state = IDLE;
        }

        // if we have textures, free them
        if ( ( bp->state != ALLOC_TEXTURES ) && ( bp->state != NO_TEXTURES ) )
        {
          Free_Bink_textures( bp->textures );
          bp->state = NO_TEXTURES;
        }

        // free the bink
        if ( bp->b )
        {
          pBinkClose( bp->b );
          bp->b = 0;
        }

        // unlock the bink, free the mutex, delete the plug-in and advance
        unlock_bink( bp );
        free_bink_lock( bp );
        ourfree( bp );
        lock_list();
        // find the new next bink ptr (rescan if list has changed)
        if ( par == 0 )
        {
          bp = all;
        }
        else
        {
          BINKPLUGIN * f;
          f = all;
          while( f )
          {
            if ( f == par )
              break;
            f = f->next;
          }
          bp = ( f == 0 ) ? 0 : f->next;
        }
      }
      else if ( gsuccessful_init )
      {
         U32 start_bink_time = pRADTimerRead();

        if(pTmPluginEnter) pTmPluginEnter("unlock_list");
        unlock_list();  // unlock list
        if(pTmPluginLeave) pTmPluginLeave();

        // allocate textures, if we haven't
        if ( bp->state == ALLOC_TEXTURES )
        {
          // start with error state
          bp->state = NO_TEXTURES;
          if(pTmPluginEnter) pTmPluginEnter("createtextures");
          switch ( gapitype )
          {
#if defined(PLATFORM_HAS_D3D12)
            case BinkD3D12:
              bp->textures = createtextures_d3d12( bp->b );
              break;
#endif
#if defined(PLATFORM_HAS_D3D11)
            case BinkD3D11:
              bp->textures = createtextures_d3d11( bp->b );
              break;
#endif
#if defined(PLATFORM_HAS_D3D9)
            case BinkD3D9:
              bp->textures = createtextures_d3d9( bp->b );
              break;
#endif
#if defined(__RADPS4__)
            case BinkPS4:
              bp->textures = createtextures_ps4( bp->b );
              break;
#endif
#if defined(PLATFORM_HAS_GL)
            case BinkGL:
              bp->textures = createtextures_gl( bp->b );
              break;
#endif
#if defined(PLATFORM_HAS_METAL)
            case BinkMetal:
              bp->textures = createtextures_metal( bp->b );
              break;
#endif
          }
          if(pTmPluginLeave) pTmPluginLeave();
          if ( bp->textures )
          {
            bp->state = IDLE;
            if ( !when_idle_reset )
              start_bink_frame( bp );
          }
        }

        // see if the decompression has finished, advance frame
        if ( bp->state == DECOMPRESSING )
        {
          wait = time_left( start, ms, 1000 );
          if(pTmPluginEnter) pTmPluginEnter("BinkDoFrameAsyncWait");
          if ( !cdo_async || pBinkDoFrameAsyncWait( bp->b, wait ) )
          {
            bp->state = DO_NEXT;
          }
          if(pTmPluginLeave) pTmPluginLeave();
        }

        // handle moving to next frame
        if ( bp->state == DO_NEXT )
        {
          if(pTmPluginEnter) pTmPluginEnter("DO_NEXT");
          if ( bp->start_goto_frame )
          {
            pBinkGoto( bp->b, bp->start_goto_frame, BINKGOTOQUICK );
            bp->start_goto_frame = 0;
          }
          else
          {
           do_next:
            // handle looping
            if ( bp->b->FrameNum == bp->b->Frames ) 
            {
              if ( bp->loops > 1 )
              {
                --bp->loops;
              }
              else if ( bp->loops == 1 )
              {
                bp->state = AT_END;
                goto cont;
              }
            }
            
            if ( bp->loops != 1 )
              pBinkSetWillLoop( bp->b, 1 );

            pBinkNextFrame( bp->b );
          }
          if(pTmPluginLeave) pTmPluginLeave();
          bp->state = LOCKED;
        }

        // if the texture is locked, unlock it
        if ( bp->state == LOCKED )
        {
          if(pTmPluginEnter) pTmPluginEnter("Finish_Bink_texture_update");
          Finish_Bink_texture_update( bp->textures );
          if(pTmPluginLeave) pTmPluginLeave();
          bp->finished_one = 1;
          bp->state = IDLE;
        }
 
        if ( ( bp->state == IN_RESET ) && ( !when_idle_reset ) )
        {
          After_Reset_Bink_textures( bp->textures );
          bp->state = IDLE;
        }

        // if we're idle, start the next frame
        if ( bp->state == IDLE )
        {
          if ( when_idle_reset )
          {
            Before_Reset_Bink_textures( bp->textures );
            bp->state = IN_RESET;
          }
          else
          {
            // check for pause
            if ( bp->paused_frame )
            {
              if ( ( bp->paused_frame < 0 ) || ( (U32)bp->paused_frame == bp->b->FrameNum ) )
              {
                bp->paused_frame = 0;
                pBinkPause( bp->b, 1 ); 
              }
            }

            if ( bp->start_goto_frame )
            {
              pBinkGoto( bp->b, bp->start_goto_frame, BINKGOTOQUICK );
              bp->start_goto_frame = 0;
            }
   
            if ( bp->goto_frame )
            {
              if ( bp->goto_frame == bp->b->FrameNum )
              {
                bp->goto_frame = 0;
                pBinkSetSoundOnOff( bp->b, 1 );
              }
              else
              {
                // keep reading ahead
                start_bink_frame( bp );
                if(pTmPluginEnter) pTmPluginEnter("BinkDoFrameAsyncWait");
                if(cdo_async)
                  pBinkDoFrameAsyncWait( bp->b, -1 );
                if(pTmPluginLeave) pTmPluginLeave();
                bp->state = DO_NEXT;
                if ( ( pRADTimerRead() - start_bink_time ) <= bp->goto_time )
                  goto do_next;
              }
            }


            if(pTmPluginEnter) pTmPluginEnter("BinkWait");
            if ( ( !pBinkWait( bp->b ) ) || ( bp->goto_frame ) )
            {
              if(pTmPluginLeave) pTmPluginLeave();

              if(pTmPluginEnter) pTmPluginEnter("start_bink_frame");
              start_bink_frame( bp );
              if(pTmPluginLeave) pTmPluginLeave();

              // do we need to skip a frame?
              if ( pBinkShouldSkip( bp->b ) )
              {
                ++bp->skips;
                // when we skip, we block until we are done, and then re-run the state machine from nextframe
                if(pTmPluginEnter) pTmPluginEnter("BinkDoFrameAsyncWait");
                if(cdo_async)
                  pBinkDoFrameAsyncWait( bp->b, -1 );
                if(pTmPluginLeave) pTmPluginLeave();
                bp->state = DO_NEXT;
                goto do_next;
              }
            }
            else
            {
              if(pTmPluginLeave) pTmPluginLeave();
            }
          }
        }

        // unlock bink, lock the list and advance
       cont: 
        unlock_bink( bp );
        lock_list();
        bp = bp->next;
      }
    }
    else
    {
      if(pTmPluginLeave) pTmPluginLeave();
    }
  }
  
  // Reset GPU state
  if(pTmPluginEnter) pTmPluginEnter("enddraw");
  switch ( gapitype )
  {
#if defined(PLATFORM_HAS_D3D11)
    case BinkD3D11:
      enddraw_d3d11();
      break;
#endif
#if defined(PLATFORM_HAS_D3D9)
    case BinkD3D9:
      enddraw_d3d9();
      break;
#endif
#if defined(PLATFORM_HAS_GL)
    case BinkGL:
      enddraw_gl();
      break;
#endif
  }
  if(pTmPluginLeave) pTmPluginLeave();
  unlock_list();
  if(pTmPluginEnter) pTmPluginLeave();
}


PLUG_IN_FUNC_DEF( void ) BinkPluginProcessBinks( S32 ms_to_process )
{
  if ( !csuccessful_init ) // don't check gsuccess, because checked in process
    return;
  process_binks( ms_to_process, 0 );
}


static void draw_videos( int draw_to_textures, int draw_overlays )
{
  BINKPLUGIN * bp;  
  U64 cnt;
  S32 i;
  S32 do_clear;
  U32 draw_total;
  U32 draw_cnt = 0;
  S32 begun_drawing=0;

  if(pTmPluginEnter) pTmPluginEnter("draw_videos");

  lock_list();
  
  // find the smallest overlay numbered set to draw
  bp = all;
  cnt = 0;
  while( bp )
  {
    for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
    {
      lock_bink( bp );
      if ( bp->draws[i].frame )
      {
        if ( ( cnt == 0 ) || ( cnt > bp->draws[i].frame ) )
        {
          if ( ( ( bp->draws[i].to_texture ) && ( draw_to_textures ) ) || 
               ( ( bp->draws[i].to_texture == 0 ) && ( draw_overlays ) ) )
          {
            cnt = bp->draws[i].frame;
          }
        }
      }
      unlock_bink( bp );
    }
    bp = bp->next;
  }

  if ( cnt )
  {
    void * texture = 0;
    U32 rtw=0, rth=0;

    // if we are drawing overlays, draw them first, just
    //   jump past the search and just draw texture==0
    if ( draw_overlays )
      goto found;

    // now, draw everything of that numbered set
    for(;;)
    {
      // now find the first render texture (or screen) that match our cnt
      //   we need to draw all of the textures of each render_texture/screen at once for speed
      bp = all;
      while( bp )
      {
        lock_bink( bp );
        for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
        {
          if ( bp->draws[i].frame == cnt )
          {
            if ( ( ( bp->draws[i].to_texture ) && ( draw_to_textures ) ) || 
                 ( ( bp->draws[i].to_texture == 0 ) && ( draw_overlays ) ) )
            {
              texture = bp->draws[i].to_texture;
              rtw =  bp->draws[i].to_texture_width;
              rth =  bp->draws[i].to_texture_height;
              unlock_bink( bp );
              goto found;
            }
          }
        }
        unlock_bink( bp );
        bp = bp->next;
      }

      // if fell out of loop, nothing left to draw at this numbered set
      break;

     found:

      // Count the total number of draws we *will* be doing below
      {
        draw_total = 0;
        bp = all;
        while( bp )
        {
          lock_bink( bp );
          for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
            if ( bp->draws[i].frame == cnt && bp->draws[i].to_texture == texture )
              ++draw_total;
          unlock_bink( bp );
          bp = bp->next;
        }
      }

      // if we have a render target, get a rendertargetview  OR  if we have to start drawing...
      if ( ( texture ) || ( !begun_drawing ) )
      {
        // unlock the list while we create a render target/start rendering
        unlock_list();

        // start the drawing if we haven't done so yet
        if ( !begun_drawing )
        {
          // start drawing if we have anything that we're going to render
          switch ( gapitype )
          {
#if defined(PLATFORM_HAS_D3D12)
            case BinkD3D12:
              begindraw_d3d12();
              // if we have a set screen resource, and we're drawing overlays (texture==0), select the screen
              if ( ( screen_data.resource ) && ( texture == 0 ) )
                selectrendertargetview_d3d12( get_cached_rendertargetview( screen_data.resource, cnt, draw_cnt, allocaterendertargetview_d3d12, freerendertargetview_d3d12 ), screen_data.width, screen_data.height, 1, 0, screen_data.resource_state );
              break;
#endif
#if defined(PLATFORM_HAS_D3D11)
            case BinkD3D11:
              begindraw_d3d11();
              break;
#endif
#if defined(PLATFORM_HAS_D3D9)
            case BinkD3D9:
              begindraw_d3d9();
              break;
#endif
#if defined(__RADPS4__)
            case BinkPS4:
              begindraw_ps4();
              break;
#endif
#if defined(PLATFORM_HAS_GL)
            case BinkGL:
              begindraw_gl();
              break;
#endif
#if defined(PLATFORM_HAS_METAL)
            case BinkMetal:
              begindraw_metal();
              // if we have a set screen resource, and we're drawing overlays (texture==0), select the screen
              if ( ( screen_data.resource ) && ( texture == 0 ) )
                selectrendertargetview_metal( get_cached_rendertargetview( screen_data.resource, cnt, draw_cnt, allocaterendertargetview_metal, freerendertargetview_metal ), 0 );
              break;
#endif
          }
          begun_drawing = 1;
        }

        // now start the render target if necessary
        if ( texture )
        {
          do_clear = draw_total == 1 ? 0 : 1;
          ++draw_cnt; // keep track of how many textures have we used
          switch ( gapitype )
          {
#if defined(PLATFORM_HAS_D3D12)
            case BinkD3D12:
              selectrendertargetview_d3d12( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_d3d12, freerendertargetview_d3d12 ), rtw, rth, 0, do_clear, -1 );
              break;
#endif
#if defined(PLATFORM_HAS_D3D11)
            case BinkD3D11:
              selectrendertargetview_d3d11( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_d3d11, freerendertargetview_d3d11 ), rtw, rth, do_clear );
              break;
#endif
#if defined(PLATFORM_HAS_D3D9)
            case BinkD3D9:
              selectrendertargetview_d3d9( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_d3d9, freerendertargetview_d3d9 ), rtw, rth, do_clear );
              break;
#endif
#if defined(__RADPS4__)
            case BinkPS4:
              selectrendertargetview_ps4( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_ps4, freerendertargetview_ps4 ), rtw, rth, do_clear );
              break;
#endif
#if defined(PLATFORM_HAS_GL)
            case BinkGL:
              selectrendertargetview_gl( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_gl, freerendertargetview_gl ), rtw, rth, do_clear );
              break;
#endif
#if defined(PLATFORM_HAS_METAL)
            case BinkMetal:
              selectrendertargetview_metal( get_cached_rendertargetview( texture, cnt, draw_cnt, allocaterendertargetview_metal, freerendertargetview_metal ), do_clear );
              break;
#endif
          }
        }
    
        lock_list();
      }

      for(;;)
      {
        BINKPLUGIN * low = 0;  
        S32 low_depth = 0x7fffffff;
        S32 low_i = 0;

        // find the lowest depth at this texture
        bp = all;
        while( bp )
        {
          lock_bink( bp );

          for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
          {
            if ( bp->draws[i].frame == cnt )
            {
              if ( bp->draws[i].depth <= low_depth )
              {
                if ( bp->draws[i].to_texture == texture )
                {
                  // unlock the previous low, if there is one
                  if ( ( low ) && ( low != bp ) )
                    unlock_bink( low );

                  low_depth = bp->draws[i].depth;
                  low = bp;
                  low_i = i;
                }
              }
            }
          }

          // if this is not a low, unlock it
          if ( low != bp )
            unlock_bink( bp );

          bp = bp->next;
        }

        // if we didn't find anything, this rendertarget is done
        if ( low == 0 )
          break;

        // at this point low is locked and so is the list, unlock the list while we draw
        unlock_list();

        // draw if we have at least one frame done
        if ( low->finished_one )
        {
          F32 x0 = low->draws[low_i].x0;
          F32 y0 = low->draws[low_i].y0;
          F32 x1 = low->draws[low_i].x1;
          F32 y1 = low->draws[low_i].y1;

          // if drawing to a texture, are we in bottom_up land (usually for unity)
          if ( texture )
          {
            if ( gapitype == BinkGL )
            {
              if ( !bottom_up_render_targets )
                goto flip;
            }
            else
            {
              if ( bottom_up_render_targets ) 
              { 
               flip:
                y0 = 1.0f - y0; 
                y1 = 1.0f - y1; 
              }
            }
          }
          Set_Bink_draw_position( low->textures, x0,y0,x1,y1 );
          Set_Bink_alpha_settings( low->textures, low->alpha, draw_total == 1 && texture ? 2 : 0 );
          Draw_Bink_textures( low->textures, 0, gcontext );
        }

        // clear the draw trigger and target
        low->draws[ low_i ].frame = 0;
        low->draws[ low_i ].to_texture = 0;

        // unlock the bink we drew, relock the list and loop around to find the next depth
        unlock_bink( low );
        lock_list();
      }

      // we don't have to clear the rendertexture, either we select a new one, or we end the drawing below

    }
  }
  unlock_list();
  
  if ( begun_drawing )
  {
    switch ( gapitype )
    {
#if defined(PLATFORM_HAS_D3D12)
      case BinkD3D12:
        enddraw_d3d12();
        break;
#endif
#if defined(PLATFORM_HAS_D3D11)
      case BinkD3D11:
        enddraw_d3d11();
        break;
#endif
#if defined(PLATFORM_HAS_D3D9)
      case BinkD3D9:
        enddraw_d3d9();
        break;
#endif
#if defined(__RADPS4__)
      case BinkPS4:
        enddraw_ps4();
        break;
#endif
#if defined(PLATFORM_HAS_GL)
      case BinkGL:
        enddraw_gl();
        break;
#endif
#if defined(PLATFORM_HAS_METAL) 
      case BinkMetal:
        enddraw_metal();
        break;
#endif
    }
  }

  if(pTmPluginEnter) pTmPluginLeave();
}


PLUG_IN_FUNC_DEF( void ) BinkPluginDraw( S32 draw_overlays, S32 draw_to_render_textures )
{
  if ( ( !csuccessful_init ) || ( !gsuccessful_init ) )
    return;
  draw_videos( draw_overlays, draw_to_render_textures );
}


PLUG_IN_FUNC_DEF( void ) BinkPluginIOPause( S32 IO_on )
{
  BINKPLUGIN * bp;  
  
  if ( !plugin_init() )
    return;

  lock_list();
  bp = all;
  while( bp )
  {
    pBinkControlBackgroundIO( bp->b, ( IO_on ) ? BINKBGIOSUSPEND : BINKBGIORESUME );
    bp = bp->next;
  }
  unlock_list();
}

static U8 mask21[3*2] =
{
  0,0,0,
  0,1,0+(1<<4),
};

static U8 mask51[6*4] = 
{
  0,0,0,0,0,0,
  0,1,0+(1<<4),0+(1<<4)+0,0,1,
  0,1,2,0+(1<<4),0,1,
  0,1,0+(1<<4),0+(1<<4),2,3,
};

static U8 mask71[8*6] = 
{
  0,0,0,0,0,0,0,0,
  0,1,0+(1<<4),0+(1<<4),0,1,0,1,
  0,1,2,0+(1<<4),0,1,0,1,
  0,1,0+(1<<4),0+(1<<4),2,3,2,3,
  0,1,2,3,4,5,4,5,
  0,1,2,3,4,5,4,5,
};

static S32 get_limit_speaker_index( S32 orig_speaker_index, U8 * masks, S32 total_tracks, S32 limit_to )
{
  S32 speaker_index = 0;
  S32 cnt = 0;
  
  if ( ( total_tracks == 3 ) && ( limit_to == 4 ) )   // if 4 out, but handed 3 tracks, use stereo routing
    limit_to = 2;
  
  while( orig_speaker_index )
  {
    S32 si;
    si = orig_speaker_index&15;
    if ( total_tracks > limit_to )
      si = masks[ total_tracks * ( limit_to - 1 ) + si ];
    si<<=cnt;
    cnt+=4;
    orig_speaker_index >>= 4;
    speaker_index |= si;
  }

  return speaker_index;
}

static void set_speakers( BINKPLUGIN * bp, S32 speaker_index, S32 track_index, F32 * track_volumes )
{
  S32 vols[8];
  U32 ind[8];
  S32 cnt = 1;

  vols[0] = (S32)(track_volumes[track_index]*32768.0f);
  
  ind[0] = speaker_index & 15;
  for(;;)
  {
    speaker_index >>= 4;
    if ( speaker_index == 0 )
      break;
    ind[ cnt ] = speaker_index;
    vols[ cnt ] = vols[0];
    ++cnt;
  }

  pBinkSetSpeakerVolumes( bp->b, bp->tracks[track_index], ind, vols, cnt );
}

static void set_volumes( BINKPLUGIN * bp, F32 * trk_vols )
{
  S32 i, si;

  switch ( bp->num_tracks )
  {
    case 2: // one mono or stereo track, and one language track
      // handle the language track, and then fall down into the one track case
      si = get_limit_speaker_index( 2, mask21, 3, limit_speakers_to );
      set_speakers( bp, si, 1, trk_vols );
      // fall through
    case 1:  // one mono or stereo track
      si = get_limit_speaker_index( 0+(1<<4), mask21, 3, limit_speakers_to );
      set_speakers( bp, si, 0, trk_vols );
      break;
    
    case 7:  // 5+1 and a language track
      // handle the language track, and then fall down into the 5.1 case
      si = get_limit_speaker_index( 2, mask51, 6, limit_speakers_to );
      set_speakers( bp, si, 6, trk_vols );
      // fall through
    case 6: // 5+1
      for( i = 0 ; i < 6 ; i++ )
      {
        si = get_limit_speaker_index( i, mask51, 6, limit_speakers_to );
        set_speakers( bp, si, i, trk_vols );
      }
      break;
    case 9: // 7+1 and a language track
      // handle the language track, and then fall down into the 7.1 case
      si = get_limit_speaker_index( 2, mask71, 8, limit_speakers_to );
      set_speakers( bp, si, 8, trk_vols );
      // fall through
    case 8: // 7+1
      for( i = 0 ; i < 8 ; i++ )
      {
        si = get_limit_speaker_index( i, mask71, 8, limit_speakers_to );
        set_speakers( bp, si, i, trk_vols );
      }
      break;
  }
}

static int has_string( char const * big, char const * has )
{
  while( has[0] )
  {
    if ( has[0] != big[0] )
      return 0;
    ++has;
    ++big;
  }
  return 1;
}

static int snd_override( char const * name, U32 * snd_track_type )
{
  for(;;)
  {
    if ( name[0] == 0 )
      return 0;
    ++name;
    if ( name[-1] == '_' )
    {
      if ( has_string( name, "51." ) )
      {
        *snd_track_type = BinkSnd51;
        return 1;
      }
      if ( has_string( name, "51L." ) )
      {
        *snd_track_type = BinkSnd51LanguageOverride;
        return 1;
      }
      if ( has_string( name, "71." ) )
      {
        *snd_track_type = BinkSnd71;
        return 1;
      }
      if ( has_string( name, "71L." ) )
      {
        *snd_track_type = BinkSnd71LanguageOverride;
        return 1;
      }
    }
  }
}


PLUG_IN_FUNC_DEF( BINKPLUGIN * ) BinkPluginOpen( char const * name, U32 snd_track_type, S32 snd_track_start, U32 buffering_type, U64 file_byte_offset )
{
  BINKPLUGIN * bp;
  BINK_OPEN_OPTIONS o;
  S32 i;
  U32 flags = 0;

  if ( !plugin_init() )
  {
    return 0;
  }

  bp = ourmalloc( sizeof(*bp) );
  if ( bp == 0 )
  {
    BinkPluginSetError( "Out of memory." );
    return 0;
  }
  ourmemset( bp, 0, sizeof(*bp) );
  
  o.FileOffset = file_byte_offset;
 again: 
  switch ( snd_track_type )
  {
    case BinkSndNone:
      bp->tracks[0] = 0;
      bp->num_tracks = 0;
      break;
    case BinkSndSimple:
      if ( snd_override( name, &snd_track_type ) )
        goto again;
      bp->tracks[0] = snd_track_start;
      bp->num_tracks = 1;
      break;
    case BinkSndLanguageOverride:
      bp->tracks[0] = 0;
      bp->tracks[1] = snd_track_start;
      bp->num_tracks = 2;
      break;
    case BinkSnd51:
      for( i = 0 ; i < 6 ; i++ )
        bp->tracks[ i ] = snd_track_start + i;
      bp->num_tracks = 6;
      break;
    case BinkSnd51LanguageOverride:
      for( i = 0 ; i < 6 ; i++ )
        bp->tracks[ i ] = i;
      bp->tracks[ 6 ] = snd_track_start;
      bp->num_tracks = 7;
      break;
    case BinkSnd71:
      for( i = 0 ; i < 8 ; i++ )
        bp->tracks[ i ] = snd_track_start + i;
      bp->num_tracks = 8;
      break;
    case BinkSnd71LanguageOverride:
      for( i = 0 ; i < 8 ; i++ )
        bp->tracks[ i ] = i;
      bp->tracks[ 8 ] = snd_track_start;
      bp->num_tracks = 9;
      break;
    default:
      BinkPluginSetError( "Bad sound track type specified." );
      ourfree( bp );
      return 0;
  }
  o.TotTracks = bp->num_tracks;
  o.TrackNums = bp->tracks;

  switch ( buffering_type )
  {
    case BinkStream:
      break;
    case BinkPreloadAll:
      flags |= BINKPRELOADALL;
      break;
    case BinkStreamUntilResident:
      flags |= BINKIOSIZE;
      o.IOBufferSize = 0x7fffffffffffffffULL;
      break;
  }

  if ( gapitype == BinkD3D12 || gapitype == BinkPS4 )
    flags |= BINKUSETRIPLEBUFFERING;

  bp->b = pBinkOpenWithOptions( name, &o, BINKALPHA|BINKFILEOFFSET|BINKSNDTRACK|BINKDONTCLOSETHREADS|BINKNOFRAMEBUFFERS|flags );
  if ( bp->b == 0 )
  {
    BinkPluginSetError( pBinkGetError() );
    ourfree( bp );
    return 0;
  }

  // only warn if there are sound tracks at all in the bink
  if ( ( bp->b->NumTracks != (S32)bp->num_tracks ) && ( bp->b->NumTracks ) )
  {
    BinkPluginSetError( "Mismatched number of tracks opened." );
  }

  bp->state = ALLOC_TEXTURES;
  bp->loops = 1;
  bp->alpha = 1;

  {
    F32 vols[9]={1.0f,1.0f,1.0f,1.0f,1.0f,1.0f,1.0f,1.0f,1.0f};
    set_volumes( bp, vols );
  }

  alloc_bink_lock( bp );

  lock_list();

  // insert at end of list
  bp->next = 0;
  if ( all == 0 )
  {
    all = bp;
  }
  else
  {
    BINKPLUGIN * f;
    f = all;
    for(;;)
    {
      if ( f->next == 0 )
        break;
      f = f->next;
    }
#if defined(_DEBUG) && defined(_MSC_VER)
    if ( ( f==0 ) || ( f->next ) ) __debugbreak(); 
#endif
    f->next = bp;
  }
  unlock_list();

  return bp;
}


PLUG_IN_FUNC_DEF( void ) BinkPluginClose( BINKPLUGIN * bp )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    bp->closing = 1;
    unlock_bink( bp );
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginInfo( BINKPLUGIN * bp, BINKPLUGININFO * info )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      info->Width = bp->b->Width;
      info->Height = bp->b->Height;
      info->Frames = bp->b->Frames;
      info->FrameNum = bp->b->FrameNum;
      info->TotalFrames = bp->b->playedframes;
      info->FrameRate = bp->b->FrameRate;
      info->FrameRateDiv = bp->b->FrameRateDiv;
      info->LoopsRemaining = bp->loops;
      info->ReadError = bp->b->ReadError;
      info->TexturesError = ( bp->state == NO_TEXTURES ) ? 1 : 0 ;
      info->SndTrackType = bp->snd_track_type;
      info->NumTracksRequested = bp->num_tracks;
      info->NumTracksOpened = bp->b->NumTracks;
      info->BufferSize = bp->b->bio.CurBufSize;
      info->BufferUsed = bp->b->bio.CurBufUsed;
      info->SoundDropOuts = bp->b->soundskips;
      info->SkippedFrames = bp->skips;
      info->Alpha = bp->alpha;
      switch ( bp->state )
      {
        case AT_END:
          info->PlaybackState = 3;
          break;
        case GOTOING:
          info->PlaybackState = 2;
          break;
        case PAUSED:
          info->PlaybackState = 1;
          break;
        default:
          info->PlaybackState = 0;
          break;
      }
    }
    unlock_bink( bp );

    lock_list();
    if ( processbinks_cnt < PB_WINDOW )
      info->ProcessBinksFrameRate = 0;
    else
      info->ProcessBinksFrameRate = (((F32)PB_WINDOW)*1000.0f) / (F32)(processbinks_times[processbinks_pos]-processbinks_times[(processbinks_pos+PB_WINDOW+1)&(PB_WINDOW-1)]);
    unlock_list();
  }
}


PLUG_IN_FUNC_DEF( S32 ) BinkPluginScheduleToTexture( BINKPLUGIN * bp, F32 x0, F32 y0, F32 x1, F32 y1, S32 depth, void * render_target_texture, U32 render_target_width, U32 render_target_height )
{
  S32 i;
  
  if ( !plugin_init() )
    return 0;

  if ( ( bp ) && ( render_target_texture ) )
  {
    U64 frame;

    lock_list();

    frame = drawvideo_frame;

    lock_bink( bp );
  
    // find spot
    for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
      if ( bp->draws[i].frame == 0 )
        break;
    
    // did we find a spot?
    if ( i < MAX_OVERLAY_DRAWS )
    {
      bp->draws[i].x0 = x0;
      bp->draws[i].y0 = y0;
      bp->draws[i].x1 = x1;
      bp->draws[i].y1 = y1;
      bp->draws[i].depth = depth;
      bp->draws[i].to_texture = render_target_texture;
      bp->draws[i].to_texture_width = render_target_width;
      bp->draws[i].to_texture_height = render_target_height;
      bp->draws[i].frame = frame;
    }

    unlock_bink( bp );
    
    unlock_list();
  }
  return 1;
}


PLUG_IN_FUNC_DEF( S32 ) BinkPluginScheduleOverlay( BINKPLUGIN * bp, F32 x0, F32 y0, F32 x1, F32 y1, S32 depth )
{
  S32 i;
  
  if ( !plugin_init() )
    return 0;

  if ( bp )
  {
    U64 frame;

    lock_list();

    frame = drawvideo_frame;

    lock_bink( bp );
  
    // find spot
    for( i = 0 ; i < MAX_OVERLAY_DRAWS ; i++ )
      if ( bp->draws[i].frame == 0 )
        break;
    
    // did we find a spot?
    if ( i < MAX_OVERLAY_DRAWS )
    {
      bp->draws[i].x0 = x0;
      bp->draws[i].y0 = y0;
      bp->draws[i].x1 = x1;
      bp->draws[i].y1 = y1;
      bp->draws[i].depth = depth;
      bp->draws[i].frame = frame;
    }

    unlock_bink( bp );
    
    unlock_list();
  }
  return 1;
}

PLUG_IN_FUNC_DEF( void ) BinkPluginAllScheduled( void )
{
  if ( !plugin_init() )
    return;

  lock_list();
  if(drawvideo_frame == (U64)-1) {
    drawvideo_frame = 1;
  } else {
    ++drawvideo_frame;
  }
  unlock_list();
}

PLUG_IN_FUNC_DEF( void ) BinkPluginPause( BINKPLUGIN * bp, S32 pause_frame )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      bp->paused_frame = pause_frame;
      if ( pause_frame == 0 )
      {
        if ( bp->b->Paused ) 
          pBinkPause( bp->b, 0 ); // resume if unpausing
        if ( bp->state == PAUSED )
          bp->state = IDLE;
      }
    }
    unlock_bink( bp );
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginGoto( BINKPLUGIN * bp, S32 goto_frame, S32 ms_per_process )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      U32 keyframe;
      if ( (U32)goto_frame > bp->b->Frames )
        goto_frame = bp->b->Frames;
      keyframe = pBinkGetKeyFrame( bp->b, goto_frame, BINKGETKEYPREVIOUS );
      if ( ( (U32)goto_frame < bp->b->FrameNum ) || ( keyframe > (U32) ( bp->b->FrameNum + ( ( bp->b->FrameRate + ( ( bp->b->FrameRateDiv + 1 ) / 2 ) ) / bp->b->FrameRateDiv ) ) ) ) // farther than a second in the future? use goto 
      {
        bp->start_goto_frame = keyframe;  
      }

      pBinkSetSoundOnOff( bp->b, 0 );
      bp->goto_frame = goto_frame;
      bp->goto_time = ms_per_process;
      if ( bp->state == AT_END )
        bp->state = IDLE;
    }
    unlock_bink( bp );
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginVolume( BINKPLUGIN * bp, F32 vol )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      U32 i;
      for( i = 0 ; i < bp->num_tracks ; i++ )
        pBinkSetVolume( bp->b, bp->tracks[i], (S32) (32768.0*vol) );
    }
    unlock_bink( bp );
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginSpeakerVolumes( BINKPLUGIN * bp, F32 * vols, U32 count )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      if ( bp->num_tracks != count )
      {
        BinkPluginSetError( "Wrong track count used in SpeakerVolumes." );
      }
      else
      {
        set_volumes( bp, vols );
      }
    }
    unlock_bink( bp );
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginLoop( BINKPLUGIN * bp, U32 loops )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b )
    {
      bp->loops = loops;
      if ( bp->state == AT_END )
      {
        bp->state = DO_NEXT;
      }
    }
    unlock_bink( bp );
  }
}

PLUG_IN_FUNC_DEF( void ) BinkPluginSetHdrSettings( BINKPLUGIN * bp, U32 tonemap, F32 exposure, S32 output_nits )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    if ( bp->b && bp->textures )
    {
      Set_Bink_hdr_settings( bp->textures, tonemap, exposure, output_nits );
    }
    unlock_bink( bp );
  }
}

PLUG_IN_FUNC_DEF( void ) BinkPluginSetAlphaSettings( BINKPLUGIN * bp, F32 alpha )
{
  if ( !plugin_init() )
    return;

  if ( bp )
  {
    lock_bink( bp );
    bp->alpha = alpha;
    unlock_bink( bp );
  }
}

PLUG_IN_FUNC_DEF( void ) BinkPluginLimitSpeakers( U32 speaker_count )
{
  if ( (( speaker_count >= 1 ) && ( speaker_count <= 4 )) || ( speaker_count == 6 ) || ( speaker_count == 8 ) )
    limit_speakers_to = speaker_count;
}

#if defined(__RADWIN__)

#if !defined(__RADWINRT__) && !defined(__RADXBOXONE__)

PLUG_IN_FUNC_DEF( void ) BinkPluginWindowsUseDirectSound( void )
{
  if ( !plugin_init() )
    return;

  pBinkSetSoundSystem(pBinkOpenDirectSound,0);
}

#endif

#if defined(PLATFORM_HAS_D3D9)
PLUG_IN_FUNC_DEF( void ) BinkPluginWindowsD3D9BeginReset( void )
{
  if ( !plugin_init() )
    return;

  if ( ( gapitype == BinkD3D9 ) && ( currently_reset == 0 ) )
  {
    // empty out the render target view cache
    free_cached_rendertargetviews_before( ALL_RENDER_TARGET_VIEWS );

    // now release all the per-bink textures
    process_binks( -1, 1 );
    currently_reset = 1;
  }
}


PLUG_IN_FUNC_DEF( void ) BinkPluginWindowsD3D9EndReset( void )
{
  if ( !plugin_init() )
    return;

  if ( ( gapitype == BinkD3D9 ) && ( currently_reset == 1 ) )
  {
    currently_reset = 0;
    process_binks( 1, 0 );
  }
}
#endif
#endif

#if defined(PLATFORM_HAS_D3D12 ) || defined( PLATFORM_HAS_METAL )

PLUG_IN_FUNC_DEC( void ) BinkPluginSetPerFrameInfo(void * info)
{
  BINKSCREENGPUDATA * screen = (BINKSCREENGPUDATA*) info;
  screen_data = *screen;
}

#endif

#ifdef __RADPS4__

PLUG_IN_FUNC_DEC( void ) BinkPluginSetPerFrameInfo(void * context)
{
  gcontext = context;
}

#endif

void indirectBinkGetFrameBuffersInfo( HBINK bink, BINKFRAMEBUFFERS * fbset )
{
  pBinkGetFrameBuffersInfo( bink, fbset );
}


void indirectBinkRegisterFrameBuffers( HBINK bink, BINKFRAMEBUFFERS * fbset )
{
  pBinkRegisterFrameBuffers( bink, fbset );
}

S32  indirectBinkGetGPUDataBuffersInfo( HBINK bink, BINKGPUDATABUFFERS * gpu )
{
  return pBinkGetGPUDataBuffersInfo( bink, gpu );
}

void indirectBinkRegisterGPUDataBuffers( HBINK bink, BINKGPUDATABUFFERS * gpu )
{
  pBinkRegisterGPUDataBuffers( bink, gpu );
}

void * indirectBinkUtilMalloc( U64 bytes )
{
  return ourmalloc( bytes );
}

void indirectBinkUtilFree( void * ptr )
{
  ourfree( ptr );
}

void indirectBinkAllocateFrameBuffers( HBINK bp, BINKFRAMEBUFFERS * set, U32 minimum_alignment )
{
  pBinkAllocateFrameBuffers( bp, set, minimum_alignment );
}

#ifdef BUILDING_WITH_UNITY
#include "binkpluginunity.inl"
#endif
