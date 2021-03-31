using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core;

namespace Weather
{
    public class GetLatestCo2
    {
        private readonly IHttpClientFactory _clientFactory;

        public GetLatestCo2(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [FunctionName("GetLatestCo2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var token = "5ec2c8a6d9385d10f53977f5|44f603a71dc57c23d3a7aea654befb24";
            var macAddress = "70:ee:50:64:1e:26";
            var timeFrame = "1hour";
            var dateBegin = DateTimeOffset.Now.Add(-new TimeSpan(0, 30, 0)).ToUnixTimeSeconds().ToString();

            var restClient = new RestClient(_clientFactory, log);
            var response = await restClient.GetCo2Information(macAddress,timeFrame, dateBegin, token);

            if (response == null)
            {
                return new NotFoundObjectResult("Something went wrong");
            }

            return new OkObjectResult(response.Body.Values.LastOrDefault());
        }
    }
}

