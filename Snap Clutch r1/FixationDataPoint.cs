using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SnapClutch
{
    public class FixationDataPoint
    {
        private Point fixationPoint = new Point();
        private TimeSpan fixationStartTime = new TimeSpan();
        private TimeSpan fixationEndTime = new TimeSpan();
        private int newFixStart;
        private int newFixEnd;
        private bool lastFixationInStartZone = false;

        public FixationDataPoint()
        {
        }

        public FixationDataPoint(Point argPoint, TimeSpan argStartTime)
        {
            fixationPoint = argPoint;
            fixationStartTime = argStartTime;
        }

        public FixationDataPoint(Point argPoint, TimeSpan argStartTime, TimeSpan argEndTime)
        {
            fixationPoint = argPoint;
            fixationStartTime = argStartTime;
            fixationEndTime = argEndTime;
        }

        public FixationDataPoint(Point argPoint, TimeSpan argStartTime, TimeSpan argEndTime, bool argLastInZone)
        {
            fixationPoint = argPoint;
            fixationStartTime = argStartTime;
            fixationEndTime = argEndTime;
            lastFixationInStartZone = argLastInZone;
        }

        public void SetFixationPoint(Point argPoint)
        {
            fixationPoint = argPoint;
        }

        public void SetAsLastFixationInStartZone(bool arg)
        {
            lastFixationInStartZone = arg;
        }

        public bool IsLastFixationInStartZone()
        {
            return lastFixationInStartZone;
        }

        public void SetFixationStartTime(TimeSpan argStartTime)
        {
            fixationStartTime = argStartTime;
        }

        public void SetFixationEndTime(TimeSpan argEndTime)
        {
            fixationEndTime = argEndTime;
        }

        public Point GetFixationPoint()
        {
            return fixationPoint;
        }

        public TimeSpan GetFixationStartTime()
        {
            return fixationStartTime;
        }

        public TimeSpan GetFixationEndTime()
        {
            return fixationEndTime;
        }

        public void SetNewFixStartTime(int start)
        {
            newFixStart = start;
        }

        public int GetNewFixStartTime()
        {
            return newFixStart;
        }

        public void SetNewFixEndTime(int end)
        {
            newFixEnd = end;
        }

        public int GetNewFixEndTime()
        {
            return newFixEnd;
        }

        public override string ToString()
        {
            return "Fixation point is: " + fixationPoint + " Fixation time is: " + fixationStartTime.TotalMilliseconds.ToString();
        }
    }
}
