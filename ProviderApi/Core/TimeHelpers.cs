using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using NodaTime.Text;
using ProviderApi.Models;

namespace ProviderApi.Core
{
    public static class TimeHelpers
    {
        public static readonly List<IsoDayOfWeek> AllDayOfWeeks = new List<IsoDayOfWeek> 
        {
            IsoDayOfWeek.Monday,
            IsoDayOfWeek.Tuesday,
            IsoDayOfWeek.Wednesday,
            IsoDayOfWeek.Thursday,
            IsoDayOfWeek.Friday,
            IsoDayOfWeek.Saturday,
            IsoDayOfWeek.Sunday
        };
        public static List<IsoDayOfWeek> DaysFromStringList(string list)
        {
            return list.Split(',').Select(s => DayOfWeekFromString(s)).ToList();    
        }
        
        public static InstantPattern InvariantParser = InstantPattern.CreateWithInvariantCulture("g");

        public static TimeDay GetUtcTimeDayFromString(string dateTime)
        {
            var zonedDateTime = GetUtcDateTimeFromString(dateTime);

            if (zonedDateTime == null)
            {
                return null;
            }

            return GetTimeDayFromZonedDateTime(zonedDateTime.Value);
        }

        public static ZonedDateTime? GetUtcDateTimeFromString(string dateTime)
        {
            var instant = InvariantParser.Parse(dateTime);
            if (!instant.Success)
            {
                return null;
            }

            return instant.Value.InUtc();
        }

        public static TimeDay GetTimeDayFromZonedDateTime(ZonedDateTime zonedDateTime)
        {
            var hour = zonedDateTime.Hour;
            var minute = zonedDateTime.Minute;

            var result = new TimeDay
            {
                DayOfWeek = zonedDateTime.DayOfWeek,
                TimeOfDay = hour * 100 + minute
            };

            return result;
        }

        public static IsoDayOfWeek DayOfWeekFromString(string commonName)
        {
            switch (commonName) 
            {
                case "mon":
                    return IsoDayOfWeek.Monday;
                case "tues":
                    return IsoDayOfWeek.Tuesday;
                case "wed":
                    return IsoDayOfWeek.Wednesday;
                case "thurs":
                    return IsoDayOfWeek.Thursday;
                case "fri":
                    return IsoDayOfWeek.Friday;
                case "sat":
                    return IsoDayOfWeek.Saturday;
                case "sun":
                    return IsoDayOfWeek.Sunday;
                default:
                    throw new ArgumentException($"Could not determine day of week from {commonName}");
            }
        }
    }
}