using System;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch
{
    /// <summary>
    /// Configuration class for the magnifier.
    /// </summary>
    public class Configuration
    {
        public string EyeTrackerAddress;
        public int BufferSize;
        public int PixelRange;
        public int CursorUpdate;
        public int OffScreenGlanceDist;
        public int DwellClickDelay;
        public int G9ButtonPosX;
        public int G9ButtonPosY;
        public int G9ButtonWidth;
        public int G9ButtonHeight;
        public int G9DwellTime;
        public bool UseEyeTracker;


        public bool MinimizeOnStart;


        public Configuration()
        {
            
        }



    }
}
