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
using System.Windows.Forms;
using SnapClutch.SCTools;

namespace SnapClutch.Modes
{
    // dwell click on left mouse button
    public class ModeMinecraftCBlock : Mode
    {
        MOUSESTATE myMouse;
        bool onInit = false;

        // constructor
        public ModeMinecraftCBlock()
        {
            modeName = "Change block type";
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            if (!onInit)
            {
                //first time so set the current block to 1
                argMouse.mcBlock = 1;
            }

            if (argMouse.leftButton)
            {
                MouseEvent.LeftUp();
                argMouse.leftButton = false;
            }
            if (argMouse.rightButton)
            {
                MouseEvent.RightUp();
                argMouse.rightButton = false;
            }


            switch (argMouse.mcBlock)
            {
                case 1:
                    KeyEvent.NumKey2Down();
                    KeyEvent.NumKey2Up();
                    argMouse.mcBlock = 2;
                    break;
                case 2:
                    KeyEvent.NumKey3Down();
                    KeyEvent.NumKey3Up();
                    argMouse.mcBlock = 3;
                    break;
                case 3:
                    KeyEvent.NumKey4Down();
                    KeyEvent.NumKey4Up();
                    argMouse.mcBlock = 4;
                    break;
                case 4:
                    KeyEvent.NumKey5Down();
                    KeyEvent.NumKey5Up();
                    argMouse.mcBlock = 5;
                    break;
                case 5:
                    KeyEvent.NumKey6Down();
                    KeyEvent.NumKey6Up();
                    argMouse.mcBlock = 6;
                    break;
                case 6:
                    KeyEvent.NumKey7Down();
                    KeyEvent.NumKey7Up();
                    argMouse.mcBlock = 7;
                    break;
                case 7:
                    KeyEvent.NumKey8Down();
                    KeyEvent.NumKey8Up();
                    argMouse.mcBlock = 8;
                    break;
                case 8:
                    KeyEvent.NumKey9Down();
                    KeyEvent.NumKey9Up();
                    argMouse.mcBlock = 9;
                    break;
                case 9:
                    KeyEvent.NumKey1Down();
                    KeyEvent.NumKey1Up();
                    argMouse.mcBlock = 1;
                    break;
            }

       

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            return myMouse;
        }

        public override MOUSESTATE BlinkEvent(Point argGazePos, int argEye)
        {
            return myMouse;
        }

        // return the name of the mode
        public override string GetModeName()
        {
            return modeName;
        }
    }
}
