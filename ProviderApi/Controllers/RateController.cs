using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProviderApi.Core;
using ProviderApi.Models;
using ProviderApi.Services;

namespace ProviderApi.Controllers
{
    [Route("api/[controller]")]
    public class RateController : Controller
    {
        private readonly IGroupRateService ProviderService;
        public static readonly string NoRateResponse = "unavailable";

        public RateController(IGroupRateService providerService)
        {
            ProviderService = providerService;
        }

        [HttpGet]
        public string Get([FromQuery] Guid? groupId, [FromQuery] string begin, [FromQuery] string end)
        {
            var beginTime = TimeHelpers.GetUtcTimeDayFromString(begin);
            var endTime = TimeHelpers.GetUtcTimeDayFromString(end);

            if (beginTime == null || endTime == null)
            {
                throw new ArgumentException($"Rate request time range could not be parsed from {groupId} {begin} {end}");
            }
            
            // Temporary hardcoded group id for sample project
            groupId = groupId ?? Guid.Empty;

            var rate = ProviderService.GetRateForRange(groupId.Value, beginTime, endTime);
            return rate.HasValue ? rate.Value.ToString() : NoRateResponse;
        }

        [HttpPost]
        public string Post([FromBody]RateRequest rateRequest)
        {
            return Get(rateRequest.GroupId, rateRequest.BeginTime, rateRequest.EndTime);
        }
    }
}
