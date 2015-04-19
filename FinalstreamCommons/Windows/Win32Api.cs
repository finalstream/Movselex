﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FinalstreamCommons.Windows
{
    public static class Win32Api
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        extern static int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        public static string GetWindowCaption(IntPtr hwnd, string className, string regexPattern)
        {
            StringBuilder sb = new StringBuilder(100);
            IntPtr dialogHandle;
            IntPtr windowHandle;
            dialogHandle = FindWindowEx(hwnd, IntPtr.Zero, null, null);
            while (dialogHandle != IntPtr.Zero)
            {
                windowHandle = FindWindowEx(dialogHandle, IntPtr.Zero, null, null);
                while (windowHandle != IntPtr.Zero)
                {
                    GetWindowText(windowHandle, sb, sb.Capacity);  // タイトルバー文字列を取得
                    if (new System.Text.RegularExpressions.Regex(regexPattern).IsMatch(sb.ToString()))
                    {
                        return sb.ToString();
                    }
                    windowHandle = FindWindowEx(dialogHandle, windowHandle, null, null);
                }
                dialogHandle = FindWindowEx(hwnd, dialogHandle, null, null);
            }
            /*
            if (hWndChild != IntPtr.Zero)
            {
                GetWindowText(hwnd, sb, sb.Capacity);  // タイトルバー文字列を取得
            }*/

            return "";
        }

    }
}
