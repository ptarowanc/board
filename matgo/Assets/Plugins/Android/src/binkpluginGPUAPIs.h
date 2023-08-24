#include "radtypes.h"

RADDEFSTART

typedef struct RENDERTARGETVIEW RENDERTARGETVIEW;

int setup_d3d12( void * device, S32 gpu_assisted, void ** context );
void shutdown_d3d12( void );
void * createtextures_d3d12( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_d3d12( void * render_texture_target );
void freerendertargetview_d3d12( RENDERTARGETVIEW * render_view );
void selectrendertargetview_d3d12( RENDERTARGETVIEW * render_view, S32 width, S32 height, S32 is_the_screen, S32 do_clear, S32 resource_state );
void clearrendertargetview_d3d12( void );
void begindraw_d3d12( void );
void enddraw_d3d12( void );

int setup_d3d11( void * device, S32 gpu_assisted, void ** context );
void shutdown_d3d11( void );
void * createtextures_d3d11( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_d3d11( void * render_texture_target );
void freerendertargetview_d3d11( RENDERTARGETVIEW * render_view );
void selectrendertargetview_d3d11( RENDERTARGETVIEW * render_view, S32 width, S32 height, S32 do_clear );
void clearrendertargetview_d3d11( void );
void begindraw_d3d11( void );
void enddraw_d3d11( void );

int setup_d3d9( void * device );
void shutdown_d3d9( void );
void * createtextures_d3d9( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_d3d9( void * render_texture_target );
void freerendertargetview_d3d9( RENDERTARGETVIEW * render_view );
void selectrendertargetview_d3d9( RENDERTARGETVIEW * render_view, S32 width, S32 height, S32 do_clear );
void clearrendertargetview_d3d9( void );
void begindraw_d3d9( void );
void enddraw_d3d9( void );

int setup_gl( S32 gpu_assisted );
void shutdown_gl( void );
void * createtextures_gl( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_gl( void * render_texture_target );
void freerendertargetview_gl( RENDERTARGETVIEW * render_view );
void selectrendertargetview_gl( RENDERTARGETVIEW * render_view, S32 width, S32 height, S32 do_clear );
void clearrendertargetview_gl( void );
void begindraw_gl( void );
void enddraw_gl( void );

int setup_metal( void *device, S32 gpu_assisted );
void shutdown_metal( void );
void * createtextures_metal( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_metal( void * render_texture_target );
void freerendertargetview_metal( RENDERTARGETVIEW * render_view );
void selectrendertargetview_metal( RENDERTARGETVIEW * render_view, S32 do_clear );
void clearrendertargetview_metal( void );
void begindraw_metal( void );
void enddraw_metal( void );

int setup_ps4( S32 gpu_assisted );
void shutdown_ps4( void );
void * createtextures_ps4( void * bink );
RENDERTARGETVIEW * allocaterendertargetview_ps4( void * render_texture_target );
void freerendertargetview_ps4( RENDERTARGETVIEW * render_view );
void selectrendertargetview_ps4( RENDERTARGETVIEW * render_view, S32 width, S32 height, S32 do_clear );
void clearrendertargetview_ps4( void );
void begindraw_ps4( void );
void enddraw_ps4( void );

RADDEFEND
