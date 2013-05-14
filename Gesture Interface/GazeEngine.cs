/************************************************************
 * Gaze Engine Class V2
 * GazeEngine.cs
 * 
 * Stephen Vickers & Howell Istance
 * De Montfort University 
 * 7th August 2008
 * 
 * Description
 * -----------
 * This class processes all of the gaze data and 
 *  - calculates a rolling average
 *  - determines when a dwell occurs
 *  - determines when an off screen glance occurs
 * 
 * To Do
 * -----
 * 1)
 ************************************************************
 */

using System;
using System.Collections;
using System.Drawing;

namespace Gesture_Interface
{
    public class GazeEngine
    {
        public event DwellEventHandler DwellEvent;
        public event OffScreenGlanceEventHandler OffScreenGlanceEvent;
        public event SemiDwellEventHandler SemiDwellEvent;

        private bool outOfRange, dwell, dwellCheck, semiDwell;
        private ArrayList myArrayList;
        private int glanceMode;
        private int bufferSize = 30, pixelRange = 30, offScreenBuffer = 75, dwellClickTime = 700;
        private double screenResX = 0, screenResY = 0;
        private MovingAverage myMovingAvgX, myMovingAvgY;
        private float xIn, yIn;
        private enum Glance
        {
            Top = 1, Bottom = 2, Left = 3, Right = 4
        };
        private DateTime start;
        private TimeSpan dwellTimer, semiDwellTimer;
        private float dwellPosX, dwellPosY;

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

        }



        #region Moving Average

        // adds gaze position to the buffer for using the moving average algorithm
        // NOTE: this uses floats and not Points so convertion to coordinates is done afterwards
        public void AddPositionMovingAvg(float xArg, float yArg)
        {
            xIn = xArg;
            yIn = yArg;

            myMovingAvgX.AddSample(xIn);
            myMovingAvgY.AddSample(yIn);

            OffScreenGlanceMovingAvg();
            CalculateDwell();
        }

        public void AddMouseEmulatorPos(float xArg, float yArg)
        {
            xIn = xArg;
            yIn = yArg;

            myMovingAvgX.AddSample(xIn);
            myMovingAvgY.AddSample(yIn);
            
            OffScreenGlanceMovingAvg();
            CalculateDwell();
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

            // glance to left
            if (pnt.X <= (0 - offScreenBuffer))
            {
                //SnapClutchSounds.ModeChange();
                //System.Media.SystemSounds.Beep.Play();                    
                glanceMode = (int)Glance.Left;
                OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                OnOffScreenGlanceEvent(off1);
            }

            // glance to right
            if (pnt.X >= (screenResX + offScreenBuffer))
            {
                //SnapClutchSounds.ModeChange();
                //System.Media.SystemSounds.Beep.Play();
                glanceMode = (int)Glance.Right;
                OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                OnOffScreenGlanceEvent(off1);
            }

            // glance to top
            if (pnt.Y <= (0 - offScreenBuffer))
            {
                //SnapClutchSounds.ModeChange();
                //System.Media.SystemSounds.Beep.Play();
                glanceMode = (int)Glance.Top;
                OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                OnOffScreenGlanceEvent(off1);
            }

            // glance to bottom
            if (pnt.Y >= (screenResY + offScreenBuffer))
            {
                //SnapClutchSounds.ModeChange();
                //System.Media.SystemSounds.Beep.Play();
                glanceMode = (int)Glance.Bottom;
                OffScreenGlanceEventArgs off1 = new OffScreenGlanceEventArgs(glanceMode);
                OnOffScreenGlanceEvent(off1);
            }
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
                        DwellEventArgs d1 = new DwellEventArgs(true);
                        OnDwellEvent(d1);
                        dwellPosX = GetPositionXMovingAvg();
                        dwellPosY = GetPositionYMovingAvg();
                        //Console.WriteLine(dwellPosX);
                        //Console.WriteLine(dwellPosY);
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
            float tobiiX = argX;
            float tobiiY = argY;

            double dx = Math.Round(screenResX * Convert.ToDouble(tobiiX));
            double dy = Math.Round(screenResY * Convert.ToDouble(tobiiY));

            int x = Convert.ToInt32(dx);
            int y = Convert.ToInt32(dy);

            Point tempMousePos = new Point(x, y);

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

        // reset the array buffer
        private void ResetBuffer()
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
