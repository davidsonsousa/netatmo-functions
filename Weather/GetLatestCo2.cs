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
using Weather.Exceptions;
using Weather.Models;

namespace Weather
{
    public class GetLatestCo2
    {
        private readonly IRestClient _restClient;

        public GetLatestCo2(IRestClient restClient)
        {
            _restClient = restClient;
        }

        [FunctionName("GetLatestCo2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string deviceId = req.Query["device"];
            string moduleId = req.Query["module"];

            StationResponse response;

            try
            {
                var authResponse = await _restClient.Authenticate();
                Guard.ForAuthenticationFailure(authResponse);

                response = await _restClient.GetStationInformation(deviceId, moduleId, authResponse.AccessToken, Constants.CO2);
            }
            catch (HttpStatusException ex)
            {
                log.LogError(ex, $"Request to Netatmo API failed ({ex.StatusCode})");
                return new StatusCodeResult(ex.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                log.LogError(ex, "Request to Netatmo API failed");
                return new BadRequestObjectResult(ex);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error when loading station information");
                return new BadRequestObjectResult("Error when loading station information");
            }

            return new OkObjectResult(response.Body.Values.LastOrDefault());
        }
    }
}

