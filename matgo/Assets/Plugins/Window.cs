using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class Window : MonoBehaviour
{
    public bool QuickAutoBorderless = true;
    public bool FullyAutoBorderless = false;
    public bool SilenceWarnings = false;
    public bool AutoFixAfterResizing = true;
    public bool AllowSizeResettingBeforeExit = false;
    [DllImport("USER32.DLL")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("USER32.DLL")]
    public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "ShowWindow")]
    private static extern bool ShowWindow(int hwnd, int nCmdShow);
    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    private static extern int GetActiveWindow();
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
    [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
    private static extern bool GetWindowRect(int hWnd, ref RECT NewRect);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindowEx(ref Flash flash);
    [DllImport("user32.dll")]
    static extern long GetWindowText(int hWnd, StringBuilder text, int count);
    [DllImport("user32.dll")]
    static extern bool SetWindowText(int hwnd, string lpString);


    public struct RECT
    {
        public int x;
        public int y;
        public int z;
        public int w;
    }
    public static bool DoneCalculating;
    public static Window Local;
    public static Vector4 Limitations = new Vector4(0, 4096, 0, 4096); // Min/Max Width/Height
    public static RECT Position;
    public static bool MoveWindow;
    private static bool Borders;
    private static Vector4 MoveOffSet;
    private static Vector3 OldOffSet;
    private static int Action;
    private static Vector2 CursorUpdate = Vector3.zero;
    private static Vector2 ClientPosition;
    private static float AspectRation;
    private static int WindowID;
    private bool Once;

    [StructLayout(LayoutKind.Sequential)]
    private struct Flash { public uint a; public int b; public int c; public int d; public int e; }
    private static void FlashWindow(int handle, int flags, int count, int timeout)
    {
        Flash flash = new Flash();
        flash.a = Convert.ToUInt32(Marshal.SizeOf(flash));
        flash.b = handle;
        flash.c = flags;
        flash.d = count;
        flash.e = timeout;
        FlashWindowEx(ref flash);
    }
    //--------------------------------------------------------------------------------------------------------- Start Autoborderless
    void Awake()
    {
        WindowID = (int)GetActiveWindow();
    }
    void Start()
    {
        Local = this;
        Once = true;
        if (WindowID == 0 && (int)GetActiveWindow() != 0)
        {
            WindowID = (int)GetActiveWindow();
        }
    }
    void Update()
    {
        if (WindowID == 0 && (int)GetActiveWindow() != 0)
        {
            WindowID = (int)GetActiveWindow();
        }
        if (!Local) { Local = this; }
        if (Once && (QuickAutoBorderless || FullyAutoBorderless) && DoneCalculating && Borders && !UnityEngine.Application.isEditor)
        {

            if (QuickAutoBorderless)
            {
                if (WindowID != 0)
                {
                    Once = false;
                }
            }
            else
            {
                Once = false;
            }

            if (QuickAutoBorderless)
            {
                SetWindowLong((IntPtr)WindowID, -16, 0x00080000);
                SetWindowPos(WindowID, -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                SetWindowLong((IntPtr)WindowID, -16, 0x00080000);
                SetWindowPos(WindowID, -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
            }
            else
            {
                System.Diagnostics.Process.Start(Regex.Replace(UnityEngine.Application.dataPath.ToString().Remove(UnityEngine.Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"), "-popupwindow");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        //--------------------------------------------------------------------------------------------------------- Move Window		
        if (MoveWindow)
        {
            if (Action == 1)
            {
                SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)MoveOffSet.z, (int)MoveOffSet.w, 32 | 64);
            }
            if (Action == 2)
            {
                if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) > Limitations.y)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)MoveOffSet.w, 32 | 64);
                }
                else
                {
                    if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) < Limitations.x)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)MoveOffSet.w, 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)MoveOffSet.w, 32 | 64);
                    }
                }
            }
            if (Action == 3)
            {
                if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) > Limitations.y)
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                        }
                    }
                }
                else
                {
                    if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) < Limitations.x)
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                            }
                        }
                    }
                }
            }
            if (Action == 4)
            {
                if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)MoveOffSet.z, (int)Limitations.w, 32 | 64);
                }
                else
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)MoveOffSet.z, (int)Limitations.z, 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)MoveOffSet.z, (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                    }
                }
            }
            if (Action == 5)
            {
                if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) > Limitations.y)
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                        }
                    }
                }
                else
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) < Limitations.x)
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w), 32 | 64);
                            }
                        }
                    }
                }
            }
            if (Action == 6)
            {
                if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) > Limitations.y)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.y, (int)MoveOffSet.w, 32 | 64);
                }
                else
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) < Limitations.x)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)Limitations.x, (int)MoveOffSet.w, 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)MoveOffSet.w, 32 | 64);
                    }
                }
            }
            if (Action == 7)
            {
                if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) > Limitations.y)
                {
                    if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)Limitations.y, (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                        }
                    }
                }
                else
                {
                    if ((int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z) < Limitations.x)
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)Limitations.x, (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z), (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                            }
                        }
                    }
                }
            }
            if (Action == 8)
            {
                if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)MoveOffSet.z, (int)Limitations.w, 32 | 64);
                }
                else
                {
                    if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)MoveOffSet.z, (int)Limitations.z, 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)MoveOffSet.z, (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                    }
                }
            }
            if (Action == 9)
            {
                if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) > Limitations.y)
                {
                    if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.y - MoveOffSet.z)), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)Limitations.y, (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                        }
                    }
                }
                else
                {
                    if ((int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)) < Limitations.x)
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.x + MoveOffSet.x - (Limitations.x - MoveOffSet.z)), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)Limitations.x, (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.w - MoveOffSet.w)) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(OldOffSet.y + MoveOffSet.y - (Limitations.z - MoveOffSet.w)) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)(System.Windows.Forms.Cursor.Position.X + MoveOffSet.x), (int)(System.Windows.Forms.Cursor.Position.Y + MoveOffSet.y) + 1, (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x)), (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)), 32 | 64);
                            }
                        }
                    }
                }
            }
            int ActionTemp;
            if (Action == 12)
            {
                ActionTemp = (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x));
                if (ActionTemp > Limitations.y)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - Limitations.y), (int)(MoveOffSet.y) + 1, (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                }
                else
                {
                    if (ActionTemp < Limitations.x)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - Limitations.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - ActionTemp), (int)(MoveOffSet.y) + 1, ActionTemp, (int)(ActionTemp / AspectRation), 32 | 64);
                    }
                }
            }
            if (Action == 13)
            {
                ActionTemp = (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x));
                if (ActionTemp / (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) >= AspectRation)
                {
                    if (ActionTemp > Limitations.y)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - Limitations.y), (int)(MoveOffSet.y) + 1, (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.x)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - Limitations.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - ActionTemp), (int)(MoveOffSet.y) + 1, ActionTemp, (int)(ActionTemp / AspectRation), 32 | 64);
                        }
                    }
                }
                if (ActionTemp / (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < AspectRation)
                {
                    ActionTemp = (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w);

                    if (ActionTemp > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - (Limitations.w * AspectRation)), (int)(MoveOffSet.y) + 1, (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - (Limitations.z * AspectRation)), (int)(MoveOffSet.y) + 1, (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(OldOffSet.z + MoveOffSet.z - (ActionTemp * AspectRation)), (int)(MoveOffSet.y) + 1, (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                        }
                    }
                }
            }
            if (Action == 14)
            {
                ActionTemp = (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w);
                if (ActionTemp > Limitations.w)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                }
                else
                {
                    if (ActionTemp < Limitations.z)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                    }
                }
            }
            if (Action == 15)
            {
                ActionTemp = (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z);
                if (ActionTemp / (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) >= AspectRation)
                {
                    if (ActionTemp > Limitations.y)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.x)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(ActionTemp), (int)(ActionTemp / AspectRation), 32 | 64);
                        }
                    }
                }
                if (ActionTemp / (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w) < AspectRation)
                {
                    ActionTemp = (int)(System.Windows.Forms.Cursor.Position.Y - OldOffSet.y + MoveOffSet.w);

                    if (ActionTemp > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                        }
                    }
                }
            }
            if (Action == 16)
            {
                ActionTemp = (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z);
                if (ActionTemp > Limitations.y)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                }
                else
                {
                    if (ActionTemp < Limitations.x)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(MoveOffSet.y) + 1, ActionTemp, (int)(ActionTemp / AspectRation), 32 | 64);
                    }
                }
            }
            if (Action == 17)
            {
                ActionTemp = (int)(System.Windows.Forms.Cursor.Position.X - OldOffSet.x + MoveOffSet.z);
                if (ActionTemp / (MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) >= AspectRation)
                {
                    if (ActionTemp > Limitations.y)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - (Limitations.y / AspectRation)), (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.x)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - (Limitations.x / AspectRation)), (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - (ActionTemp / AspectRation)), ActionTemp, (int)(ActionTemp / AspectRation), 32 | 64);
                        }
                    }
                }
                if (ActionTemp / (MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < AspectRation)
                {
                    ActionTemp = (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y));

                    if (ActionTemp > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - Limitations.w), (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - Limitations.z), (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(OldOffSet.z + MoveOffSet.w - ActionTemp), (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                        }
                    }
                }
            }
            if (Action == 18)
            {
                ActionTemp = (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y));
                if (ActionTemp > Limitations.w)
                {
                    SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(Position.y + MoveOffSet.w - Limitations.w), (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                }
                else
                {
                    if (ActionTemp < Limitations.z)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(Position.y + MoveOffSet.w - Limitations.z), (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x), (int)(Position.y + MoveOffSet.w - ActionTemp), (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                    }
                }
            }
            if (Action == 19)
            {
                ActionTemp = (int)(MoveOffSet.z - (System.Windows.Forms.Cursor.Position.X - OldOffSet.x));
                if (ActionTemp / (MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) >= AspectRation)
                {
                    if (ActionTemp > Limitations.y)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - Limitations.y), (int)(OldOffSet.z + MoveOffSet.w - (Limitations.y / AspectRation)), (int)(Limitations.y), (int)(Limitations.y / AspectRation), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.x)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - Limitations.x), (int)(OldOffSet.z + MoveOffSet.w - (Limitations.x / AspectRation)), (int)(Limitations.x), (int)(Limitations.x / AspectRation), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - ActionTemp), (int)(OldOffSet.z + MoveOffSet.w - (ActionTemp / AspectRation)), ActionTemp, (int)(ActionTemp / AspectRation), 32 | 64);
                        }
                    }
                }
                if (ActionTemp / (MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y)) < AspectRation)
                {
                    ActionTemp = (int)(MoveOffSet.w - (System.Windows.Forms.Cursor.Position.Y - OldOffSet.y));

                    if (ActionTemp > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - (Limitations.w * AspectRation)), (int)(OldOffSet.z + MoveOffSet.w - Limitations.w), (int)(Limitations.w * AspectRation), (int)(Limitations.w), 32 | 64);
                    }
                    else
                    {
                        if (ActionTemp < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - (Limitations.z * AspectRation)), (int)(OldOffSet.z + MoveOffSet.w - Limitations.z), (int)(Limitations.z * AspectRation), (int)(Limitations.z), 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)(MoveOffSet.x + MoveOffSet.z - (ActionTemp * AspectRation)), (int)(OldOffSet.z + MoveOffSet.w - ActionTemp), (int)(ActionTemp * AspectRation), ActionTemp, 32 | 64);
                        }
                    }
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------- Calculate Position
        GetWindowRect((int)GetActiveWindow(), ref Position);
        Vector2 CursorDelta = new Vector2(Input.mousePosition.x - CursorUpdate.x, Input.mousePosition.y - CursorUpdate.y);
        if (CursorDelta == Vector2.zero)
        {
            ClientPosition = new Vector2(System.Windows.Forms.Cursor.Position.X - Input.mousePosition.x - 1, System.Windows.Forms.Cursor.Position.Y - (UnityEngine.Screen.height - Input.mousePosition.y));
        }
        CursorUpdate = Input.mousePosition;
        //--------------------------------------------------------------------------------------------------------- Calculate Border
        if (UnityEngine.Screen.height != (int)(Position.w - Position.y))
        {
            Borders = true;
        }
        else
        {
            Borders = false;
        }
        //--------------------------------------------------------------------------------------------------------- Done Calculating (1st frame)
        DoneCalculating = true;
    }
    //--------------------------------------------------------------------------------------------------------- IsDoneLoading
    public static bool IsDoneLoading()
    {
        return DoneCalculating;
    }
    //--------------------------------------------------------------------------------------------------------- IsBorderless
    public static bool IsBorderless()
    {
        return !Borders;
    }
    //--------------------------------------------------------------------------------------------------------- Border
    public static void Border()
    {
        Border(!Borders);
    }
    public static void Border(bool Active)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (Active)
            {
                System.Diagnostics.Process.Start(Regex.Replace(UnityEngine.Application.dataPath.ToString().Remove(UnityEngine.Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"));
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                System.Diagnostics.Process.Start(Regex.Replace(UnityEngine.Application.dataPath.ToString().Remove(UnityEngine.Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"), "-popupwindow");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("Border function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- SetRect
    public static void SetRect(Rect Source)
    {
        SetRect((int)Source.x, (int)Source.y, (int)Source.width, (int)Source.height);
    }
    public static void SetRect(int LeftCorner, int TopCorner, int width, int height)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                if (width > Limitations.y)
                {
                    if (height > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if (height < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.y, height, 32 | 64);
                        }
                    }
                }
                else
                {
                    if (width < Limitations.x)
                    {
                        if (height > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if (height < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, (int)Limitations.x, height, 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if (height > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, width, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if (height < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, width, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, LeftCorner, TopCorner + 1, width, height, 32 | 64);
                            }
                        }
                    }
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("SetRect function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetRect function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- SetPosition
    public static void SetPosition(int LeftCorner, int TopCorner)
    {
        SetPosition(new Vector2(LeftCorner, TopCorner));
    }
    public static void SetPosition(Vector2 Source)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                int Width = UnityEngine.Screen.width;
                int Height = UnityEngine.Screen.height;
                SetWindowPos((int)GetActiveWindow(), 0, (int)Source.x, (int)Source.y + 1, Width, Height, 32 | 64);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("SetPosition function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetPosition function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- SetSize
    public static void SetSize(int Width, int Height)
    {
        SetSize(new Vector2(Width, Height));
    }
    public static void SetSize(Vector2 Source)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                if ((int)Source.x > Limitations.y)
                {
                    if ((int)Source.y > Limitations.w)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                    else
                    {
                        if ((int)Source.y < Limitations.z)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.y, (int)Limitations.z, 32 | 64);
                        }
                        else
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.y, (int)Source.y, 32 | 64);
                        }
                    }
                }
                else
                {
                    if ((int)Source.x < Limitations.x)
                    {
                        if ((int)Source.y > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)Source.y < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Limitations.x, (int)Source.y, 32 | 64);
                            }
                        }
                    }
                    else
                    {
                        if ((int)Source.y > Limitations.w)
                        {
                            SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Source.x, (int)Limitations.w, 32 | 64);
                        }
                        else
                        {
                            if ((int)Source.y < Limitations.z)
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Source.x, (int)Limitations.z, 32 | 64);
                            }
                            else
                            {
                                SetWindowPos((int)GetActiveWindow(), 0, (int)Position.x, (int)Position.y, (int)Source.x, (int)Source.y, 32 | 64);
                            }
                        }
                    }
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("SetSize function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetSize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- GetBorderRect
    public static Rect GetBorderRect()
    {
        return new Rect(Position.x, Position.y, Position.z - Position.x, Position.w - Position.y);
    }
    //--------------------------------------------------------------------------------------------------------- GetBorderPosition
    public static Vector2 GetBorderPosition()
    {
        return new Vector2(Position.x, Position.y);
    }
    //--------------------------------------------------------------------------------------------------------- GetBorderSize
    public static Vector2 GetBorderSize()
    {
        if (Borders)
        {
            return new Vector2((Position.z - Position.x - UnityEngine.Screen.width) / 2, (Position.w - Position.y - UnityEngine.Screen.height) / 2); ;
        }
        else
        {
            return Vector2.zero;
        }
    }
    //--------------------------------------------------------------------------------------------------------- GetRect
    public static Rect GetRect()
    {
        return new Rect(Position.x - GetBorderSize().x, Position.y - GetBorderSize().y, UnityEngine.Screen.width, UnityEngine.Screen.height);
    }
    //--------------------------------------------------------------------------------------------------------- GetPosition
    public static Vector2 GetPosition()
    {
        if (Borders)
        {
            return ClientPosition;
        }
        else
        {
            return new Vector2(Position.x, Position.y);
        }
    }
    //--------------------------------------------------------------------------------------------------------- GetSize
    public static Vector2 GetSize()
    {
        return new Vector2(UnityEngine.Screen.width, UnityEngine.Screen.height);
    }
    //--------------------------------------------------------------------------------------------------------- Grab
    public static void GrabStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                MoveWindow = true;
                Action = 1;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a Grab function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("GrabStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void GrabEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && Action == 1)
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a Grab function while you haven't started a Grab function or another Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("GrabEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize Left
    public static void ResizeLeftStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                OldOffSet.z = Position.x;
                MoveWindow = true;
                Action = 12;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeLeftStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 2;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeLeftEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 2 || Action == 12))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeLeft function while you haven't started a ResizeLeft function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize DownLeft
    public static void ResizeDownLeftStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                OldOffSet.z = Position.x;
                MoveWindow = true;
                Action = 13;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDownLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownLeftStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 3;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDownLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownLeftEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 3 || Action == 13))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeDownLeft function while you haven't started a ResizeDownLeft function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize Down
    public static void ResizeDownStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 14;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDown function while another Grab or Resize function hasnt ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 4;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDown function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 4 || Action == 14))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeDown function while you haven't started a ResizeDown function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize DownRight
    public static void ResizeDownRightStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 15;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDownRight function while another Grab or Resize function hasnt ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownRightStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 5;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeDownRight function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeDownRightEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 5 || Action == 15))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeDownRight function while you haven't started a ResizeDownRight function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeDownRightEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize Right
    public static void ResizeRightStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 16;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeRight function while another Grab or Resize function hasnt ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeRightStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Position.y - 1, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 6;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeRight function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeRightEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 6 || Action == 16))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeRight function while you haven't started a ResizeRight function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize RightTop
    public static void ResizeRightTopStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                OldOffSet.z = Position.y - 1;
                MoveWindow = true;
                Action = 17;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeRightTop function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeRightTopStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 7;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeRightTop function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeRightTopEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 7 || Action == 17))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeRightTop function while you haven't started a ResizeRightTop function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeRightTopEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize Top
    public static void ResizeTopStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                OldOffSet.z = Position.y;
                MoveWindow = true;
                Action = 18;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeTop function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeTopStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 8;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeTop function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeTopEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 8 || Action == 18))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeTop function while you haven't started a ResizeTop function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTop function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Resize TopLeft
    public static void ResizeTopLeftStart(float aspectRatio)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(Position.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                OldOffSet.z = Position.y;
                MoveWindow = true;
                Action = 19;
                AspectRation = aspectRatio;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeTopLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTopLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeTopLeftStart()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow && Action == 0)
            {
                MoveOffSet = new Vector4(-Input.mousePosition.x, Input.mousePosition.y - UnityEngine.Screen.height, UnityEngine.Screen.width, UnityEngine.Screen.height);
                OldOffSet.x = System.Windows.Forms.Cursor.Position.X;
                OldOffSet.y = System.Windows.Forms.Cursor.Position.Y;
                MoveWindow = true;
                Action = 9;
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant start a ResizeTopLeft function while another Grab or Resize function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTopLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void ResizeTopLeftEnd()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (MoveWindow && (Action == 9 || Action == 19))
            {
                MoveOffSet = new Vector4(0, 0, 0, 0);
                MoveWindow = false;
                Action = 0;
                if (Local.AutoFixAfterResizing)
                {
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                    SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
                    SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("You cant end a ResizeTopLeft function while you haven't started a ResizeTopLeft function or a Grab function hasn't ended.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ResizeTopLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Set Limitations
    public static void SetMinWidth(int Minimum)
    {
        if (!UnityEngine.Application.isEditor)
        {
            Limitations.x = Minimum;
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetMinWidth function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void SetMaxWidth(int Maximum)
    {
        if (!UnityEngine.Application.isEditor)
        {
            Limitations.y = Maximum;
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetMaxWidth function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void SetMinHeight(int Minimum)
    {
        if (!UnityEngine.Application.isEditor)
        {
            Limitations.z = Minimum;
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetMinHeight function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    public static void SetMaxHeight(int Maximum)
    {
        if (!UnityEngine.Application.isEditor)
        {
            Limitations.w = Maximum;
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetMaxHeight function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Get Limitations
    public static int GetMinWidth()
    {
        return (int)Limitations.x;
    }
    public static int GetMaxWidth()
    {
        return (int)Limitations.y;
    }
    public static int GetMinHeight()
    {
        return (int)Limitations.z;
    }
    public static int GetMaxHeight()
    {
        return (int)Limitations.w;
    }
    //--------------------------------------------------------------------------------------------------------- QuickDisableBorders
    public static void QuickDisableBorders()
    {
        if (!UnityEngine.Application.isEditor)
        {
            SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
            SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
            SetWindowLong((IntPtr)GetActiveWindow(), -16, 0x00080000);
            SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("QuickDisableBorders function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- QuickEnableBorders
    public static void QuickEnableBorders()
    {
        if (!UnityEngine.Application.isEditor)
        {
            SetWindowLong((IntPtr)GetActiveWindow(), -16, 349110272);
            SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
            SetWindowLong((IntPtr)GetActiveWindow(), -16, 349110272);
            SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
            //SetWindowPos((int)GetActiveWindow(), -2, (int)GetRect().x, (int)GetRect().y, (int)GetRect().width, (int)GetRect().height, 0x0040);
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("QuickEnableBorders function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Minimize
    public static void Minimize()
    {
        Minimize(true);
    }
    public static void Minimize(bool TurnAutoBorderlessOff)
    {
        if (!UnityEngine.Application.isEditor)
        {
            Local.QuickAutoBorderless = !TurnAutoBorderlessOff;
            if (!MoveWindow)
            {
                ShowWindow((int)GetActiveWindow(), 2);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("Minimize function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("Minimize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Fullscreen
    public static void Fullscreen()
    {
        Fullscreen(UnityEngine.Screen.width, UnityEngine.Screen.height);
    }
    public static void Fullscreen(Vector2 Quality)
    {
        Fullscreen((int)Quality.x, (int)Quality.y);
    }
    public static void Fullscreen(int Width, int Height)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                UnityEngine.Screen.SetResolution(Width, Height, !UnityEngine.Screen.fullScreen);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("Fullscreen function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("Fullscreen function doesnt work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Maximize
    public static void UnMaximize()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                ShowWindow((int)GetActiveWindow(), 1);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("UnMaximize function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("UnMaximize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Maximize
    public static void Maximize()
    {
        Maximize(true);
    }
    public static void Maximize(bool IgnoreLimitations)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                if (!IsBorderless())
                {
                    ShowWindow((int)GetActiveWindow(), 3);
                }
                else
                {
                    if (IgnoreLimitations)
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, 0, 0, UnityEngine.Screen.currentResolution.width, UnityEngine.Screen.currentResolution.height, 32 | 64);
                    }
                    else
                    {
                        SetWindowPos((int)GetActiveWindow(), 0, UnityEngine.Screen.currentResolution.width / 2 - (int)(Limitations.y / 2), UnityEngine.Screen.currentResolution.height / 2 - (int)(Limitations.w / 2) + 1, (int)Limitations.y, (int)Limitations.w, 32 | 64);
                    }
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("Maximize function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("Maximize function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- GetMaximized
    public static bool GetMaximized()
    {
        return (UnityEngine.Screen.width == UnityEngine.Screen.currentResolution.width && UnityEngine.Screen.height == UnityEngine.Screen.currentResolution.height);
    }
    //--------------------------------------------------------------------------------------------------------- Exit / Close / Shut / Quit
    public static void Exit()
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                UnityEngine.Application.Quit();
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("Exit function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("Exit function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Force Exit / Close / Shut / Quit
    public static void ForceExit()
    {
        if (!UnityEngine.Application.isEditor)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("ForceExit function is not recommended to be called within the editor because it can cause data loss.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- ResetSize before exit
    //Win2000 System.Environment.OSVersion.Version.Major >= 5
    void OnApplicationQuit()
    {
        if (AllowSizeResettingBeforeExit)
        {
            PlayerPrefs.SetInt("Screenmanager Resolution Height", 90);
            PlayerPrefs.SetInt("Screenmanager Resolution Width", 116);
        }
    }
    //--------------------------------------------------------------------------------------------------------- Flash End
    public static void FlashEnd()
    {
        FlashEnd((int)GetActiveWindow());
    }
    public static void FlashEnd(int WindowId)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                FlashWindow(WindowId, 0, 0, 0);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("FlashEnd function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("FlashEnd function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Flash Pause
    public static void FlashPause()
    {
        FlashPause((int)GetActiveWindow());
    }
    public static void FlashPause(int WindowId)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                FlashWindow(WindowId, 0, int.MaxValue, 0);
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("FlashPause function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("FlashPause function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Flash Start
    public static void FlashStart()
    {
        FlashStart((int)GetActiveWindow(), 0.0f, int.MaxValue, "");
    }
    public static void FlashStart(float MilisecSpeed)
    {
        FlashStart((int)GetActiveWindow(), MilisecSpeed, int.MaxValue, "");
    }
    public static void FlashStart(int FlashTimes)
    {
        FlashStart((int)GetActiveWindow(), 0.0f, FlashTimes, "");
    }
    public static void FlashStart(float MilisecSpeed, int FlashTimes)
    {
        FlashStart((int)GetActiveWindow(), MilisecSpeed, FlashTimes, "");
    }
    public static void FlashStart(int WindowId, float MilisecSpeed)
    {
        FlashStart(WindowId, MilisecSpeed, int.MaxValue, "");
    }
    public static void FlashStart(int WindowId, float MilisecSpeed, int FlashTimes)
    {
        FlashStart(WindowId, MilisecSpeed, FlashTimes, "");
    }
    public static void FlashStart(string Mode)
    {
        FlashStart((int)GetActiveWindow(), 0.0f, int.MaxValue, Mode);
    }
    public static void FlashStart(float MilisecSpeed, string Mode)
    {
        FlashStart((int)GetActiveWindow(), MilisecSpeed, int.MaxValue, Mode);
    }
    public static void FlashStart(int FlashTimes, string Mode)
    {
        FlashStart((int)GetActiveWindow(), 0.0f, FlashTimes, Mode);
    }
    public static void FlashStart(float MilisecSpeed, int FlashTimes, string Mode)
    {
        FlashStart((int)GetActiveWindow(), MilisecSpeed, FlashTimes, Mode);
    }
    public static void FlashStart(int WindowId, float MilisecSpeed, string Mode)
    {
        FlashStart(WindowId, MilisecSpeed, int.MaxValue, Mode);
    }
    public static void FlashStart(int WindowId, float MilisecSpeed, int FlashTimes, string Mode)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                if (Mode == "Taskbar")
                {
                    FlashWindow(WindowId, 2, FlashTimes, (int)MilisecSpeed);
                }
                else
                {
                    if (Mode == "Caption")
                    {
                        FlashWindow(WindowId, 1, FlashTimes, (int)MilisecSpeed);
                    }
                    else
                    {
                        FlashWindow(WindowId, 3, FlashTimes, (int)MilisecSpeed);
                    }
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("FlashStart function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("FlashStart function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Flash Until Focus
    public static void FlashUntilFocus()
    {
        FlashUntilFocus((int)GetActiveWindow(), 0.0f, int.MaxValue, "");
    }
    public static void FlashUntilFocus(float MilisecSpeed)
    {
        FlashUntilFocus((int)GetActiveWindow(), MilisecSpeed, int.MaxValue, "");
    }
    public static void FlashUntilFocus(int FlashTimes)
    {
        FlashUntilFocus((int)GetActiveWindow(), 0.0f, FlashTimes, "");
    }
    public static void FlashUntilFocus(float MilisecSpeed, int FlashTimes)
    {
        FlashUntilFocus((int)GetActiveWindow(), MilisecSpeed, FlashTimes, "");
    }
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed)
    {
        FlashUntilFocus(WindowId, MilisecSpeed, int.MaxValue, "");
    }
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, int FlashTimes)
    {
        FlashUntilFocus(WindowId, MilisecSpeed, FlashTimes, "");
    }
    public static void FlashUntilFocus(string Mode)
    {
        FlashUntilFocus((int)GetActiveWindow(), 0.0f, int.MaxValue, Mode);
    }
    public static void FlashUntilFocus(float MilisecSpeed, string Mode)
    {
        FlashUntilFocus((int)GetActiveWindow(), MilisecSpeed, int.MaxValue, Mode);
    }
    public static void FlashUntilFocus(int FlashTimes, string Mode)
    {
        FlashUntilFocus((int)GetActiveWindow(), 0.0f, FlashTimes, Mode);
    }
    public static void FlashUntilFocus(float MilisecSpeed, int FlashTimes, string Mode)
    {
        FlashUntilFocus((int)GetActiveWindow(), MilisecSpeed, FlashTimes, Mode);
    }
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, string Mode)
    {
        FlashUntilFocus(WindowId, MilisecSpeed, int.MaxValue, Mode);
    }
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, int FlashTimes, string Mode)
    {
        if (!UnityEngine.Application.isEditor)
        {
            if (!MoveWindow)
            {
                if (Mode == "Taskbar")
                {
                    FlashWindow(WindowId, 2 | 13, FlashTimes, (int)MilisecSpeed);
                }
                else
                {
                    if (Mode == "Caption")
                    {
                        FlashWindow(WindowId, 1 | 13, FlashTimes, (int)MilisecSpeed);
                    }
                    else
                    {
                        FlashWindow(WindowId, 3 | 13, FlashTimes, (int)MilisecSpeed);
                    }
                }
            }
            else
            {
                if (!Local.SilenceWarnings) Debug.LogWarning("FlashUntilFocus function cant be called while GrabStart has been called and GrabEnd hasn't.");
            }
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("FlashUntilFocus function is not designed to work in the editor.");
        }
    }
    //--------------------------------------------------------------------------------------------------------- Get Window Title
    public static string GetTitle()
    {
        StringBuilder Buffer = new StringBuilder(256);
        if (!UnityEngine.Application.isEditor)
        {
            if (GetWindowText((int)GetActiveWindow(), Buffer, 256) > 0)
            {
                return Buffer.ToString();
            }
            if (!Local.SilenceWarnings) Debug.LogWarning("GetTitle function should not be called in the editor.");
        }
        return null;
    }
    //--------------------------------------------------------------------------------------------------------- Set Window Title
    public static void SetTitle(string newTitle)
    {
        if (!UnityEngine.Application.isEditor)
        {
            SetWindowText((int)GetActiveWindow(), newTitle);
        }
        else
        {
            if (!Local.SilenceWarnings) Debug.LogWarning("SetTitle function should not be called in the editor.");
        }
    }
}