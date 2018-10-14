using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace DBMS.Utils
{
    public static class Win32API
    {
        private const int GWL_STYLE = -16;
        private const long WS_VISIBLE = 0x10000000L;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;
        private static IntPtr unityWindow = IntPtr.Zero;
        private delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class OpenFileName
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;
            public String filter = null;
            public String customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public String file = null;
            public int maxFile = 0;
            public String fileTitle = null;
            public int maxFileTitle = 0;
            public String initialDir = null;
            public String title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public String defExt = null;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public String templateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("kernel32.dll")]
        private static extern void SetLastError(uint dwErrCode);
        private static IntPtr GetUnityHandle()
        {
            if (unityWindow != IntPtr.Zero)
            {
                return unityWindow;
            }
            IntPtr ptrWnd = IntPtr.Zero;
            uint pid = (uint)Process.GetCurrentProcess().Id;  // 当前进程 ID  
            bool bResult = EnumWindows(new WNDENUMPROC(delegate (IntPtr hwnd, uint lParam)
            {
                uint id = 0;
                if (GetParent(hwnd) == IntPtr.Zero)
                {
                    GetWindowThreadProcessId(hwnd, ref id);
                    if (id == lParam)    // 找到进程对应的主窗口句柄  
                    {
                        ptrWnd = hwnd;   // 把句柄缓存起来  
                        SetLastError(0);    // 设置无错误  
                        return false;   // 返回 false 以终止枚举窗口  
                    }
                }
                return true;
            }), pid);
            unityWindow = (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
            return unityWindow;
        }
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern long SetWindowLong(IntPtr hwnd, int nlndex, long dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
        public static void SetWindowStyle()
        {
#if !UNITY_EDITOR
                SetWindowLong(GetUnityHandle(), GWL_STYLE, WS_VISIBLE);
#endif
        }
        public static void PanelTitleMouseDown()
        {
            ReleaseCapture();
            SendMessage(GetUnityHandle(), WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        public static void ShowMinWindow()
        {
            ShowWindow(GetUnityHandle(), 2);
        }
        public static string GetOpenFileName()
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            //ofn.filter = "All Files\0*.*\0\0";
            ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            string path = Application.streamingAssetsPath;
            path = path.Replace('/', '\\');
            //默认路径  
            ofn.initialDir = path;
            //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  
            ofn.title = "选择图片";
            ofn.defExt = "JPG";//显示文件的类型  
            //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  
            if (GetOpenFileName(ofn))
            {
                return ofn.file;
            }
            return null;
        }
        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetSaveFileName([In, Out] OpenFileName lpofn);
        public static string GetSaveFileName()
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            //ofn.filter = "All Files\0*.*\0\0";
            ofn.filter = "Excel(*.xlsx)\0*.xlsx";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            string path = Application.streamingAssetsPath;
            path = path.Replace('/', '\\');
            //默认路径  
            ofn.initialDir = path;  
            ofn.title = "保存到";
            ofn.defExt = "xlsx";//显示文件的类型  
            ofn.flags = 0x00000004 | 0x00000800;
            ofn.flagsEx = 0x00000800;
            if (GetSaveFileName(ofn))
            {
                return ofn.file;
            }
            return null;
        }
    }

}