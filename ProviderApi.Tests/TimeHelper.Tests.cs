using System;
using NodaTime;
using ProviderApi.Core;
using Xunit;

namespace ProviderApi.Tests
{
    public class TimeHelperTests
    {
        [Fact]
        public void BadDateStringReturnsNullTimeDay()
        {
            var result = TimeHelpers.GetUtcTimeDayFromString("bad date string");
            Assert.Null(result);
        }

        [Fact]
        public void GetTimeDayReturnsCorrectUtcTime()
        {
            var dateString = "2017-08-09T10:00:00Z"; // Weds, Aug 9th, 2017 10 AM UTC
            var result = TimeHelpers.GetUtcTimeDayFromString(dateString);
            Assert.Equal(result.DayOfWeek, IsoDayOfWeek.Wednesday);
            Assert.Equal(result.TimeOfDay, 1000);
        }

        [Theory]
        [InlineData("mon")]
        [InlineData("tues")]
        [InlineData("wed")]
        [InlineData("thurs")]
        [InlineData("fri")]
        [InlineData("sat")]
        [InlineData("sun")]
        public void DayOfWeekStringsAreValid(string validDayOfWeekString)
        {
            TimeHelpers.DayOfWeekFromString(validDayOfWeekString);
        }


        [Fact]
        public void DayOfWeekThrows()
        {
            Assert.Throws<ArgumentException>(() => TimeHelpers.DayOfWeekFromString("not a day"));
        }
    }
}
