using System;
using System.Diagnostics;

namespace JankWorks.Game
{
    internal sealed class Counter
    {
        private Stopwatch stopwatch;
        private TimeSpan period;

        public int Frequency { get => this.freq; }

        private int counter;
        private int freq;

        public Counter(TimeSpan period)
        {
            this.period = period;
            this.counter = 0;
            this.freq = 0;
            this.stopwatch = new Stopwatch();
        }

        public void Start()
        {
            this.stopwatch.Restart();
        }

        public void Count()
        {
            if (this.stopwatch.Elapsed >= this.period)
            {
                freq = counter;
                this.stopwatch.Restart();
                this.counter = 0;
            }
            else
            {
                this.counter++;
            }
        }
    }
}
