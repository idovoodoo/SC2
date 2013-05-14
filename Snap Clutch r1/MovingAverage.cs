/*
Copyright (c) 2005, Marc Clifton
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list
  of conditions and the following disclaimer. 

* Redistributions in binary form must reproduce the above copyright notice, this 
  list of conditions and the following disclaimer in the documentation and/or other
  materials provided with the distribution. 
 
* Neither the name of MyXaml nor the names of its contributors may be
  used to endorse or promote products derived from this software without specific
  prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
// I HAVE MADE SOME MODIFICATIONS TO THE ORIGINAL SOURCE
// - Stephen Vickers
using System;

namespace SnapClutch
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
                    //throw new ApplicationException("Number of samples is 0.");
                }

                return total / samples.Count;
            }
        }

        // Constructor, initializing the sample size to the specified number.
        public MovingAverage(int numSamples)
        {
            if (numSamples < 0) // this was <= 0
            {
                throw new ArgumentOutOfRangeException("numSamples can't be negative or 0.");
            }

            samples = new CircularList<float>(numSamples);
            total = 0;
        }

        // Adds a sample to the sample collection.
        public void AddSample(float val)
        {
            //try
            //{
                if (samples.Count == samples.Length)
                {
                    total -= samples.Value;
                }

                samples.Value = val;
                total += val;
                samples.Next();
            //}
            //catch (Exception ec)
            //{
                //Console.WriteLine(ec.ToString());
            //}
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

