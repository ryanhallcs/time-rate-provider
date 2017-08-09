using System;
using System.Collections.Generic;
using System.Linq;
using ProviderApi.Models;

namespace ProviderApi.Services
{
    public interface IGroupRateService
    {
        float? GetRateForRange(Guid groupId, TimeDay beginTime, TimeDay endTime);
    }

    public class GroupRateService : IGroupRateService
    {
        private readonly IGroupProvider RateGroupProvider;
        public GroupRateService(IGroupProvider rateGroupProvider)
        {
            RateGroupProvider = rateGroupProvider;
        }

        public float? GetRateForRange(Guid groupId, TimeDay beginTime, TimeDay endTime)
        {
            if (beginTime.DayOfWeek != endTime.DayOfWeek)
            {
                throw new ArgumentException($"Days do not match for {beginTime} and {endTime}");
            }

            var rateGroup = RateGroupProvider.GetGroup(groupId);
            var dayOfWeek = beginTime.DayOfWeek;

            // This is linear in time. If we expect groups to have many rates for each day, 
            // a better data structure could be used to get O(log n) time on this search.
            // e.g. A type of Trie in the form of [Day][Hour][Minute] => rate could be 
            // pre-computed for a given group
            if (!rateGroup.DayRateRanges.TryGetValue(dayOfWeek, out List<TimeRangeRate> dayRates))
            {
                return null; // unavailable
            }

            // Strictly greater and lesser than based on requirements of full encapsulation
            var result = dayRates.SingleOrDefault(dayRate => 
                dayRate.BeginTime < beginTime.TimeOfDay && dayRate.EndTime > endTime.TimeOfDay);

            return result?.Price;
        }
    }
}