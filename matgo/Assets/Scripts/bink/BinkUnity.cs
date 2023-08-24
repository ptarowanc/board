using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using BinkPlugin;
using System.Diagnostics;
using System.Threading;

public class BinkUnity : MonoBehaviour
{
    [SerializeField]
    private string binkName = "";

    private enum DrawStyles
    {
        OverlayFillScreenWithAspectRatio,
        OverlayFillScreen,
        OverlayOriginalMovieSize,
        OverlaySpecificDestinationRectangle,
        RenderToTexture
    }

    [SerializeField]
    private DrawStyles drawStyle = DrawStyles.OverlayFillScreenWithAspectRatio;

    [SerializeField]
    private int loopCount = 0;

    [SerializeField]
    private int layerDepth = 0;

    [SerializeField]
    private Vector2 destinationUpperLeft = new Vector2(0, 0);
    [SerializeField]
    private Vector2 destinationLowerRight = new Vector2(1, 1);

    [SerializeField]
    private RenderTexture targetTexture = null;

    [SerializeField]
    private Bink.BufferingTypes ioBuffering = Bink.BufferingTypes.Stream;

    [SerializeField]
    private Bink.SoundTrackTypes soundOutput = Bink.SoundTrackTypes.SndSimple;

    [SerializeField]
    private int soundTrackOffset = 0;

    private IntPtr bink = IntPtr.Zero;
    private IntPtr our_cached_native_target_texture = IntPtr.Zero;
    private float binkw;
    private float binkh;

    static Coroutine cr = null;
    static int cr_num = 0;

    [SerializeField]
    bool m_bOnLoadStart = false;

    [SerializeField]
    bool m_bActive = false;

    [SerializeField]
    bool m_bStartEnable = false;

    bool m_bPause;
    MeshRenderer m_MeshRender;

    public static Vector2 get_viewport_size()
    {
#if (UNITY_EDITOR)
        if (Application.isEditor)
        {
            // nonsense to get the size of the game viewport
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
        else
#endif
        {
            return new Vector2(Screen.width, Screen.height);
        }
    }

    void Awake()
    {
        m_MeshRender = GetComponent<MeshRenderer> ();
        m_MeshRender.enabled = m_bStartEnable;
    }
    void Start()
    {
        if (m_bOnLoadStart)
        {
            StartCoroutine(CoStart());
        }
    }

    public void Load()
    {
        //if (m_bOnLoadStart)
        {
            //StartCoroutine(CoStart());     
            #if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR_WIN)
            Bink.UseDirectSound(); // turn on directsound, more generally compatible
            #endif

            // This turns on GPU-assisted decoding - it is much faster, but GPU drivers
            // are still really buggy with compute shaders. Only turn this on if you 
            // have a way to let the user force it back off, or if you control the 
            // hardware you are running on (in an embedded case, for example).
            // Bink.TurnOnGPUAssist();  // turn on gpu-assist

            if( binkName.Length != 0 )
            {
                //Tell the render queue to create the shaders and textures needed to render bink movies
                String movie_path;
                ulong fileOffset = 0;
                if (Application.platform == RuntimePlatform.Android)
                {
                    // jar:file:///data/app/com.rad-1/base.apk!/assets/alphatst.bik 
                    movie_path = Application.dataPath;
                    fileOffset = Bink.GetFileOffset(movie_path, binkName);
                }
                else
                {
                    movie_path = System.IO.Path.Combine (Application.streamingAssetsPath, binkName);
                }

                bink = Bink.Open( movie_path, soundOutput, soundTrackOffset, ioBuffering, fileOffset );
                if ( bink == IntPtr.Zero )
                {
                    string s = Bink.GetError();
                    UnityEngine.Debug.Log(s);
                }
                else
                {
                    Bink.Info i = new Bink.Info();
                    Bink.GetInfo( bink, ref i );
                    binkw = i.Width;
                    binkh = i.Height;
                    Bink.Loop( bink, (uint)loopCount );
                }
            }

            if ( cr == null )
                cr = StartCoroutine ("EndOfFrame");
            ++cr_num;
        }
    }
    public IEnumerator CoStart()
    {
#if (UNITY_SWITCH) || (UNITY_IOS) // NX requires a special call to regsiter the plugin with unity (only do this once pls)
      Bink.RegisterPlugin();
#endif

#if (UNITY_STANDALONE_WIN) || (UNITY_EDITOR_WIN)
        Bink.UseDirectSound(); // turn on directsound, more generally compatible
#endif

        // This turns on GPU-assisted decoding - it is much faster, but GPU drivers
        // are still really buggy with compute shaders. Only turn this on if you 
        // have a way to let the user force it back off, or if you control the 
        // hardware you are running on (in an embedded case, for example).
#if UNITY_PS4 // Always turn on gpu-assist for PS4
    Bink.TurnOnGPUAssist();  // turn on gpu-assist
#endif

        if (binkName.Length != 0)
        {
            //Tell the render queue to create the shaders and textures needed to render bink movies
            String movie_path;
            ulong fileOffset = 0;
            if (Application.platform == RuntimePlatform.Android)
            {
                // jar:file:///data/app/com.rad-1/base.apk!/assets/alphatst.bik 
                movie_path = Application.dataPath;
                fileOffset = Bink.GetFileOffset(movie_path, binkName);
            }
            else
            {
                movie_path = System.IO.Path.Combine(Application.streamingAssetsPath, binkName);
            }

            bink = Bink.Open(movie_path, soundOutput, soundTrackOffset, ioBuffering, fileOffset);
            if (bink == IntPtr.Zero)
            {
                string s = Bink.GetError();
                UnityEngine.Debug.Log(s);
            }
            else
            {
                Bink.Info i = new Bink.Info();
                Bink.GetInfo(bink, ref i);
                binkw = i.Width;
                binkh = i.Height;
                Bink.Loop(bink, (uint)loopCount);
            }
        }

        if (cr == null)
            cr = StartCoroutine("EndOfFrame");
        ++cr_num;

        yield return null;
    }

    void Update()
    {
        if (bink == IntPtr.Zero || m_bPause)
            return;

        if (IsEnd())
        {
            Pause(true);
            m_MeshRender.enabled = m_bActive;
        }

        float ulx, uly, lrx, lry;

        ulx = destinationUpperLeft.x;
        uly = destinationUpperLeft.y;
        lrx = destinationLowerRight.x;
        lry = destinationLowerRight.y;

        // figure out the x,y screencoords for all of the overlay types
        if (drawStyle == DrawStyles.OverlayFillScreenWithAspectRatio)
        {
            Vector2 s = get_viewport_size();
            lrx = binkw / s.x;
            lry = binkh / s.y;

            if (lrx > lry)
            {
                lry /= lrx;
                lrx = 1;
            }
            else
            {
                lrx /= lry;
                lry = 1;
            }
            ulx = (1.0f - lrx) / 2.0f;
            uly = (1.0f - lry) / 2.0f;
            lrx += ulx;
            lry += uly;
        }
        else if (drawStyle == DrawStyles.OverlayOriginalMovieSize)
        {
            Vector2 s = get_viewport_size();
            ulx = (s.x - binkw) / (2.0f * s.x);
            uly = (s.y - binkh) / (2.0f * s.y);
            lrx = binkw / s.x + ulx;
            lry = binkh / s.y + uly;
        }

        // draw it
        if (drawStyle == DrawStyles.RenderToTexture)
        {
            // make sure the render texture exists
            if (!targetTexture.IsCreated())
            {
                targetTexture.Create();
                our_cached_native_target_texture = IntPtr.Zero;
            }
            if (our_cached_native_target_texture == IntPtr.Zero)
            {
                our_cached_native_target_texture = targetTexture.GetNativeTexturePtr();
            }

            // now tell Bink to draw to it
            Bink.ScheduleToTexture(bink, ulx, uly, lrx, lry, layerDepth, our_cached_native_target_texture, (uint)targetTexture.width, (uint)targetTexture.height);
        }
        else
        {
            Bink.ScheduleOverlay(bink, ulx, uly, lrx, lry, layerDepth);
        }
    }

    private void OnDisable()
    {
        Bink.Close(bink);
        bink = IntPtr.Zero;
    }

    public enum UnityPluginCommands
    {
        Process = 0x18888880,
        ProcessNoWait = 0x18888881,
        ProcessCloses = 0x18888882,
        Draw = 0x18888885,
        DrawToTexturesOnly = 0x18888886,
        DrawOverlaysOnly = 0x18888887
    };

    private IEnumerator EndOfFrame()
    {
        while (true)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            // Tell Bink that we have scheduled everything that we are going to	 
            Bink.AllScheduled();

            Bink.SetOverlayRenderBuffer(Display.main.colorBuffer.GetNativeRenderBufferPtr(), Display.main.renderingWidth, Display.main.renderingHeight);

            // Issue a plugin event to process and draw binks
            {
                RenderTexture savedRT = RenderTexture.active;
                RenderTexture.active = null;
#if (UNITY_IOS)
        GL.IssuePluginEvent(Bink.GetUnityEventFunc(), (int)UnityPluginCommands.ProcessNoWait);
        GL.IssuePluginEvent(Bink.GetUnityEventFunc(), (int)UnityPluginCommands.Draw);
#else
                GL.IssuePluginEvent(Bink.GetUnityEventFunc(), (int)UnityPluginCommands.Draw);
                GL.IssuePluginEvent(Bink.GetUnityEventFunc(), (int)UnityPluginCommands.ProcessNoWait);
#endif
                RenderTexture.active = savedRT;
            }
        }
    }

    void OnDestroy()
    {
        --cr_num;
        if (cr_num == 0)
        {
            StopCoroutine("EndOfFrame");
            cr = null;

            // Issue a plugin event to close any binks
            GL.IssuePluginEvent(Bink.GetUnityEventFunc(), (int)UnityPluginCommands.ProcessCloses);
        }
    }

    public bool IsEndFrame()
    {
        if (m_bPause)
            return true;
        
        Bink.Info i = new Bink.Info();
        Bink.GetInfo( bink, ref i );

        if (i.Frames == i.FrameNum)
            return true;

        return false;
    }

	public int GetCurrentFrame()
	{
        Bink.Info i = new Bink.Info();
        Bink.GetInfo( bink, ref i );

		return (int)i.FrameNum;
	}

    public int GetMaxFrame()
    {
        Bink.Info i = new Bink.Info();
        Bink.GetInfo( bink, ref i );

        return (int)i.Frames;
    }

	public bool IsEnd()
	{
        if (m_bPause)
            return true;
        
		Bink.Info i = new Bink.Info();
		Bink.GetInfo( bink, ref i );

		if (i.PlaybackState == 3)
			return true;

		return false;
	}

	public void Goto(int iFrame, int ms_per_process)
	{        
        Bink.Info i = new Bink.Info();
        Bink.GetInfo( bink, ref i );

        Bink.Loop(bink, (uint)loopCount);
        Bink.Goto(bink, iFrame, ms_per_process);
        //Bink.ProcessBinks(ms_per_process);
        //		Bink.Info i = new Bink.Info();
//		Bink.GetInfo( bink, ref i );
//		i.FrameNum = 0;
//		i.PlaybackState = 0;

//        float ulx, uly, lrx, lry;
//
//        ulx = destinationUpperLeft.x;
//        uly = destinationUpperLeft.y;
//        lrx = destinationLowerRight.x;
//        lry = destinationLowerRight.y;
//
//        // figure out the x,y screencoords for all of the overlay types
//        if ( drawStyle == DrawStyles.OverlayFillScreenWithAspectRatio )
//        {
//            Vector2 s = get_viewport_size();
//            lrx = binkw / s.x;
//            lry = binkh / s.y;
//
//            if ( lrx > lry )
//            {
//                lry /= lrx;
//                lrx = 1;
//            }
//            else
//            {
//                lrx /= lry;
//                lry = 1;
//            }
//            ulx = ( 1.0f - lrx ) / 2.0f;
//            uly = ( 1.0f - lry ) / 2.0f;
//            lrx += ulx;
//            lry += uly;
//        }
//        else if ( drawStyle == DrawStyles.OverlayOriginalMovieSize )
//        {
//            Vector2 s = get_viewport_size();
//            ulx = ( s.x - binkw ) / ( 2.0f * s.x );
//            uly = ( s.y - binkh ) / ( 2.0f * s.y );
//            lrx = binkw / s.x + ulx;
//            lry = binkh / s.y + uly;
//        }
//
//        // draw it
//        if ( drawStyle == DrawStyles.RenderToTexture )
//        {
//            // make sure the render texture exists
//            if ( !targetTexture.IsCreated() )
//            {
//                targetTexture.Create();
//                our_cached_native_target_texture = IntPtr.Zero;
//            }
//            if ( our_cached_native_target_texture == IntPtr.Zero )
//            {
//                our_cached_native_target_texture = targetTexture.GetNativeTexturePtr();
//            }
//
//            // now tell Bink to draw to it
//            Bink.ScheduleToTexture( bink, ulx,uly, lrx,lry, layerDepth, our_cached_native_target_texture, (uint)targetTexture.width, (uint)targetTexture.height );
//        }
//        else
//        {
//            Bink.ScheduleOverlay( bink, ulx,uly, lrx,lry, layerDepth );
//        }
//
	}

	public void Close()
	{
		Bink.Close (bink);
		--cr_num;
		if ( cr_num == 0 )
		{
			StopCoroutine( "EndOfFrame" );
			cr = null;

			// Issue a plugin event to close any binks
			GL.IssuePluginEvent( Bink.GetUnityEventFunc(), (int)UnityPluginCommands.ProcessCloses );
		}
	}

	public void Loop(int iCount)
	{
		loopCount = iCount;
	}

	public void Pause(bool bFlag)
	{
        m_bPause = bFlag;
		if (bFlag)
			Bink.Pause (bink, -1);
		else
			Bink.Pause (bink, 0);
	}
    public void Pause(bool bFlag, int pauseFrame)
    {
        m_bPause = bFlag;
        if (bFlag)
            Bink.Pause(bink, pauseFrame);
        else
            Bink.Pause(bink, 0);
    }
    public bool IsPause()
    {
        return m_bPause;
    }

    IEnumerator Goto(int iFrame, int ms_per_process, bool bFlag)
    {
        Bink.Goto(bink, iFrame, ms_per_process);
        while (true)
        {
            Bink.Info i = new Bink.Info();
            Bink.GetInfo(bink, ref i);
            if (i.FrameNum == iFrame)
            {
                break;
            }
            if (m_bPause)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(0.034f);
        // if( bFlag )
        m_MeshRender.enabled = bFlag;
    }

    public void SetEnable(bool bFlag, int iFrame, int ms_per)
    {
        if (bink == IntPtr.Zero)
        {
            StartCoroutine(CoStart());
            if (bFlag)
                return;
        }
        //        if( !bFlag )
        //            m_MeshRender.enabled = bFlag;

        StartCoroutine(Goto(iFrame, 30, bFlag));
        //Goto(iFrame, 30);
        //Pause(!bFlag);
        if (!bFlag)
            StartCoroutine(CheckFrame(iFrame, bFlag));
        else
            Pause(!bFlag);
    }

    //IEnumerator CoPause(bool bFlag, int iFrame)
    //{
    //    float fTime = 0.0f;
    //    while (true)
    //    {
    //        fTime += Time.deltaTime;
    //        Bink.Info i = new Bink.Info();
    //        Bink.GetInfo( bink, ref i );
    //        if( i.FrameNum == iFrame )
    //        {
    //            break;
    //        }
    //        if (fTime >= 1.0f)
    //            break; 
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(0.05f);
    //    Pause(!bFlag);
    //}

    IEnumerator CheckFrame(int iFrame, bool bFlag)
    {
        //yield return null;
        Pause(!bFlag);
        //        while(true)
        //        {
        //            Bink.Info i = new Bink.Info();
        //            Bink.GetInfo( bink, ref i );
        //            if( i.FrameNum == iFrame )
        //            {
        //                break;
        //            }
        //            yield return null;
        //        }
        yield return new WaitForSeconds(0.034f);
        //Bink.ScheduleToTexture( bink, 0,0, 1,1, layerDepth, our_cached_native_target_texture, (uint)targetTexture.width, (uint)targetTexture.height );
        //Pause(!bFlag);
        //        yield return new WaitForSeconds(0.1f);
        //        Pause(!bFlag);
    }

    IEnumerator CoPause(bool bFlag, int iFrame)
    {
        float fTime = 0.0f;
        while (true)
        {
            fTime += Time.deltaTime;
            Bink.Info i = new Bink.Info();
            Bink.GetInfo(bink, ref i);
            if (i.FrameNum == iFrame)
            {
                break;
            }
            if (fTime >= 1.0f)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        Pause(!bFlag);
    }

    public void Goto2(int iFrame, int ms_per_process)
    {
        Bink.Info i = new Bink.Info();
        Bink.GetInfo(bink, ref i);

        Bink.Loop(bink, (uint)loopCount);
        Bink.Goto(bink, iFrame, ms_per_process);
    }

    public void SetEnable2(bool bFlag, int iFrame, int ms_per)
    {
        m_MeshRender.enabled = bFlag;
        Goto2(iFrame, 30);

        if (!bFlag)
        {
            //Pause(true);
            StartCoroutine(CoPause(false, iFrame));
        }
        else
        {
            Pause(!bFlag);
        }
        //StartCoroutine(CheckFrame(iFrame, bFlag));
    }
}
