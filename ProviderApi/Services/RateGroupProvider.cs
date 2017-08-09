using System;
using System.Collections.Generic;
using ProviderApi.Models;

namespace ProviderApi.Services
{
    public interface IGroupProvider
    {
        RateGroup GetGroup(Guid groupId);
    }

    public class InMemoryGroupProvider : IGroupProvider
    {
        private readonly Dictionary<Guid, RateGroup> RateGroups;
        public InMemoryGroupProvider(Dictionary<Guid, RateGroup> rateGroups)
        {
            RateGroups = rateGroups;
        }

        public RateGroup GetGroup(Guid groupId)
        {
            return RateGroups[groupId];
        }

        public void AddGroup(RateGroup group)
        {
            RateGroups[group.GroupId] = group;
        }
    }
}