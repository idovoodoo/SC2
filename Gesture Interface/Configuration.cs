using System;
using System.Collections.Generic;
using System.Text;

namespace Gesture_Interface
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
        public int DwellClickDelay ;
        public int OffGlassGlanceDist;
        public int MagnifierButtonPosX;
        public int MagnifierButtonPosY;
        public int MagnifierButtonWidth;
        public int MagnifierButtonHeight;
        public int G9ButtonPosX;
        public int G9ButtonPosY;
        public int G9ButtonWidth;
        public int G9ButtonHeight;
        public int G9DwellTime;

        public bool DoubleBuffered;
        public bool UseMagnifier;
        public bool UseG9Text;

        public int MagnifierWidth;
        public int MagnifierHeight;

        public bool MinimizeOnStart;

        public static readonly float ZOOM_FACTOR_MAX = 10.0f;
        public static readonly float ZOOM_FACTOR_MIN = 1.0f;
        public static readonly float ZOOM_FACTOR_DEFAULT = 3.0f;

        public static readonly float SPEED_FACTOR_MAX = 1.0f;
        public static readonly float SPEED_FACTOR_MIN = 0.05f;
        public static readonly float SPEED_FACTOR_DEFAULT = 0.35f;

        public Configuration()
        {
            
        }

        public float ZoomFactor
        {
            get { return mZoomFactor; }
            set
            {
                if (value > ZOOM_FACTOR_MAX)
                {
                    mZoomFactor = ZOOM_FACTOR_MAX;
                }
                else if (value < ZOOM_FACTOR_MIN)
                {
                    mZoomFactor = ZOOM_FACTOR_MIN;
                }
                else
                {
                    mZoomFactor = value;
                }
            }
        } 
        
        private float mZoomFactor = ZOOM_FACTOR_DEFAULT;

        public float SpeedFactor
        {
            get { return mSpeedFactor; }
            set
            {
                if (value > SPEED_FACTOR_MAX)
                {
                    mSpeedFactor = SPEED_FACTOR_MAX;
                }
                else if (value < SPEED_FACTOR_MIN)
                {
                    mSpeedFactor = SPEED_FACTOR_MIN;
                }
                else
                {
                    mSpeedFactor = value;
                }
            }
        } 
        
        private float mSpeedFactor = SPEED_FACTOR_DEFAULT;

        public int MagnifierWidthHeight
        {
            get
            {
                return MagnifierWidth;
            }
            set
            {
                MagnifierWidth = value;
                MagnifierHeight = value;
            }
        }
    }
}
