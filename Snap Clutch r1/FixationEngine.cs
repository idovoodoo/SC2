/* based on santtu's class!!!
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace SnapClutch
{
    public class FixationEngine
    {
        //Fixation testing related:
        int xTreshold = 0;  //for 35cm x 27cm screen, a 2,5cm treshold is 7,3% for horizontal and 10% for vertical size
        int yTreshold = 0;   //the persentage is set on constructor
        int fixationBufferSize = 5; // Minumum number of consecutive points within treshold area that can form a fixation.

        int fixations = 1; // == fixations found
        int[,] pointArray;
        int index = 0; //index pointer to pointArray
        int pointsInArray = 0;
        float xAvg, yAvg, startX, startY;
        bool isFixated = false;
        bool arrayFull = false;
        bool startSet = false;
        bool isStillFixated = false;
        //Point test related:
        TimeSpan fixationStartTime = new TimeSpan();
        TimeSpan fixationEndTime = new TimeSpan();
        int newFixStartTime;
        int newFixEndTime;
        FixationDataPoint myFixationDataPoint = new FixationDataPoint();
        //int gazeTimeStamp;

        /// <summary>
        /// Constructor for class.
        /// </summary>
        public FixationEngine()
        {
            pointArray = new int[fixationBufferSize, 7]; //x, y, day, hour, minute, second, millisecond
            xTreshold = 40; // (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.03 * 0.5);
            yTreshold = 40; // (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.038 * 0.5);

            //silly formattings here only :)
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            string day = DateTime.Now.Day.ToString();
            if (day.Length == 1)
                day = "0" + day;
            string hour = DateTime.Now.Hour.ToString();
            if (hour.Length == 1)
                hour = "0" + hour;
            string minute = DateTime.Now.Minute.ToString();
            if (minute.Length == 1)
                minute = "0" + minute;
            string second = DateTime.Now.Second.ToString();
            if (second.Length == 1)
                second = "0" + second;



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">x coordinate of the point</param>
        /// <param name="y">y coordinate of the point</param>
        /// <returns></returns>
        public FixationDataPoint testPoint(int x, int y, int timestamp)
        {
            //gazeTimeStamp = timestamp;

            //tests fixation and returns true if the fifth point in array creates fixation
            if (testFixation(x, y, timestamp))
            {
                return myFixationDataPoint;
            }
            return null;
        }

        /// <summary>
        /// Overload to allow float arguments. (Does convert to int and treat as such.)
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <returns> True if </returns>
        public FixationDataPoint testPoint(float x, float y, int timestamp)
        {
            //gazeTimeStamp = timestamp;

            if (testFixation((int)x, (int)y, (int)timestamp))
            {
                return myFixationDataPoint;
            }
            return null;

        }





        #region Fixation testing
        /// <summary>
        /// Tests if x, y coordinates form a fixation over time.
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <returns>True if Z many points are in R treshold.</returns>
        private bool testFixation(int x, int y, int timestamp)
        {
            if (!arrayFull)
            {
                //set point with time to array
                setPointToArray(x, y, timestamp);
                //if array is full now
                if ((index + 1) == fixationBufferSize)
                {
                    pointsInArray = 5;
                    countAverage();
                    bool temp = inTreshold();
                    arrayFull = true;
                    return temp; //was temp;
                }
            }

            //arrayFull
            else
            {
                if (!isFixated)
                {
                    setPointToArray(x, y, timestamp);
                    pointsInArray = 5;
                    countAverage();
                    bool tresh = inTreshold();

                    //    //test if fixation is now set
                    return inTreshold(); //was inTreshold();

                }

                //is fixated in previous iteration and new point does not fit within treshold
                if (!inTreshold(x, y))
                {

                    TimeSpan now = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                    TimeSpan duration = now.Subtract(fixationStartTime);

                    //setPointToArray(x, y);

                    //fixationEndTime = new TimeSpan(pointArray[index, 2],  //day
                    //             pointArray[index, 3],  //hour
                    //             pointArray[index, 4],  //minute
                    //             pointArray[index, 5],  //second
                    //             pointArray[index, 6]);  //millisecond
                    fixationEndTime = now;

                    //Console.WriteLine(fixations + "\tts:" + fixationStartTime.Hours + ":" +
                    //                     formatDigits(fixationStartTime.Minutes, 2) + ":" +
                    //                     formatDigits(fixationStartTime.Seconds, 2) + "," +
                    //                     formatDigits(fixationStartTime.Milliseconds, 3) +
                    //          "\t te:" + formatDigits(now.Hours, 2) + ":" +
                    //                     formatDigits(now.Minutes, 2) + ":" +
                    //                     formatDigits(now.Seconds, 2) + "," +
                    //                     formatDigits(now.Milliseconds, 3) + "\t" +
                    //                     (int)xAvg + "\t" + (int)yAvg + " Starting XY: " + startX + "\t" + startY + "\t z");

                    //myFixationDataPoint.SetFixationStartTime(fixationStartTime);
                    myFixationDataPoint.SetFixationEndTime(fixationEndTime);
                    //newFixEndTime = pointArray[index, 7];
                    //myFixationDataPoint.SetNewFixEndTime(newFixEndTime);
                    //myFixationDataPoint.SetFixationPoint(new System.Drawing.Point((int)xAvg, (int)yAvg));

                    //fixations++;
                    //sw.WriteLine("-----------Last array for fixation above --------------");
                    yAvg = xAvg = 0;
                    index = 0;
                    pointsInArray = 0;
                    isFixated = false;
                    startSet = false;
                    arrayFull = false;
                    isStillFixated = false;

                    return true; //was false;
                }
                else //fixated in previous, and new point is within treshold
                {
                    if (!isStillFixated)
                    {
                        isStillFixated = true;

                        return true; // was nothing
                    }
                    setPointToArray(x, y, timestamp);
                    countAverage();
                    //index = (index+1) % fixationBufferSize;
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets point and its timetag to array with Last in Last out principle. 
        /// </summary>
        /// <param name="x">X coord of the new point that goes into the array.</param>
        /// <param name="y">Y coord of the new point that goes into the array.</param>
        private void setPointToArray(int x, int y, int timestamp)
        {
            int a = 0;  //index out of bounds -crash debugging index 

            try
            {
                //set points to array. [x, y, time now seconds, time now milliseconds]
                pointArray[index, 0] = x; a++;
                pointArray[index, 1] = y; a++;
                pointArray[index, 2] = DateTime.Now.Day; a++;
                pointArray[index, 3] = DateTime.Now.Hour; a++;
                pointArray[index, 4] = DateTime.Now.Minute; a++;
                pointArray[index, 5] = DateTime.Now.Second; a++;
                pointArray[index, 6] = DateTime.Now.Millisecond; a++;
                //pointArray[index, 7] = timestamp;

                pointsInArray++; a++;
                index = (index + 1) % fixationBufferSize; a++;
            }
            catch (Exception e)
            {
                //StreamWriter boom = new StreamWriter("GR_boom_e." + DateTime.Now.Ticks + ".txt");
                //boom.WriteLine("Setpoint crashed. Index a: " + a + " with xy: " + x + "," + y + " with e: " + e + " WITH INDEX: " + index);
                //boom.Close();
            }
        }

        /// <summary>
        /// Counts the average for X and Y points that are in the array
        /// </summary>
        private void countAverage()
        {
            int sumX = 0;
            int sumY = 0;

            for (int i = 0; i < fixationBufferSize; i++)
            {
                sumX += pointArray[i, 0];
                sumY += pointArray[i, 1];
            }
            //sw.Write(" Counting AVG! SumXY: "+sumX+", "+sumY+". PointsInArray: "+pointsInArray+". fix buff size: "+fixationBufferSize);
            xAvg = (sumX + (xAvg * (pointsInArray - fixationBufferSize))) / pointsInArray;
            yAvg = (sumY + (yAvg * (pointsInArray - fixationBufferSize))) / pointsInArray;
            //sw.Write(" Counted AVG XY: " + xAvg + ", " + yAvg + " > ");
            if (xAvg == 0 && yAvg == 0)
                isFixated = false;
        }

        /// <summary>
        /// Checks if all points in Array are withing treshold. If even one is not in the treshold it will return false.
        /// Handles timer for fixation duration and starts fixation stuff.
        /// Note: Array Index must have been added by 1 after point adding and before running this.
        /// </summary>
        /// <returns>True if all points are within treshold.</returns>
        private bool inTreshold()
        {
            //for x
            for (int i = 0; i < fixationBufferSize; i++)
                if (Math.Abs(pointArray[i, 0] - xAvg) > xTreshold)
                {
                    //if point is too far from treshold
                    isFixated = false;
                    return false;
                }
            //for y
            for (int i = 0; i < fixationBufferSize; i++)
                if (Math.Abs(pointArray[i, 1] - yAvg) > yTreshold)
                {
                    //if point is too far from treshold
                    isFixated = false;
                    return isFixated;
                }
            if (xAvg == 0 && yAvg == 0)
            {
                isFixated = false;
                return false;
            }

            //tp.Write(" >Fixation ok for treshold. ");
            if (!startSet)
            {

                fixationStartTime = new TimeSpan(pointArray[index, 2],  //day
                                                 pointArray[index, 3],  //hour
                                                 pointArray[index, 4],  //minute
                                                 pointArray[index, 5],  //second
                                                 pointArray[index, 6]);  //millisecond

                //newFixStartTime = pointArray[index, 7];

                //Console.WriteLine("fixation start time = " + fixationStartTime);
                //Console.WriteLine("start of fixation! " + fixationStartTime.ToString());
                startSet = true;
                startX = (int)xAvg;
                startY = (int)yAvg;
                fixations++;
                //myFixationDataPoint.SetNewFixStartTime(newFixStartTime);
                myFixationDataPoint.SetFixationStartTime(fixationStartTime);
                myFixationDataPoint.SetFixationPoint(new System.Drawing.Point((int)startX, (int)startY));

                isFixated = true;
                return true;
            }

            isFixated = true;
            return false;
        }

        /// <summary>
        /// Tests if given X and Y coords fit inside treshold, without setting the point into array.
        /// Does not start any timers.
        /// </summary>
        /// <param name="x">X coord for test point.</param>
        /// <param name="y">Y coord for test point.</param>
        /// <returns></returns>
        private bool inTreshold(int x, int y)
        {
            if (Math.Abs(x - xAvg) > xTreshold)
                return false;
            if (Math.Abs(y - yAvg) > yTreshold)
                return false;
            return true;
        }


        ///// <summary>
        ///// Prints out the raw point array using specific initialized writer to be used for printout.
        ///// </summary>
        ///// <param name="O"></param>
        //private void printArray(StreamWriter O)
        //{
        //    O.Write("\r\nX: ");
        //    for (int i = 0; i < fixationBufferSize; i++)
        //        O.Write("[" + pointArray[i, 0] + "]");
        //    O.WriteLine(" avg: " + xAvg + " Index: " + index + " (after point added)");
        //    O.Write("Y: ");
        //    for (int i = 0; i < fixationBufferSize; i++)
        //        O.Write("[" + pointArray[i, 1] + "]");
        //    O.WriteLine(" avg: " + yAvg);

        //}
        #endregion

        ///// <summary>
        ///// Finalizes the writes on streamWriters and closes them.
        ///// </summary>
        //public void close()
        //{
        //    F.Close();  //dispose == if you don't use the writer again, close if you might use writer again
        //    zoneTest.close();
        //    gestureCollection.close();
        //    GSWriter.Close();
        //    runSound.Close();


        //}


        /// <summary>
        /// Formats given int to have three digits. int "7" -> string "007"
        /// </summary>
        /// <param name="stuff">Int that needs to be formatted.</param>
        /// <param name="digits">Number of desired digits.</param>
        /// <returns> 'Digits' length string of the given int, filled with zeroes in front to fill up. </returns>
        private string formatDigits(int stuff, int digits)
        {
            string thing = stuff.ToString();
            while (thing.Length < digits)
                thing = "0" + thing;
            return thing;
        }



    }
}
