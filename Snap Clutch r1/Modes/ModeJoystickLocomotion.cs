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
using System.Threading;
using SnapClutch.SCTools;

namespace SnapClutch.Modes
{
    public class ModeJoystickLocomotion : Mode
    {

        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private bool goingForward = false;
        private bool rotating = false;
        private JoystickToolglass myJoystickToolglass, myRotateToolglass;
        private int toolglassWidthHeight = 150;
        private int speedFactor = 20;
        private double xGrad1 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.45);
        private double xGrad2 = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width * 0.55);
        private int offsetPlacement = 200;

        // constructor
        public ModeJoystickLocomotion()
        {
            modeName = "Joystick Locomotion";
            myJoystickToolglass = new JoystickToolglass();
            myRotateToolglass = new JoystickToolglass();
            myJoystickToolglass.Hide();
            myRotateToolglass.Hide();
        }

        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {

            if (!argMouse.joystickToolglass)
            {
                // show the toolglass
                argMouse.joystickToolglass = true;
                
                myJoystickToolglass.Show();
                myJoystickToolglass.Width = toolglassWidthHeight;
                myJoystickToolglass.Height = toolglassWidthHeight;
                myJoystickToolglass.Left = centrePoint.X - (myJoystickToolglass.Width / 2) - offsetPlacement;
                myJoystickToolglass.Top = centrePoint.Y + (screenSize.Height / 5);
                myJoystickToolglass.BackColor = Color.Red;
                myJoystickToolglass.BackgroundImage = Properties.Resources.right_foot_print;
                myJoystickToolglass.BackgroundImageLayout = ImageLayout.Center;

                myRotateToolglass.Show();
                myRotateToolglass.Width = toolglassWidthHeight;
                myRotateToolglass.Height = toolglassWidthHeight;
                myRotateToolglass.Left = centrePoint.X - (myRotateToolglass.Width / 2) + offsetPlacement;
                myRotateToolglass.Top = centrePoint.Y + (screenSize.Height / 5);
                myRotateToolglass.BackColor = Color.Red;
                myRotateToolglass.BackgroundImage = Properties.Resources.rotate_arrows;
                myRotateToolglass.BackgroundImageLayout = ImageLayout.Center;
                

                //move mouse cursor to centre of screen
                Cursor.Position = new Point(centrePoint.X, centrePoint.Y +2); //was y+2
            }
            else
            {
                // toolglass is on
                // has locomotion started
                if (!goingForward && !rotating)
                {
                    // we are moving forward so lets switch off
                    //if (argMouse.wKey)
                    //{
                        KeyEvent.WKeyUp();
                        argMouse.wKey = false;
                        myJoystickToolglass.BackColor = Color.Red;
                        myRotateToolglass.BackColor = Color.Red;
                        if (argMouse.rightButton)
                        {
                            MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2); //was y+2
                            argMouse.rightButton = false;
                        }
                    //}
                }
                else if (goingForward && !rotating)
                {
                    // we are moving forward so lets turn left/right or whatever!!!
                    if (!argMouse.wKey)
                    {
                        // if key isn't down then press it!
                        KeyEvent.WKeyDown();
                        argMouse.wKey = true;
                        myJoystickToolglass.BackColor = Color.Green;
                    }

                    //turn left
                    if (argGazePos.X < xGrad1)// centrePoint.X)
                    {
                        double offset = speedFactor - ((argGazePos.X / xGrad1) * speedFactor);

                        MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y+2); //was y+2
                        argMouse.rightButton = true;
                        Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2); //was +2

                    }
                     //turn right
                    if (argGazePos.X > xGrad2)// centrePoint.X)
                    {
                        double offset = ((argGazePos.X - xGrad2) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad2)) * speedFactor;

                        MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y+2); //was y+2
                        argMouse.rightButton = true;
                        Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2); //was +2
                    }
                }
                else if (!rotating)
                {
                    myRotateToolglass.BackColor = Color.Red;

                    if (argMouse.rightButton)
                    {
                        MouseEvent.RightUpPoint(centrePoint.X, centrePoint.Y + 2); //was y+2
                        argMouse.rightButton = false;
                    }
                }
                else if (rotating)
                {
                    myRotateToolglass.BackColor = Color.Green;

                    // making sure the w key isnt down
                    if (argMouse.wKey)
                    {
                        // if key isn't down then press it!
                        KeyEvent.WKeyUp();
                        goingForward = false;
                        argMouse.wKey = false;
                        myJoystickToolglass.BackColor = Color.Red;

                    }

                    //turn left
                    if (argGazePos.X < centrePoint.X)
                    {
                        double offset = speedFactor - ((argGazePos.X / xGrad1) * speedFactor);

                        MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2); //was y+2
                        argMouse.rightButton = true;
                        Cursor.Position = new Point(centrePoint.X - Convert.ToInt16(offset), centrePoint.Y + 2); //was +2

                    }
                    //turn right
                    if (argGazePos.X > centrePoint.X)
                    {
                        double offset = ((argGazePos.X - xGrad2) / (Screen.PrimaryScreen.WorkingArea.Width - xGrad2)) * speedFactor;

                        MouseEvent.RightDownPoint(centrePoint.X, centrePoint.Y + 2); //was y+2
                        argMouse.rightButton = true;
                        Cursor.Position = new Point(centrePoint.X + Convert.ToInt16(offset), centrePoint.Y + 2); //was +2
                    }
                }
                else
                {
                }
            }

            return argMouse;
        }

        public void TurnOffToolglass()
        {
            myJoystickToolglass.Hide();
            myRotateToolglass.Hide();
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            // check if dwell is over the toolglass!
            if (myJoystickToolglass.Bounds.Contains(argGazePos))
            {
                if (!goingForward)
                    goingForward = true;
                else
                    goingForward = false;
            }

            if (myRotateToolglass.Bounds.Contains(argGazePos))
            {
                if (!rotating)
                    rotating = true;
                else
                    rotating = false;
            }


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
