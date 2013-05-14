using System;
using System.Collections;
using System.Collections.Generic;

namespace Gesture_Interface
{
    public class CircularList<T> : IEnumerable<T>, IEnumerator<T>
    {
        protected T[] items;
        protected int idx;
        protected bool loaded;
        protected int enumIdx;

        // Constructor that initializes the list with the 
        // required number of items.
        public CircularList(int numItems)
        {
            if (numItems <= 0)
            {
                throw new ArgumentOutOfRangeException("numItems can't be negative or 0.");
            }

            items = new T[numItems];
            idx = 0;
            loaded = false;
            enumIdx = -1;
        }

        // Gets/sets the item value at the current index.
        public T Value
        {
            get { return items[idx]; }
            set { items[idx] = value; }
        }

        // Returns the count of the number of loaded items, up to
        // and including the total number of items in the collection.
        public int Count
        {
            get { return loaded ? items.Length : idx; }
        }

        // Returns the length of the items array.
        public int Length
        {
            get { return items.Length; }
        }

        // Gets/sets the value at the specified index.
        public T this[int index]
        {
            get
            {
                RangeCheck(index);
                return items[index];
            }
            set
            {
                RangeCheck(index);
                items[index] = value;
            }
        }

        // Advances to the next item or wraps to the first item.
        public void Next()
        {
            if (++idx == items.Length)
            {
                idx = 0;
                loaded = true;
            }
        }

        // Clears the list, resetting the current index to the 
        // beginning of the list and flagging the collection as unloaded.
        public void Clear()
        {
            idx = 0;
            items.Initialize();
            loaded = false;
        }

        // Sets all items in the list to the specified value, resets
        // the current index to the beginning of the list and flags the
        // collection as loaded.
        public void SetAll(T val)
        {
            idx = 0;
            loaded = true;

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = val;
            }
        }

        // Internal indexer range check helper.  Throws
        // ArgumentOutOfRange exception if the index is not valid.
        protected void RangeCheck(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Indexer cannot be less than 0.");
            }

            if (index >= items.Length)
            {
                throw new ArgumentOutOfRangeException("Indexer cannot be greater than or equal to the number if items in the collection.");
            }
        }

        // Interface implementations:
        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public T Current
        {
            get { return this[enumIdx]; }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return this[enumIdx]; }
        }

        public bool MoveNext()
        {
            ++enumIdx;
            bool ret = enumIdx < Count;

            if (!ret)
            {
                Reset();
            }

            return ret;
        }

        public void Reset()
        {
            enumIdx = -1;
        }
    }
}
