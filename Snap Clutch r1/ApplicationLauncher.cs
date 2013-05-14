/*
Copyright (c) 2008 - 2009, De Montfort University
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
is not permitted.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SnapClutch
{
    public class ApplicationLauncher
    {
        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int GW_OWNER = 4;
        public delegate int EnumWindowsProcDelegate(int hWnd, int lParam);
        [DllImport("user32")]
        private static extern int EnumWindows(EnumWindowsProcDelegate lpEnumFunc, int lParam);
        [DllImport("User32.Dll")]
        public static extern void GetWindowText(int h, StringBuilder s, int nMaxCount);
        [DllImport("user32", EntryPoint = "GetWindowLongA")]
        private static extern int GetWindowLongPtr(int hwnd, int nIndex);
        [DllImport("user32")]
        private static extern int GetParent(int hwnd);
        [DllImport("user32")]
        private static extern int GetWindow(int hwnd, int wCmd);
        [DllImport("user32")]
        private static extern int IsWindowVisible(int hwnd);
        [DllImport("user32")]
        private static extern int GetDesktopWindow();

        private static bool IsTaskbarWindow(int hWnd)
        {
            int lExStyle;
            int hParent;
            lExStyle = GetWindowLongPtr(hWnd, GWL_EXSTYLE);
            hParent = GetParent(hWnd);
            bool fTaskbarWindow = ((IsWindowVisible(hWnd) != 0) & (GetWindow(hWnd, GW_OWNER) == 0) & (hParent == 0 | hParent == GetDesktopWindow()));
            if ((lExStyle & WS_EX_TOOLWINDOW) == WS_EX_TOOLWINDOW)
            {
                fTaskbarWindow = false;
            }
            if ((lExStyle & WS_EX_APPWINDOW) == WS_EX_APPWINDOW)
            {
                fTaskbarWindow = true;
            }
            return fTaskbarWindow;
        }

        public static int EnumWindowsProc(int hWnd, int lParam)
        {
            if (IsTaskbarWindow(hWnd))
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText(hWnd, sb, sb.Capacity);
                String xMsg = sb.ToString();
                {
                    if (xMsg.Length > 0)
                    {
                        //Do whatever.
                    }
                }
            }
            return 1;
        }


    }
}
