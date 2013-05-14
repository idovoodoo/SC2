using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SnapClutch.SCTools
{
    public class InputControl
    {

        //C# signature for "SendInput()"
        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize); //array of inputs, incase multiple inputs have to be done

        //C# signature for "GetMessageExtraInfo()"
        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        static extern IntPtr GetMessageExtraInfo();

        //C# signature for "GetWindowRect()"
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        //C# signature for "FindWindow()"
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Misc variables
        RECT rect;   //New RECT struct representing the size of the WoW Window
        INPUT mouse;  //Declares a new INPUT

        /// <summary>
        /// Defines a RECT struct used to find infos about the size of the WoW window
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        /// <summary>
        /// Type of Input: mouse, keyboard, hardware
        /// </summary>
        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        /// <summary>
        /// Mouse events
        /// </summary>
        [Flags()]
        private enum MOUSEEVENTF
        {
            MOVE = 0x0001,  // mouse move
            LEFTDOWN = 0x0002,  // left button down
            LEFTUP = 0x0004,  // left button up
            RIGHTDOWN = 0x0008,  // right button down
            RIGHTUP = 0x0010,  // right button up
            MIDDLEDOWN = 0x0020,  // middle button down
            MIDDLEUP = 0x0040,  // middle button up
            XDOWN = 0x0080,  // x button down
            XUP = 0x0100,  // x button down
            WHEEL = 0x0800,  // wheel button rolled
            VIRTUALDESK = 0x4000,  // map to entire virtual desktop
            ABSOLUTE = 0x8000,  // absolute move
        }

        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            KEYDOWN = 0x0000,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        /// <summary>
        /// Flags for the mouseinput
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        /// <summary>
        /// Struct containing type of inputs: MOUSEINPUT, KEYBDINPUT, HARDWAREINPUT.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        /// <summary>
        /// Declares a new mouse control.
        /// </summary>
        public InputControl()
        {
            bool isRectFound;
            rect = new RECT();
            mouse = new INPUT();

            //try
            //{
            //    isRectFound = GetWindowRect(FindWindow(null, "World of Warcraft"), out rect);
            //    if (!isRectFound)
            //        throw new Exception("Cannot find the World of Warcraft window");
            //}
            //catch (Exception E)
            //{
            //    MessageBox.Show(E.Message);
            //    return;
            //}

        }

        /// <summary>
        /// Centers the mouse cursor in the WoW Window
        /// </summary>
        public uint CenterMouse()
        {
            mouse.mi.dx = 65535 * ((rect.Width + rect.X) / 2) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            mouse.mi.dy = 65535 * ((rect.Height + rect.Y) / 2) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            mouse.mi.mouseData = 0;
            mouse.mi.dwFlags = (int)MOUSEEVENTF.MOVE + (int)MOUSEEVENTF.ABSOLUTE;

            INPUT[] input = { mouse };
            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        /// <summary>
        /// This function simulates a simple mouseclick at the destination cursor position.
        /// </summary>
        /// <param name="destX">Destination X for the click</param>
        /// <param name="destY">Destination Y for the click</param>
        /// <param name="button">Mouse button used for the click: "left" or "right"</param>
        /// <returns>Returns the SendInput to be executed.</returns>
        public uint Click(string button)
        {
            //Click destination, adds X and Y values of the rect incase WoW is in Window Mode.
            mouse.mi.dx = 0;
            mouse.mi.dy = 0;
            mouse.mi.mouseData = 0;  //Mostly used for scrolling event

            INPUT input_up = mouse;  //declares an input to put mouse button up

            //Depending on which button is called.
            switch (button)
            {
                case "left":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;
                    input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;
                    break;
                case "right":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.RIGHTDOWN;
                    input_up.mi.dwFlags = (int)MOUSEEVENTF.RIGHTUP;
                    break;
                default:
                    throw new Exception();
            }

            INPUT[] input = { mouse, input_up };

            return SendInput(2, input, Marshal.SizeOf(input_up));
        }

        /// <summary>
        /// Press a mouse button down
        /// </summary>
        /// <param name="button">Specifies the button that should be pressed: "left" or "right"</param>
        /// <returns>Returns the SendInput to be executed.</returns>
        public uint MouseDown(string button)
        {
            //dx and dy flags to 0 to click at the current position of the cursor.
            mouse.mi.dx = 0;
            mouse.mi.dy = 0;
            mouse.mi.mouseData = 0;  //Mostly used for scrolling event

            switch (button)
            {
                case "left":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;
                    break;
                case "right":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.RIGHTDOWN;
                    break;
                default:
                    throw new Exception();
            }

            INPUT[] input = { mouse };
            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        /// <summary>
        /// Release a mouse button
        /// </summary>
        /// <param name="button">Specifies the button that should be released: "left" or "right"</param>
        /// <returns>Returns the SendInput to be executed.</returns>
        public uint MouseUp(string button)
        {
            //dx and dy flags to 0 to click at the current position of the cursor.
            mouse.mi.dx = 0;
            mouse.mi.dy = 0;
            mouse.mi.mouseData = 0;  //Mostly used for scrolling event

            switch (button)
            {
                case "left":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;
                    break;
                case "right":
                    mouse.mi.dwFlags = (int)MOUSEEVENTF.RIGHTUP;
                    break;
                default:
                    throw new Exception();
            }
            INPUT[] input = { mouse };
            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        /// <summary>
        /// Moves the cursor to a specified absolute location
        /// </summary>
        /// <param name="destX">Absolute Destination X</param>
        /// <param name="destY">Absolute Destination Y</param>
        /// <returns>Returns the SendInput to be executed.</returns>
        public uint AMoveMouse(Int32 destX, Int32 destY)
        {
            mouse.mi.dx = 65535 * (destX + rect.X) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            mouse.mi.dy = 65535 * (destY + rect.Y) / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            mouse.mi.mouseData = 0;
            mouse.mi.dwFlags = (int)MOUSEEVENTF.MOVE + (int)MOUSEEVENTF.ABSOLUTE;

            INPUT[] input = { mouse };
            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        /// <summary>
        /// Moves the cursor to a specified relative location
        /// </summary>
        /// <param name="destX">Relative destination for the X, depending on the current's cursor position</param>
        /// <param name="destY">Relative destination for the Y, depending on the current's cursor position</param>
        /// <returns>Returns the SendInput to be executed.</returns>
        public uint RMoveMouse(Int32 destX, Int32 destY)
        {
            mouse.mi.dx = destX;
            mouse.mi.dy = destY;
            mouse.mi.mouseData = 0;
            mouse.mi.dwFlags = (int)MOUSEEVENTF.MOVE;

            INPUT[] input = { mouse };
            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        public uint keyDown(Keys key)
        {
            mouse.ki.wVk = Convert.ToInt16(key);
            mouse.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            mouse.ki.time = 0;
            mouse.type = 0x01;

            INPUT[] input = { mouse };

            return SendInput(1, input, Marshal.SizeOf(mouse));
        }

        public uint keyUp(Keys key)
        {
            mouse.ki.wVk = Convert.ToInt16(key);
            mouse.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            mouse.ki.time = 0;
            mouse.type = 0x01;

            INPUT[] input = { mouse };

            return SendInput(1, input, Marshal.SizeOf(mouse));
        }
    }
} 
