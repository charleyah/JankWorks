using System;
using System.Diagnostics;

using JankWorks.Game.Platform;

namespace JankWorks.Game
{
    // Poor Mans Mixin
    internal interface IRunner<in S, in I>
    {
        Stopwatch Timer { get; }

        TimeSpan TotalElapsed { get; set; }

        TimeSpan TargetElapsed { get; }

        TimeSpan Accumulated { get; set; }

        long LastRunTick { get; set; }

        void BeginRun()
        {
            this.TotalElapsed = TimeSpan.Zero;
            this.LastRunTick = 0;
            this.Timer.Restart();
        }

        void StopRun() => this.Timer.Stop();


        TimeSpan Run(S simulateState, I interpolateState)
        {
            TimeSpan processTime;
            long currentTick;
            long lastTick = this.LastRunTick;
            TimeSpan total = this.TotalElapsed;
            TimeSpan target = this.TargetElapsed;
            TimeSpan accumulated = this.Accumulated;
            var updateCount = 0;
            
            retry:

            currentTick = this.Timer.ElapsedTicks;

            TimeSpan since = TimeSpan.FromTicks(currentTick - lastTick);
            lastTick = currentTick;

            accumulated += since;
           
            if(accumulated < target)
            {
                var until = target - accumulated;

                if(until > TimeSpan.FromMilliseconds(2))
                {
                    PlatformApi.Instance.Sleep(until);
                }
                
                goto retry;
            }
            else
            {
                var startUpdate = this.Timer.ElapsedTicks;
                do
                {
                    accumulated -= target;
                    total += target;

                    this.Simulate(new GameTime(total, target), simulateState);
                    updateCount++;
                }
                while (accumulated >= target);

                processTime = TimeSpan.FromTicks(this.Timer.ElapsedTicks - startUpdate);                
            }

            var delta = TimeSpan.FromTicks(target.Ticks * updateCount);

            this.Interpolate(new GameTime(total, delta), interpolateState);

            this.LastRunTick = currentTick;
            this.TotalElapsed = total;            
            this.Accumulated = accumulated;

            return processTime;
        }

        void Simulate(GameTime time, S state);

        void Interpolate(GameTime time, I state);
    }
}
