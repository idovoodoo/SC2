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
using System.Drawing;
using System.Windows.Forms;

namespace SnapClutch.SCTools
{
    public class GazeOverlay
    {
        private List<Rectangle> myZoneRectList;
        private bool overlayOn = true;

        public GazeOverlay()
        {
            myZoneRectList = new List<Rectangle>();
        }

        // add a zone
        public void AddZone(string argName, Point argTopLeft, int argWidth, int argHeight)
        {
            string name = argName;
            Point topLeft = argTopLeft;
            int width = argWidth;
            int height = argHeight;

            // create a rectangle that represents a zone and adds it to the zonelist
            Rectangle myRectangle = new Rectangle(topLeft.X, topLeft.Y, width, height);
            myZoneRectList.Add(myRectangle);
        }

        public void SetOverlayStatus(bool argStatus)
        {
            overlayOn = argStatus;
        }

        // get the cursor position based on the gaze point argument using the zones
        // within the overlay
        public Point GetPosition(Point argGazePoint)
        {
            
            
            Point cursorPos = argGazePoint;

            if (overlayOn)
            {
                foreach (Rectangle rect in myZoneRectList)
                {
                    Rectangle myR = rect;
                    // if gaze point is within a rectangle then assign the cursor pos
                    // to the centre point of the rectangle
                    if (rect.Contains(argGazePoint))
                    {
                        int centreX = myR.X + (rect.Width / 2);
                        int centreY = myR.Y + (rect.Height / 2);
                        Point centrePos = new Point(centreX, centreY);
                        cursorPos = centrePos;

                    }

                }
            }
            return cursorPos;
        }
    }
}
