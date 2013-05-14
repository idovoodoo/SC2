using System;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch.G9CSharp
{

    public class G9WordCS : IComparable
    {
        public int[] key;

        public string word = null;

        public virtual int CompareTo(object o)
        {
            for (int i = 0; (i < this.key.Length) && (i < ((G9WordCS)o).key.Length); i++)
            {
                int val = this.key[i] - ((G9WordCS)o).key[i];
                if (val != 0)
                {
                    return val;
                }
            }
            return (((G9WordCS)o).key.Length - this.key.Length);
        }


        public override string ToString()
        {
            return this.ToString();
        }
    }
}









//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace SnapClutch.G9CSharp
//{

//    public class G9WordCS : IComparable
//    {
//        public int[] key;

//        public string word = null;

//        public virtual int CompareTo(object o)
//        {
//            for (int i = 0; (i < this.key.Length) && (i < ((G9WordCS)o).key.Length); i++)
//            {
//                int val = this.key[i] - ((G9WordCS)o).key[i];
//                if (val != 0)
//                {
//                    return val;
//                }
//            }
//            return (((G9WordCS)o).key.Length - this.key.Length);
//        }


//        public override string ToString()
//        {
//            return this.ToString();
//        }
//    }
//}








