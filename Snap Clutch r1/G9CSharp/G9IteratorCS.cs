using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SnapClutch.G9CSharp
{
    public class G9IteratorCS : IEnumerator
    {

        public int currentIndex;
        public int deepnessLevel;
        //public HashSet lastMatchedWord = new HashSet();
        public int levelBegin;
        public int levelEnd;
        public int storedIndex;
        public string tempWord;
        public G9DictionaryCS parentDictionary;
        public ArrayList lastMatchedWord = new ArrayList();

        /// <summary>
        /// Creates an iterator/enumerator for cycling through dictionary words
        /// </summary>
        /// <param name="parentDictionary"></param>
        public G9IteratorCS(G9DictionaryCS parentDictionary)
        {
            this.parentDictionary = parentDictionary;
            this.deepnessLevel = 0;
            this.levelBegin = 0;
            this.levelEnd = parentDictionary.wordList.Count;
            this.currentIndex = 0;
        }

        /// <summary>
        /// Cycles through the possible words
        /// </summary>
        /// <returns></returns>
        public object Cycle()
        {
            while (this.currentIndex < this.levelEnd)
            {
                string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;
                tempWord = s;
                if (this.deepnessLevel <= s.Length)
                {
                    string ss = s.Substring(0, this.deepnessLevel);
                    if (!this.lastMatchedWord.Contains(ss))
                    {
                        this.lastMatchedWord.Clear();
                        this.lastMatchedWord.Add(ss);
                        return ss;
                    }

                }
            }
            currentIndex = levelBegin;
            string s2 = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;

            string ss2 = s2.Substring(0, this.deepnessLevel);
            if (!this.lastMatchedWord.Contains(ss2))
            {
                this.lastMatchedWord.Clear();
                this.lastMatchedWord.Add(ss2);
                return ss2;
            }

            throw new IndexOutOfRangeException();
        }


        /// <summary>
        /// Adds a new letter
        /// </summary>
        /// <param name="key"></param>
        public void NextLevel(int key)
        {
            int i;
            this.lastMatchedWord.Clear();
            this.deepnessLevel++;
            for (i = this.levelBegin; i < this.levelEnd; i++)
            {
                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[i];
                if ((g9Word.key.Length > (this.deepnessLevel - 1)) && (g9Word.key[this.deepnessLevel - 1] == key))
                {
                    this.currentIndex = i;
                    this.levelBegin = i;
                    i = this.levelEnd;
                }
            }
            for (i = this.currentIndex; i < this.levelEnd; i++)
            {
                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[i];
                if ((g9Word.key.Length > (this.deepnessLevel - 1)) && (g9Word.key[this.deepnessLevel - 1] != key))
                {
                    this.levelEnd = i;
                    break;
                }
            }
            storedIndex = i;
            Console.WriteLine("stored index from next level is = " + storedIndex);
        }

        /// <summary>
        /// Goes back one letter
        /// </summary>
        public void PreviousLevel()
        {
            if (this.deepnessLevel <= 0)
            {
                this.currentIndex = 0;
            }
            else
            {
                int num = this.deepnessLevel - 1;
                if (num < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                int[] keys = new int[num];
                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[this.levelBegin];
                for (int i = 0; i < (this.deepnessLevel - 1); i++)
                {
                    keys[i] = g9Word.key[i];
                }
                this.deepnessLevel = 0;
                this.levelBegin = 0;
                this.levelEnd = this.parentDictionary.wordList.Count;
                this.currentIndex = 0;
                for (int i = 0; i < keys.Length; i++)
                {
                    this.NextLevel(keys[i]);
                }
            }
        }

        /// <summary>
        /// Moves to the next word
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (this.currentIndex < this.levelEnd)
            {
                int tempCurrentIndex = this.currentIndex;
                //Console.WriteLine("first index " + this.currentIndex);
                while (this.currentIndex < this.levelEnd)
                {
                    string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;
                    if (this.deepnessLevel <= s.Length)
                    {
                        //Console.WriteLine("incremental index " + this.currentIndex);
                        string ss = s.Substring(0, this.deepnessLevel);
                        if (!this.lastMatchedWord.Contains(ss))
                        {
                            this.currentIndex = tempCurrentIndex;
                            //Console.WriteLine("last index " + this.currentIndex);
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        public void Reset()
        {
        }

        /// <summary>
        /// Gets the current word
        /// </summary>
        public object Current
        {
            get
            {
                while (this.currentIndex < this.levelEnd)
                {


                    string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;

                    //Console.WriteLine("currentIndex = " + this.currentIndex + " levelEnd = " + this.levelEnd + " word = " + s);

                    if (this.deepnessLevel <= s.Length)
                    {
                        string ss = s.Substring(0, this.deepnessLevel);
                        if (!this.lastMatchedWord.Contains(ss))
                        {
                            this.lastMatchedWord.Add(ss);
                            return ss;
                        }
                    }
                }

                throw new NotImplementedException();
            }
            set
            { }
        }
    }
}





//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;

//namespace SnapClutch.G9CSharp
//{
//    public class G9IteratorCS : IEnumerator
//    {

//        public int currentIndex;
//        public int deepnessLevel;
//        //public HashSet lastMatchedWord = new HashSet();
//        public int levelBegin;
//        public int levelEnd;
//        public int storedIndex;
//        public string tempWord;
//        public G9DictionaryCS parentDictionary;
//        public ArrayList lastMatchedWord = new ArrayList();
        
//        /// <summary>
//        /// Creates an iterator/enumerator for cycling through dictionary words
//        /// </summary>
//        /// <param name="parentDictionary"></param>
//        public G9IteratorCS(G9DictionaryCS parentDictionary)
//        {
//            this.parentDictionary = parentDictionary;
//            this.deepnessLevel = 0;
//            this.levelBegin = 0;
//            this.levelEnd = parentDictionary.wordList.Count;
//            this.currentIndex = 0;
//        }

//        /// <summary>
//        /// Cycles through the possible words
//        /// </summary>
//        /// <returns></returns>
//        public object Cycle()
//        {
//            while (this.currentIndex < this.levelEnd)
//            {
//                string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;
//                tempWord = s;
//                if (this.deepnessLevel <= s.Length)
//                {
//                    string ss = s.Substring(0, this.deepnessLevel);
//                    if (!this.lastMatchedWord.Contains(ss))
//                    {
//                        this.lastMatchedWord.Clear();
//                        this.lastMatchedWord.Add(ss);
//                        return ss;
//                    }

//                }
//            }
//            currentIndex = levelBegin;
//            string s2 = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;
            
//            string ss2 = s2.Substring(0, this.deepnessLevel);
//            if (!this.lastMatchedWord.Contains(ss2))
//            {
//                this.lastMatchedWord.Clear();
//                this.lastMatchedWord.Add(ss2);
//                return ss2;
//            }

//            throw new IndexOutOfRangeException();
//        }


//        /// <summary>
//        /// Adds a new letter
//        /// </summary>
//        /// <param name="key"></param>
//        public void NextLevel(int key)
//        {
//            int i;
//            this.lastMatchedWord.Clear();
//            this.deepnessLevel++;
//            for (i = this.levelBegin; i < this.levelEnd; i++)
//            {
//                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[i];
//                if ((g9Word.key.Length > (this.deepnessLevel - 1)) && (g9Word.key[this.deepnessLevel - 1] == key))
//                {
//                    this.currentIndex = i;
//                    this.levelBegin = i;
//                    i = this.levelEnd;
//                }
//            }
//            for (i = this.currentIndex; i < this.levelEnd; i++)
//            {
//                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[i];
//                if ((g9Word.key.Length > (this.deepnessLevel - 1)) && (g9Word.key[this.deepnessLevel - 1] != key))
//                {
//                    this.levelEnd = i;
//                    break;
//                }
//            }
//            storedIndex = i;
//            Console.WriteLine("stored index from next level is = " + storedIndex);
//        }

//        /// <summary>
//        /// Goes back one letter
//        /// </summary>
//        public void PreviousLevel()
//        {
//            if (this.deepnessLevel <= 0)
//            {
//                this.currentIndex = 0;
//            }
//            else
//            {
//                int num = this.deepnessLevel - 1;
//                if (num < 0)
//                {
//                    throw new IndexOutOfRangeException();
//                }
//                int[] keys = new int[num];
//                G9WordCS g9Word = (G9WordCS)this.parentDictionary.wordList[this.levelBegin];
//                for (int i = 0; i < (this.deepnessLevel - 1); i++)
//                {
//                    keys[i] = g9Word.key[i];
//                }
//                this.deepnessLevel = 0;
//                this.levelBegin = 0;
//                this.levelEnd = this.parentDictionary.wordList.Count;
//                this.currentIndex = 0;
//                for (int i = 0; i < keys.Length; i++)
//                {
//                    this.NextLevel(keys[i]);
//                }
//            }
//        }

//        /// <summary>
//        /// Moves to the next word
//        /// </summary>
//        /// <returns></returns>
//        public bool MoveNext()
//        {
//            if (this.currentIndex < this.levelEnd)
//            {
//                int tempCurrentIndex = this.currentIndex;
//                //Console.WriteLine("first index " + this.currentIndex);
//                while (this.currentIndex < this.levelEnd)
//                {
//                    string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;
//                    if (this.deepnessLevel <= s.Length)
//                    {
//                        //Console.WriteLine("incremental index " + this.currentIndex);
//                        string ss = s.Substring(0, this.deepnessLevel);
//                        if (!this.lastMatchedWord.Contains(ss))
//                        {
//                            this.currentIndex = tempCurrentIndex;
//                            //Console.WriteLine("last index " + this.currentIndex);
//                            return true;
//                        }

//                    }
//                }
//            }
//            return false;
//        }

//        public void Reset()
//        {
//        }

//        /// <summary>
//        /// Gets the current word
//        /// </summary>
//        public object Current 
//        {
//            get 
//            {
//                while (this.currentIndex < this.levelEnd)
//                {


//                    string s = ((G9WordCS)this.parentDictionary.wordList[this.currentIndex++]).word;

//                    //Console.WriteLine("currentIndex = " + this.currentIndex + " levelEnd = " + this.levelEnd + " word = " + s);

//                    if (this.deepnessLevel <= s.Length)
//                    {
//                        string ss = s.Substring(0, this.deepnessLevel);
//                        if (!this.lastMatchedWord.Contains(ss))
//                        {
//                            this.lastMatchedWord.Add(ss);
//                            return ss;
//                        }
//                    }
//                }

//                throw new NotImplementedException();
//            }
//            set
//            { }
//        }
//    }
//}
