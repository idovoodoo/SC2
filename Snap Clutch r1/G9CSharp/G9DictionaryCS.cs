using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace SnapClutch.G9CSharp
{
    public class G9DictionaryCS
    {

        public ArrayList wordList = new ArrayList();
        StreamReader objReader;

        public G9DictionaryCS()
        {
            try
            {
                objReader = new StreamReader("500words.txt");
                Console.WriteLine("dictionary loaded");
                LoadDictionary();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("file not found");
            }

        }

        private void LoadDictionary()
        {
            string sLine = null;

            while ((sLine = objReader.ReadLine()) != null)
            {

                this.AddWord(sLine);

            }

            objReader.Close();

            SortList();

            //foreach (string wordOutput in wordList)
            //    Console.WriteLine(wordOutput);
        }

        public void SortList()
        {

            wordList.Sort();
        }

        public void AddWord(string word)
        {
            word = word.ToLower();

            G9WordCS newWord = new G9WordCS();
            newWord.word = word;
            int num = word.Length;
            if (num < 0)
            {
                throw new IndexOutOfRangeException();
                //throw new NotImplementedException();
            }

            newWord.key = new int[num];
            for (int i = 0; i < word.Length; i++)
            {
                newWord.key[i] = this.GetKey(word[i]);
            }

            this.wordList.Add(newWord);
        }


        public int GetKey(char c)
        {
            int num = 0x1a;
            if (num < 0)
            {
                throw new IndexOutOfRangeException();
                //throw new NotImplementedException();
            }

            int[] numArray1 = new int[num];
            numArray1[0] = 2;
            numArray1[1] = 2;
            numArray1[2] = 2;
            numArray1[3] = 3;
            numArray1[4] = 3;
            numArray1[5] = 3;
            numArray1[6] = 4;
            numArray1[7] = 4;
            numArray1[8] = 4;
            numArray1[9] = 5;
            numArray1[10] = 5;
            numArray1[11] = 5;
            numArray1[12] = 6;
            numArray1[13] = 6;
            numArray1[14] = 6;
            numArray1[15] = 7;
            numArray1[0x10] = 7;
            numArray1[0x11] = 7;
            numArray1[0x12] = 7;
            numArray1[0x13] = 8;
            numArray1[20] = 8;
            numArray1[0x15] = 8;
            numArray1[0x16] = 9;
            numArray1[0x17] = 9;
            numArray1[0x18] = 9;
            numArray1[0x19] = 9;

            int[] letters = numArray1;

            if (((c - 'a') < letters.Length) && ((c - 'a') >= 0))
            {
                return letters[c - 'a'];
            }
            switch (c)
            {
                case '\x00e4':
                case '\x00e5':
                    return 2;

                case '\x00f6':
                    return 6;
            }
            return 0;
        }

        public string SearchFullWord(int[] keys)
        {
            IEnumerator ienum = this.wordList.GetEnumerator();

            while (ienum.MoveNext())
            {
                G9WordCS csword = (G9WordCS)ienum.Current;
                if (keys.Equals(csword))
                {
                    return csword.word;
                }
            }
            return null;
        }


        public G9IteratorCS Iterator()
        {
            return new G9IteratorCS(this);
        }
    }
}




//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;


//namespace SnapClutch.G9CSharp
//{
//    public class G9DictionaryCS
//    {

//        public ArrayList wordList = new ArrayList();
//        StreamReader objReader;

//        public G9DictionaryCS()
//        {
//            try
//            {
//                objReader = new StreamReader("wordlist.txt");
//                Console.WriteLine("dictionary loaded");
//                LoadDictionary();
//            }
//            catch (FileNotFoundException)
//            {
//                Console.WriteLine("file not found");
//            }

//        }

//        private void LoadDictionary()
//        {
//            string sLine = null;

//            while ((sLine = objReader.ReadLine()) != null)
//            {

//                this.AddWord(sLine);

//            }

//            objReader.Close();

//            SortList();

//            //foreach (string wordOutput in wordList)
//            //    Console.WriteLine(wordOutput);
//        }

//        public void SortList()
//        {
            
//            wordList.Sort();
//        }

//        public void AddWord(string word)
//        {
//            word = word.ToLower();

//            G9WordCS newWord = new G9WordCS();
//            newWord.word = word;
//            int num = word.Length;
//            if (num < 0)
//            {
//                throw new IndexOutOfRangeException();
//                //throw new NotImplementedException();
//            }

//            newWord.key = new int[num];
//            for (int i = 0; i < word.Length; i++)
//            {
//                newWord.key[i] = this.GetKey(word[i]);
//            }

//            this.wordList.Add(newWord);
//        }


//        public int GetKey(char c)
//        {
//            int num = 0x1a;
//            if (num < 0)
//            {
//                throw new IndexOutOfRangeException();
//                //throw new NotImplementedException();
//            }

//            int[] numArray1 = new int[num];
//            numArray1[0] = 2;
//            numArray1[1] = 2;
//            numArray1[2] = 2;
//            numArray1[3] = 3;
//            numArray1[4] = 3;
//            numArray1[5] = 3;
//            numArray1[6] = 4;
//            numArray1[7] = 4;
//            numArray1[8] = 4;
//            numArray1[9] = 5;
//            numArray1[10] = 5;
//            numArray1[11] = 5;
//            numArray1[12] = 6;
//            numArray1[13] = 6;
//            numArray1[14] = 6;
//            numArray1[15] = 7;
//            numArray1[0x10] = 7;
//            numArray1[0x11] = 7;
//            numArray1[0x12] = 7;
//            numArray1[0x13] = 8;
//            numArray1[20] = 8;
//            numArray1[0x15] = 8;
//            numArray1[0x16] = 9;
//            numArray1[0x17] = 9;
//            numArray1[0x18] = 9;
//            numArray1[0x19] = 9;

//            int[] letters = numArray1;

//            if (((c - 'a') < letters.Length) && ((c - 'a') >= 0))
//            {
//                return letters[c - 'a'];
//            }
//            switch (c)
//            {
//                case '\x00e4':
//                case '\x00e5':
//                    return 2;

//                case '\x00f6':
//                    return 6;
//            }
//            return 0;
//        }

//        public string SearchFullWord(int[] keys)
//        {
//            IEnumerator ienum = this.wordList.GetEnumerator();
            
//            while (ienum.MoveNext())
//            {
//                G9WordCS csword = (G9WordCS) ienum.Current;
//                if (keys.Equals(csword))
//                {
//                    return csword.word;
//                }
//            }
//            return null;
//        }


//        public G9IteratorCS Iterator()
//        {
//            return new G9IteratorCS(this);
//        }
//    }
//}
