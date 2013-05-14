using System;

namespace Gesture_Interface
{
    public class MovingAverage
    {
        CircularList<float> samples;
        protected float total;

        // Get the average for the current number of samples.
        public float Average
        {
            get
            {
                if (samples.Count == 0)
                {
                    throw new ApplicationException("Number of samples is 0.");
                }

                return total / samples.Count;
            }
        }

        // Constructor, initializing the sample size to the specified number.
        public MovingAverage(int numSamples)
        {
            if (numSamples <= 0)
            {
                throw new ArgumentOutOfRangeException("numSamples can't be negative or 0.");
            }

            samples = new CircularList<float>(numSamples);
            total = 0;
        }

        // Adds a sample to the sample collection.
        public void AddSample(float val)
        {
            if (samples.Count == samples.Length)
            {
                total -= samples.Value;
            }

            samples.Value = val;
            total += val;
            samples.Next();
        }

        // Clears all samples to 0.
        public void ClearSamples()
        {
            total = 0;
            samples.Clear();
        }

        // Initializes all samples to the specified value.
        public void InitializeSamples(float v)
        {
            samples.SetAll(v);
            total = v * samples.Length;
        }
    }
}

