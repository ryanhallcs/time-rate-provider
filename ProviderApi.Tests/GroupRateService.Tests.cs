using System;
using System.Collections.Generic;
using Moq;
using NodaTime;
using ProviderApi.Models;
using ProviderApi.Services;
using Xunit;

namespace ProviderApi.Tests
{
    public class GroupRateServiceTests
    {
        private Mock<IGroupProvider> MockProvider;
        private RateGroup RateGroup;

        public GroupRateServiceTests()
        {
            MockProvider = new Mock<IGroupProvider>();
            RateGroup = new RateGroup();

            MockProvider.Setup(provider => provider.GetGroup(It.IsAny<Guid>())).Returns(RateGroup);
        }

        [Fact]
        public void ThrowsWhenDifferentDays()
        {
            var sut = new GroupRateService(MockProvider.Object);
            var timeDay1 = new TimeDay();
            var timeDay2 = new TimeDay();
            timeDay1.DayOfWeek = IsoDayOfWeek.Monday;
            timeDay2.DayOfWeek = IsoDayOfWeek.Tuesday;

            Assert.Throws<ArgumentException>(() => sut.GetRateForRange(Guid.Empty, timeDay1, timeDay2));
        }

        [Fact]
        public void ReturnsCorrectRate()
        {
            RateGroup.DayRateRanges = new Dictionary<IsoDayOfWeek, List<TimeRangeRate>>
            {
                { IsoDayOfWeek.Monday, new List<TimeRangeRate> 
                    { 
                        new TimeRangeRate(1000, 1300, 1000), 
                        new TimeRangeRate(1301, 1600, 1200) 
                    } 
                }
            };

            var sut = new GroupRateService(MockProvider.Object);
            var timeDay1 = new TimeDay();
            var timeDay2 = new TimeDay();
            timeDay1.DayOfWeek = IsoDayOfWeek.Monday;
            timeDay1.TimeOfDay = 1100;
            timeDay2.DayOfWeek = IsoDayOfWeek.Monday;
            timeDay2.TimeOfDay = 1200;

            var result = sut.GetRateForRange(Guid.Empty, timeDay1, timeDay2);
            Assert.Equal(1000, result);
        }

        [Fact]
        public void ReturnsNullIfNotFullEncapsulated()
        {
            RateGroup.DayRateRanges = new Dictionary<IsoDayOfWeek, List<TimeRangeRate>>
            {
                { IsoDayOfWeek.Monday, new List<TimeRangeRate> 
                    { 
                        new TimeRangeRate(1000, 1300, 1000),
                    } 
                }
            };

            var sut = new GroupRateService(MockProvider.Object);
            var timeDay1 = new TimeDay();
            var timeDay2 = new TimeDay();
            timeDay1.DayOfWeek = IsoDayOfWeek.Monday;
            timeDay1.TimeOfDay = 1100;
            timeDay2.DayOfWeek = IsoDayOfWeek.Monday;
            timeDay2.TimeOfDay = 1300;

            var result = sut.GetRateForRange(Guid.Empty, timeDay1, timeDay2);
            Assert.Null(result);
        }
    }
}