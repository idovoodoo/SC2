using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gesture_Interface
{
    public class Gesture
    {
        string name;
        int gestureString;
        string key = "";

        public Gesture(int argString)
        {
            gestureString = argString;
        }

        public void SetName(string argName)
        {
            name = argName;
        }

        public string GetName()
        {
            return name;
        }

        public void SetString(int argString)
        {
            gestureString = argString;
        }
        
        public int GetString()
        {
            return gestureString;
        }

        public void SetKey(string argKey)
        {
            key = argKey;
        }

        public string GetKey()
        {
            return key;
        }

        public void Execute()
        {
            KeyEvent.ExecuteKeyCode(name);
        }
    }
}
