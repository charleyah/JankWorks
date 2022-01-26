using System;
using System.Numerics;

namespace JankWorks.Game
{
    public readonly struct GameTime
    {
        public readonly struct TotalTime : IEquatable<TotalTime>
        {
            private readonly TimeSpan value;

            public TotalTime(TimeSpan value)
            {
                this.value = value;
            }

            public override string ToString() => this.value.ToString();

            public override int GetHashCode() => this.value.GetHashCode();

            public override bool Equals(object obj) => obj is TotalTime other && this.Equals(other);

            public bool Equals(TotalTime other) => this.value == other.value;

            public static bool operator ==(TotalTime left, TotalTime right) => left.Equals(right);

            public static bool operator !=(TotalTime left, TotalTime right) => !left.Equals(right);

            public static bool operator >(TotalTime left, TotalTime right) => left.value > right.value;

            public static bool operator <(TotalTime left, TotalTime right) => left.value < right.value;

            public static implicit operator TimeSpan(TotalTime time) => time.value;

            public static implicit operator float(TotalTime time) => Convert.ToSingle((double)time);

            public static implicit operator double(TotalTime time) => time.value.TotalMilliseconds;
        }

        public readonly struct DeltaTime : IEquatable<DeltaTime>
        {                       
            private readonly double value;

            public DeltaTime(TimeSpan value)
            {
                this.value = value.TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds;
            }

            public override string ToString() => this.value.ToString();            

            public override int GetHashCode() => this.value.GetHashCode();

            public override bool Equals(object obj) => obj is DeltaTime other && this.Equals(other);

            public bool Equals(DeltaTime other) => this.value == other.value;
           
            public static bool operator ==(DeltaTime left, DeltaTime right) => left.Equals(right);

            public static bool operator !=(DeltaTime left, DeltaTime right) => !left.Equals(right);

            public static bool operator >(DeltaTime left, DeltaTime right) => left.value > right.value;

            public static bool operator <(DeltaTime left, DeltaTime right) => left.value < right.value;

            public static implicit operator TimeSpan(DeltaTime time) => TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds * time.value);

            public static implicit operator Vector2(DeltaTime time) => new Vector2(time);

            public static implicit operator Vector3(DeltaTime time) => new Vector3(time);

            public static implicit operator float(DeltaTime time) => Convert.ToSingle(time.value);

            public static implicit operator double(DeltaTime time) => time.value;
        }

        public readonly TotalTime Total;

        public readonly DeltaTime Delta;

        public GameTime(TimeSpan total, TimeSpan delta) : this(new TotalTime(total), new DeltaTime(delta)) { }

        public GameTime(TotalTime total, DeltaTime delta)
        {
            this.Total = total;
            this.Delta = delta;
        }
    }
}
