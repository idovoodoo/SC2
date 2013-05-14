using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gesture_Interface
{
    public class GestureCollection
    {
        List<Gesture> gList = new List<Gesture>();

        public GestureCollection()
        {
            Gesture sampleGest = new Gesture(9999);
            sampleGest.SetKey("SAMPLE");
            sampleGest.SetName("SAMPLE");
            gList.Add(sampleGest);
        }

        public void AddGesture(Gesture argGesture)
        {
            bool exists = false;
            Gesture tempGesture = new Gesture(9999);

            //Console.WriteLine("**** attempeting to add gesture {0} *****", argGesture.GetString());

            //check to see if the gesture is already in the list
            foreach(Gesture gesture in gList)
            {
                if (gesture.GetString().Equals(argGesture.GetString()))
                {
                    exists = true;
                    tempGesture = gesture;
                    //Console.WriteLine("**** found gesture {0} in list *****", gesture.GetString());
                }
            }

            if (exists)
            {
                //Console.WriteLine("**** removing gesture {0} because it is in the list *****", argGesture.GetString());
                gList.Remove(tempGesture);
            }

            gList.Add(argGesture);
           // Console.WriteLine("**** add gesture {0} *****", argGesture.GetString());

            
        }

        public Gesture GetGesture(string argGestureString)
        {
            Gesture tempGesture = new Gesture(9999);

            // get gesture from list
            foreach (Gesture gesture in gList)
            {
                if (gesture.GetString().ToString().Equals(argGestureString))
                {
                    tempGesture = gesture;
                    //Console.WriteLine("****it equals!!!! {0} *****", gesture.GetString());
                }
            }
            return tempGesture;
        }

        public void ExecuteGesture(string argGestureString)
        {
            foreach(Gesture gesture in gList)
            {
                if(gesture.GetString().ToString().Equals(argGestureString))
                {
                    gesture.Execute();
                    //Console.WriteLine("****it equals!!!! {0} *****", gesture.GetString());
                }
            }
        }

    }
}
