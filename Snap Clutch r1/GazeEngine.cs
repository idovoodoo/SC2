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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace SnapClutch
{
    public class GazeEngine
    {
        //:)
        public event DwellEventHandler DwellEvent;
        public event DwellBottomEventHandler DwellBottomEvent;
        public event DwellTopEventHandler DwellTopEvent;
        public event DwellLeftEventHandler DwellLeftEvent;
        public event DwellRightEventHandler DwellRightEvent;
        public event OffScreenGlanceEventHandler OffScreenGlanceEvent;
        public event SemiDwellEventHandler SemiDwellEvent;

        private bool outOfRange, dwell, dwellCheck, semiDwell;//, outOfRangeOffScreen;
        private ArrayList myArrayList;
        private int glanceMode;
        private int bufferSize = 30, pixelRange = 30, offScreenBuffer = 75, dwellClickTime = 700;
        private double screenResX = 0, screenResY = 0;
        private MovingAverage myMovingAvgX, myMovingAvgY;
        private float xIn, yIn;
        private enum Glance
        {
            None = 0, Top = 1, Bottom = 2, Left = 3, Right = 4, Top2 = 5, Bottom2 = 6, Left2 = 7, Right2 = 8
        };
        private DateTime start;
        private TimeSpan dwellTimer, semiDwellTimer;
        private float dwellPosX, dwellPosY;
        private bool resetGlance = false;
        private bool leftToggleOn = false;
        private bool rightToggleOn = false;
        private bool topToggleOn = false;
        private bool bottomToggleOn = false;

        // constructor
        public GazeEngine(int argBufferSize, int argPixelRange, int argCursorUpdate, int argOffScreenBuffer,
            int argDwellClickTime, double argScreenResX, double argScreenResY)
        {
            bufferSize = argBufferSize;
            pixelRange = argPixelRange;
            offScreenBuffer = argOffScreenBuffer;
            dwellClickTime = argDwellClickTime;
            screenResX = argScreenResX;
            screenResY = argScreenResY;
            dwell = false;
            semiDwell = false;
            myArrayList = new ArrayList();
            myMovingAvgX = new MovingAverage(bufferSize);
            myMovingAvgY = new MovingAverage(bufferSize);
            dwellTimer = new TimeSpan(0, 0, 0, 0, dwellClickTime);
            semiDwellTimer = new TimeSpan(0, 0, 0, 0, (dwellClickTime / 2));
            start = DateTime.Now;
            myMovingAvgX.AddSample(1);
            myMovingAvgY.AddSample(1);
        }



        #region Moving Average

        // adds gaze position to the buffer for using the moving average algorithm
        // NOTE: this uses floats and not Points so convertion to coordinates is done afterwards
        public void AddPositionMovingAvg(float xArg, float yArg)
        {
            xIn = xArg;
            yIn = yArg;
            Point pnt = ConvertToMouseCoords(xIn,yIn);

            myMovingAvgX.AddSample(xIn);
            myMovingAvgY.AddSample(yIn);

            OffScreenGlanceMovingAvg();

            // check for mouse/dev mode or eye tracker mode
            if (offScreenBuffer == -1)
            {
                if (pnt.X > 20 && pnt.X < Screen.PrimaryScreen.Bounds.Width && pnt.Y > 20 && pnt.Y < Screen.PrimaryScreen.Bounds.Height)
                // if within screen boundaries then reset resetGlance
                {
                    resetGlance = false;
                    //Console.WriteLine("reset glance");
                }
            }
            else
            {
                if (Screen.PrimaryScreen.Bounds.Contains(ConvertToMouseCoords(xIn, yIn)))
                {
                    resetGlance = false;
                    //Console.WriteLine("reset glance");
                }
            }

            CalculateDwell();
        }

        public void AddMouseEmulatorPos(float xArg, float yArg)
        {
            xIn = xArg;
            yIn = yArg;

            myMovingAvgX.AddSample(xIn);
            myMovingAvgY.AddSample(yIn);
            
            OffScreenGlanceMovingAvg();

            // if within screen boundaries then reset resetGlance
            if (Screen.PrimaryScreen.Bounds.Contains(ConvertToMouseCoords(xIn, yIn)))
            {
                resetGlance = false;
            }

            CalculateDwell();
            //CalculateOffScreenDwell();
        }

        public float GetMouseEmulationX()
        {
            return xIn;
        }

        public float GetMouseEmulationY()
        {
            return yIn;
        }

        // set glance mode for moving average
        private void OffScreenGlanceMovingAvg()
        {
            // convert to mouse coords for use with off screen buffer values
            Point pnt = ConvertToMouseCoords(xIn, yIn);

            if (!resetGlance)
            {
                // glance to left
                if (pnt.X <= (0 - offScreenBuffer))
                {
                    //SnapClutchSounds.ModeChange();
                    //System.Media.SystemSounds.Beep.Play();    
                    if (!leftToggleOn)
                    {
                        glanceMode = (int)Glance.Left;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = true;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                    else
                    {
                        glanceMode = (int)Glance.Left2;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                }

                // glance to right
                if (pnt.X >= (screenResX + offScreenBuffer))
                {
                    if (!rightToggleOn)
                    {
                        //SnapClutchSounds.ModeChange();
                        //System.Media.SystemSounds.Beep.Play();
                        glanceMode = (int)Glance.Right;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = true;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                    else
                    {
                        glanceMode = (int)Glance.Right2;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                }

                // glance to top
                if (pnt.Y <= (0 - offScreenBuffer))
                {
                    if (!topToggleOn)
                    {
                        //SnapClutchSounds.ModeChange();
                        //System.Media.SystemSounds.Beep.Play();
                        glanceMode = (int)Glance.Top;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = true;
                    }
                    else
                    {
                        glanceMode = (int)Glance.Top2;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                }

                // glance to bottom
                if (pnt.Y >= (screenResY + offScreenBuffer))
                {
                    if (!bottomToggleOn)
                    {
                        //SnapClutchSounds.ModeChange();
                        //System.Media.SystemSounds.Beep.Play();
                        glanceMode = (int)Glance.Bottom;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = true;
                        topToggleOn = false;
                    }
                    else
                    {
                        glanceMode = (int)Glance.Bottom2;
                        OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                        OnOffScreenGlanceEvent(off1);
                        resetGlance = true;
                        leftToggleOn = false;
                        rightToggleOn = false;
                        bottomToggleOn = false;
                        topToggleOn = false;
                    }
                }
            }



        }

        // calculates if a dwell has taken place off screen
        private int IsDwellOffScreen()
        {
            // convert to mouse coords for use with off screen buffer values
            Point pnt = ConvertToMouseCoords(xIn, yIn);

            // left of screen
            if (pnt.X <= (0 - offScreenBuffer))
            {
                return (int)Glance.Left; 
            }

            // right of screen
            if (pnt.X >= (screenResX + offScreenBuffer))
            {
                return (int)Glance.Right; 
            }

            // top of screen
            if (pnt.Y <= (0 - offScreenBuffer))
            {
                return (int)Glance.Top; 
            }

            // bottom of screen
            if (pnt.Y >= (screenResY + offScreenBuffer))
            {
                return (int) Glance.Bottom;               
            }

            return (int)Glance.None;
        }
      
        // calculates if a dwell is taking place
        private void CalculateDwell()
        {
            dwell = false;
            semiDwell = false;

            if (OutOfRange())
            {
                // no dwell lets reset
                start = DateTime.Now;
                dwellCheck = true;
                dwell = false;
                semiDwell = false;
            }
            else
            {
                // we may have a dwell.... lets check
                if (dwellCheck)
                {
                    TimeSpan timer = DateTime.Now - start;

                    if ((DateTime.Now - start) > dwellTimer)
                    {
                        dwell = true;
                        dwellCheck = false;
                        //SnapClutchSounds.Click();

                        switch (IsDwellOffScreen())
                        {
                            case 0:
                                // normal on screen dwell
                                DwellEventArgs d1 = new DwellEventArgs(true);
                                OnDwellEvent(d1);
                                break;
                            case 1:
                                // top off screen dwell
                                DwellTopEventArgs dt1 = new DwellTopEventArgs(true);
                                OnDwellTopEvent(dt1);
                                break;
                            case 2:
                                // bottom off screen dwell
                                DwellBottomEventArgs db1 = new DwellBottomEventArgs(true);
                                OnDwellBottomEvent(db1);
                                break;
                            case 3:
                                // left off screen dwell
                                DwellLeftEventArgs dl1 = new DwellLeftEventArgs(true);
                                OnDwellLeftEvent(dl1);
                                break;
                            case 4:
                                // right off screen dwell
                                DwellRightEventArgs dr1 = new DwellRightEventArgs(true);
                                OnDwellRightEvent(dr1);
                                break;

                        }

                        dwellPosX = GetPositionXMovingAvg();
                        dwellPosY = GetPositionYMovingAvg();
                    }

                    if ((DateTime.Now - start) > semiDwellTimer)
                    {
                        //SnapClutchSounds.ModeChange();
                        semiDwell = true;

                        SemiDwellEventArgs s1 = new SemiDwellEventArgs(true);
                        OnSemiDwellEvent(s1);
                    }
                }
            }
        }




        public float GetDwellPosX()
        {
            return dwellPosX;
        }

        public float GetDwellPosY()
        {
            return dwellPosY;
        }

        // returns the position of x using moving average
        public float GetPositionXMovingAvg()
        {
            return myMovingAvgX.Average;
        }

        // returns the position of y using moving average
        public float GetPositionYMovingAvg()
        {
            return myMovingAvgY.Average;
        }

        #endregion

        #region Events

        // dwell event subscriber check
        public void OnDwellEvent(DwellEventArgs e)
        {
            if (DwellEvent != null)
                DwellEvent(new object(), e);
        }

        // dwell bottom event subscriber check
        public void OnDwellBottomEvent(DwellBottomEventArgs e)
        {
            if (DwellBottomEvent != null)
                DwellBottomEvent(new object(), e);
        }

        // dwell top event subscriber check
        public void OnDwellTopEvent(DwellTopEventArgs e)
        {
            if (DwellTopEvent != null)
                DwellTopEvent(new object(), e);
        }

        // dwell left event subscriber check
        public void OnDwellLeftEvent(DwellLeftEventArgs e)
        {
            if (DwellLeftEvent != null)
                DwellLeftEvent(new object(), e);
        }

        // dwell right event subscriber check
        public void OnDwellRightEvent(DwellRightEventArgs e)
        {
            if (DwellRightEvent != null)
                DwellRightEvent(new object(), e);
        }

        // off screen glance event subscriber check
        public void OnOffScreenGlanceEvent(OffScreenGlanceEventArgs e)
        {
            if (OffScreenGlanceEvent != null)
                OffScreenGlanceEvent(new object(), e);
        }

        // off screen glance event subscriber check
        public void OnSemiDwellEvent(SemiDwellEventArgs e)
        {
            if (SemiDwellEvent != null)
                SemiDwellEvent(new object(), e);
        }

        #endregion

        // Convert eye tracker coords to mouse points
        private Point ConvertToMouseCoords(float argX, float argY)
        {
            //float tobiiX = argX;
            //float tobiiY = argY;

            //double dx = Math.Round(screenResX * Convert.ToDouble(argX));
            //double dy = Math.Round(screenResY * Convert.ToDouble(argY));

            //int x = Convert.ToInt32(Math.Round(screenResX * Convert.ToDouble(argX)));
            //int y = Convert.ToInt32(Math.Round(screenResY * Convert.ToDouble(argY)));

            Point tempMousePos = new Point(Convert.ToInt32(Math.Round(screenResX * Convert.ToDouble(argX)))
                , Convert.ToInt32(Math.Round(screenResY * Convert.ToDouble(argY))));

            return tempMousePos;
        }

        // calculate if the current gaze position is out of pixel range
        private bool OutOfRange()
        {
            int dX;
            int dY;

            Point avPnt = ConvertToMouseCoords(myMovingAvgX.Average, myMovingAvgY.Average);
            Point newPnt = ConvertToMouseCoords(xIn, yIn);

            // set x coords to positive outputs
            if (newPnt.X >= avPnt.X)
                dX = newPnt.X - avPnt.X;
            else
                dX = avPnt.X - newPnt.X;

            // set y coords to positive outputs
            if (newPnt.Y >= avPnt.Y)
                dY = newPnt.Y - avPnt.Y;
            else
                dY = avPnt.Y - newPnt.Y;

            // determine if dX or dY is out of range
            if (dX > pixelRange || dY > pixelRange)
                outOfRange = true;
            else
                outOfRange = false;

            return outOfRange;
        }

        // calculate if the current gaze position is out of pixel range
        private bool OutOfRangeOffScreen()
        {
            int dX;
            int dY;

            Point avPnt = ConvertToMouseCoords(myMovingAvgX.Average, myMovingAvgY.Average);
            Point newPnt = ConvertToMouseCoords(xIn, yIn);

            // set x coords to positive outputs
            if (newPnt.X >= avPnt.X)
                dX = newPnt.X - avPnt.X;
            else
                dX = avPnt.X - newPnt.X;

            // set y coords to positive outputs
            if (newPnt.Y >= avPnt.Y)
                dY = newPnt.Y - avPnt.Y;
            else
                dY = avPnt.Y - newPnt.Y;

            // determine if dX or dY is out of range
            if (dX > (pixelRange * 2) || dY > (pixelRange * 2))
                outOfRange = true;
            else
                outOfRange = false;

            return outOfRange;
        }

        // reset the array buffer
        public void ResetBuffer()
        {
            myArrayList.Clear();
        }

        #region Get-Sets

        public bool IsDwell()
        {
            return dwell;
        }

        public bool IsSemiDwell()
        {
            return semiDwell;
        }

        public int IsGlance()
        {
            return glanceMode;
        }

        public void SetBufferSize(int argBuffer)
        {
            bufferSize = argBuffer;
            myMovingAvgX = new MovingAverage(bufferSize);
            myMovingAvgY = new MovingAverage(bufferSize);
        }

        public void SetPixelRange(int argPixel)
        {
            pixelRange = argPixel;
        }

        public void SetDwellClickTime(int argDwellTime)
        {
            dwellClickTime = argDwellTime;

            dwellTimer = new TimeSpan(0, 0, 0, 0, dwellClickTime);
            semiDwellTimer = new TimeSpan(0, 0, 0, 0, (dwellClickTime / 2));
        }

        public void SetOffScreenBuffer(int argOffScreen)
        {
            offScreenBuffer = argOffScreen;
        }

        public void SetScreenRes(double argX, double argY)
        {
            screenResX = argX;
            screenResY = argY;
        }

        public void ResetGlance()
        {
            glanceMode = 1;
        }

        #endregion
    }

    public delegate void DwellEventHandler(object o, DwellEventArgs e);

    public delegate void DwellBottomEventHandler(object o, DwellBottomEventArgs e);

    public delegate void DwellTopEventHandler(object o, DwellTopEventArgs e);

    public delegate void DwellLeftEventHandler(object o, DwellLeftEventArgs e);

    public delegate void DwellRightEventHandler(object o, DwellRightEventArgs e);

    public delegate void OffScreenGlanceEventHandler(object o, OffScreenGlanceEventArgs e);

    public delegate void SemiDwellEventHandler(object o, SemiDwellEventArgs e);

    public class DwellEventArgs : EventArgs
    {
        public bool IsDwell;

        public DwellEventArgs(bool argDwell)
        {
            IsDwell = argDwell;
        }
    }

    public class DwellBottomEventArgs : EventArgs
    {
        public bool BottomDwell;

        public DwellBottomEventArgs(bool argDwell)
        {
            BottomDwell = argDwell;
        }
    }

    public class DwellTopEventArgs : EventArgs
    {
        public bool TopDwell;

        public DwellTopEventArgs(bool argDwell)
        {
            TopDwell = argDwell;
        }
    }
    public class DwellLeftEventArgs : EventArgs
    {
        public bool LeftDwell;

        public DwellLeftEventArgs(bool argDwell)
        {
            LeftDwell = argDwell;
        }
    }

    public class DwellRightEventArgs : EventArgs
    {
        public bool RightDwell;

        public DwellRightEventArgs(bool argDwell)
        {
            RightDwell = argDwell;
        }
    }
    public class OffScreenGlanceEventArgs : EventArgs
    {
        public int Glance;

        public OffScreenGlanceEventArgs(int argGlance)
        {
            Glance = argGlance;
        }
    }

    public class SemiDwellEventArgs : EventArgs
    {
        public bool IsSemiDwell;

        public SemiDwellEventArgs(bool argSemiDwell)
        {
            IsSemiDwell = argSemiDwell;
        }
    }
}
