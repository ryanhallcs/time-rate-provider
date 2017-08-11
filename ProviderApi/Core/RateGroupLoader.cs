using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NodaTime;
using ProviderApi.Models;

namespace ProviderApi.Core
{
    public static class RateGroupLoader
    {
        public static RateGroup LoadFromJsonString(string rateGroupJson)
        {            
            var result = new RateGroup();
            
            // Setup all days of the week
            var allDayOfWeeks = Enum.GetValues(typeof(IsoDayOfWeek)).Cast<IsoDayOfWeek>().ToList();
            allDayOfWeeks.ForEach(day => result.DayRateRanges[day] = new List<TimeRangeRate>());

            var rawResult = JsonConvert.DeserializeObject<RawRateGroups>(rateGroupJson);
            foreach (var listedRange in rawResult.Rates)
            {
                var dayOfWeeks = TimeHelpers.DaysFromStringList(listedRange.Days);
                var times = listedRange.Times.Split('-');
                var beginTime = int.Parse(times[0]);
                var endTime = int.Parse(times[1]);
                
                foreach (var dayOfWeek in dayOfWeeks)
                {
                    result.DayRateRanges[dayOfWeek].Add(new TimeRangeRate(beginTime, endTime, listedRange.Price));
                }
            }

            return result;
        }
    }
}