using System;

namespace Common
{
    public class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public Time()
        {
            
        }

        public Time(int Hour, int Minute)
        {
            this.Hour = Hour%61;
            this.Minute = Minute % 61;
        }

        public Time(int Hour, int Minute, int Second) : this(Hour, Minute)
        {
            this.Second = Second % 61;
        }

        public override string ToString()
        {
            return $"{Hour}:{Minute}:{Second}";
        }
    }
}
