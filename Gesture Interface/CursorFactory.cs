using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gesture_Interface
{

    public class CursorFactory
    {
        [DllImport("user32.dll", EntryPoint = "LoadCursorFromFileW",
             CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadCursorFromFile(String str);

        [DllImport("user32.dll")]
        static extern bool SetSystemCursor(IntPtr hcur, uint id);

        private const uint OCR_NORMAL = 32512;

        public static Cursor Create(string filename)
        {
            IntPtr hCursor;
            Cursor result = null;

            try
            {
                hCursor = LoadCursorFromFile(filename);
                if (!IntPtr.Zero.Equals(hCursor))
                {
                    result = new Cursor(hCursor);
                    bool ret_val = SetSystemCursor(hCursor, OCR_NORMAL);
                }
                else
                {
                    throw new ApplicationException("Could not create cursor from file " + filename);
                }
            }
            catch
            {
                //log exception
            }

            return result;
        }
    }
}
