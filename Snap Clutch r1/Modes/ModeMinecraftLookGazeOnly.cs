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
    // dwell click on left mouse button
    public class ModeMinecraftLookGazeOnly : Mode
    {
        private Point centrePoint = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2), Screen.PrimaryScreen.WorkingArea.Height / 2);
        private const int MAX_SPEED = 20;
        public int MAX_VECTOR = 250;
        public int MAX_VECTOR_Y = 200;
        public int MAX_SCALE_Y = 30;
        public int MAX_SCALE = 30;
        private const int MIN_SCALE = 0;

        public static Rectangle screenSize = Screen.PrimaryScreen.Bounds;
        private int x1 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x2 = Convert.ToInt32(screenSize.Width * 0.6);
        private int x3 = Convert.ToInt32(screenSize.Width * 0.4);
        private int x4 = Convert.ToInt32(screenSize.Width * 0.6);

        private int y1 = Convert.ToInt32(screenSize.Height * 0.4);
        private int y2 = Convert.ToInt32(screenSize.Height * 0.6);

        // constructor
        public ModeMinecraftLookGazeOnly()
        {
            modeName = "Minecraft Loock Around Gaze Only";
        }

        // execute the mode


        // execute the mode
        public override MOUSESTATE Execute(Point argGazePos, MOUSESTATE argMouse, GazeOverlay argGazeOverlay)
        {
            myMouse = argMouse;

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

            //just start walking!!!
            //if (!argMouse.wKey)
            //{
               // KeyEvent.WKeyDown();
               // argMouse.wKey = true;
           // }
            if (argMouse.wKey)
            {
                KeyEvent.WKeyUp();
                argMouse.wKey = false;
            }

            double xCalc = 0.0;
            double yCalc = 0.0;

            double xV = 0.0;
            double yV = 0.0;

            if (argGazePos.Y < Cursor.Position.Y) //above cursor
            {
                if (argGazePos.X < Cursor.Position.X) // left of cursor
                {
                    xCalc = Cursor.Position.X - argGazePos.X;
                    yCalc = Cursor.Position.Y - argGazePos.Y;

                    xV = xCalc - ((screenSize.Width / 2) - x1);
                    yV = yCalc - ((screenSize.Height / 2) - y1);
                }
                else //right of cursor
                {

                    xCalc = argGazePos.X - Cursor.Position.X;
                    yCalc = Cursor.Position.Y - argGazePos.Y;

                    xV = xCalc - (x2 - (screenSize.Width / 2));
                    yV = yCalc - ((screenSize.Height / 2) - y1);
                }

            }
            else //below cursor
            {
                if (argGazePos.X < Cursor.Position.X) // left of cursor
                {
                    xCalc = Cursor.Position.X - argGazePos.X;
                    yCalc = argGazePos.Y - Cursor.Position.Y;

                    xV = xCalc - ((screenSize.Width / 2) - x1);
                    yV = yCalc - (y2 - (screenSize.Height / 2));
                }
                else //right of cursor
                {
                    xCalc = argGazePos.X - Cursor.Position.X;
                    yCalc = argGazePos.Y - Cursor.Position.Y;

                    xV = xCalc - (x2 - (screenSize.Width / 2));
                    yV = yCalc - (y2 - (screenSize.Height / 2));
                }
            }

            // vectorLength = Math.Sqrt((xCalc * 2) + (yCalc * 2));

            // thetaRadians = Math.Atan(yCalc / xCalc);

            // double dY = Math.Round(MAX_SPEED * Math.Sin(thetaRadians), 2);
            // double dX = Math.Round(MAX_SPEED * Math.Cos(thetaRadians), 2);

            double xDisp = (xV / MAX_VECTOR) * MAX_SCALE;
            double yDisp = (yV / MAX_VECTOR_Y) * MAX_SCALE_Y;

            //double tempX;
            //double tempY;



            //if (xCalc > MAX_VECTOR)
            //    tempX = MAX_SCALE;
            //else
            //    tempX = (xCalc / MAX_VECTOR) * MAX_SCALE;

            //if (yCalc > MAX_VECTOR_Y)
            //    tempY = MAX_SCALE_Y;
            //else
            //    tempY = (yCalc / MAX_VECTOR_Y) * MAX_SCALE_Y;

            //Console.WriteLine(xCalc + " " + yCalc + " " + tempX + " " + tempY);

            int diX = 0;
            int diY = 0;

            try
            {
                diX = Convert.ToInt32(xDisp);
                diY = Convert.ToInt32(yDisp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            if (diX > MAX_SPEED)
                diX = MAX_SPEED;
            if (diY > MAX_SPEED)
                diY = MAX_SPEED;

            //diX = diX / 5;
            //diY = diY / 5;

            int xTemp = 0;
            int yTemp = 0;

            if (argGazePos.Y < y1) // look up
            {
                //if (!argMouse.shiftKey) // lets slow movement down
                //{
                //    KeyEvent.LeftShiftKeyDown();
                //    argMouse.shiftKey = true;
                //}

                if (argGazePos.X < x1) // up and left
                {
                    xTemp = Cursor.Position.X - diX;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y - diY;
                }
                else if (argGazePos.X > x2) // up and right
                {
                    xTemp = Cursor.Position.X + diX;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y - diY;
                }
                else //just look up
                {
                    xTemp = Cursor.Position.X;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y - diY;
                }
                Cursor.Position = new Point(xTemp, yTemp);
            }
            else if (argGazePos.Y > y2) // look down
            {
                //if (!argMouse.shiftKey) // lets slow movement down
                //{
                //    KeyEvent.LeftShiftKeyDown();
                //    argMouse.shiftKey = true;
                //}

                if (argGazePos.X < x1) // down and left
                {
                    xTemp = Cursor.Position.X - diX;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y + diY;
                }
                else if (argGazePos.X > x2) // down and right
                {
                    xTemp = Cursor.Position.X + diX;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y + diY;
                }
                else //just look down
                {
                    xTemp = Cursor.Position.X;
                    yTemp = Cursor.Position.Y;
                    //yTemp = Cursor.Position.Y + diY;
                }
                Cursor.Position = new Point(xTemp, yTemp);
            }
            else if (argGazePos.Y > y1 && argGazePos.Y < y2)
            {
                //if (!argMouse.shiftKey) // lets slow movement down
                //{
                //    KeyEvent.LeftShiftKeyDown();
                //    argMouse.shiftKey = true;
                //}

                if (argGazePos.X < x1) // just left
                {
                    xTemp = Cursor.Position.X - diX;
                    yTemp = Cursor.Position.Y;
                }
                else if (argGazePos.X > x2) // just right
                {
                    xTemp = Cursor.Position.X + diX;
                    yTemp = Cursor.Position.Y;
                }
                else
                {
                    if (argMouse.shiftKey) // lets speed the movement back up
                    {
                        KeyEvent.LeftShiftKeyUp();
                        argMouse.shiftKey = false;
                    }

                    xTemp = Cursor.Position.X;
                    yTemp = Cursor.Position.Y;
                }
                Cursor.Position = new Point(xTemp, yTemp);
            }
            else
            {
                Console.WriteLine("doing nothing!!");

                if (argMouse.shiftKey) // lets speed the movement back up
                {
                    KeyEvent.LeftShiftKeyUp();
                    argMouse.shiftKey = false;
                }
            }

            return argMouse;
        }

        public override MOUSESTATE SomeEvent(Point argGazePos, bool argSwitch, bool argDown)
        {
            //SnapClutchSounds.Click2();
            Cursor.Position = argGazePos;

            if (argSwitch)
            {
                if (argDown)
                    MouseEvent.LeftDown();
                else
                    MouseEvent.LeftUp();
            }
            // using dwell
            else
            {
                MouseEvent.LeftClick();
            }
            return myMouse;
        }


        // return the name of the mode
        public override string GetModeName()
        {
            return modeName;
        }
    }
}
