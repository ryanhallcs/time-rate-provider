using System.Collections.Generic;
using NodaTime;

namespace ProviderApi.Models
{
    public class TimeRangeRate
    {
        public TimeRangeRate() { }
        public TimeRangeRate(int beginTime, int endTime, float price)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            Price = price;
        }

        public int BeginTime { get; set; }
        public int EndTime { get; set; }
        public float Price { get; set; }
    }

    public class TimeDay
    {
        public IsoDayOfWeek DayOfWeek { get; set; }
        public int TimeOfDay { get; set; }

        public override string ToString()
        {
            return $"{DayOfWeek} {TimeOfDay}";
        }
    }
}