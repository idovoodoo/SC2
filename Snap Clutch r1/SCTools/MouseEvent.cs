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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SnapClutch.SCTools
{
    public class MouseEvent
    {

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        private static extern void mouse_event
        (
            UInt32 dwFlags, // motion and click options
            int dx, // horizontal position or change
            int dy, // vertical position or change
            UInt32 dwData, // wheel movement
            IntPtr dwExtraInfo // application-defined information
        );
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const UInt32 MOUSEEVENTF_RIGHTUP = 0x10;

        public static void LeftClick()
        {
            LeftDown();
            LeftUp();
        }

        public static void LeftDown()
        {
            //SnapClutchSounds.Click();
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
        }

        public static void LeftUp()
        {
            //SnapClutchSounds.Click();
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void RightDown()
        {
            //SnapClutchSounds.Click();
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
        }

        public static void RightUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void DoubleLeft()
        {
            LeftDown();
            LeftUp();
            LeftDown();
            LeftUp();
        }

        public static void RightClick()
        {
            RightDown();
            RightUp();
        }

        public static void LeftClickPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void LeftClickDownPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
        }

        public static void LeftClickUpPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
        }

        public static void RightClickPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
        }

        public static void RightDownPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
        }

        public static void LeftRightDownPoint(int argX, int argY)
        {
            RightDownPoint(argX, argY);
            LeftClickDownPoint(argX, argY);
        }

        public static void LeftRightUpPoint(int argX, int argY)
        {
            RightUpPoint(argX, argY);
            LeftClickUpPoint(argX, argY);
        }

        public static void RightUpPoint(int argX, int argY)
        {
            //int x = argX * 65536 / Screen.PrimaryScreen.Bounds.Width;
            //int y = argY * 65536 / Screen.PrimaryScreen.Bounds.Height;
            //Console.WriteLine(argX + ", " + argY);
            //Console.WriteLine(x + ", " + y);
            Cursor.Position = new Point(argX, argY);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
            //SnapClutchSounds.Click();
        }

        public static void HorizontalMove(int argDx)
        {
            mouse_event(0, argDx, 0, 0, new System.IntPtr());
            //Console.WriteLine(argDx);
        } 
    }

    public struct MOUSESTATE
    {
        public bool leftButton;
        public bool rightButton;
        public bool tgOn;
        public bool tgParked;
        public bool goingForward;
        public bool aKey;
        public bool dKey;
        public bool wKey;
        public bool sKey;

        public bool numKey2;
        public bool numKey4;
        public bool numKey6;
        public bool numKey8;

        public bool mattIsSelected1;
        public int gazeX1;
        public int gazeX2;
        public int gazeY1;
        public int gazeY2;
        public int stringSelected; // the string currently selected by the user

        public bool joystickToolglass;
    }

}

