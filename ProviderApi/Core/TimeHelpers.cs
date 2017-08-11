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

            var result = new TimeDay
            {
                DayOfWeek = zonedDateTime.Value.DayOfWeek,
                TimeOfDay = zonedDateTime.Value.Hour * 100 + zonedDateTime.Value.Minute
            };

            return result;
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