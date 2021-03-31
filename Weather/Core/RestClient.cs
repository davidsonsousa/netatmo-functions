using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Core
{
    public class RestClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public RestClient(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<NetatmoResponse> GetCo2Information(string macAddress, string timeFrame, string dateBegin, string token)
        {
            try
            {
                var endpoint = $"https://api.netatmo.com/api/getmeasure?device_id={macAddress}&scale={timeFrame}&date_begin={dateBegin}&optimize=false&real_time=false&type=co2";
                var response = await CallRestApi(endpoint, token);

                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    var netatmoResponse = JsonConvert.DeserializeObject<NetatmoResponse>(contentResponse);
                    _logger.LogInformation(netatmoResponse.ToString());
                    return netatmoResponse;
                }
            }
            catch
            {
                throw;
            }

            return null;
        }

        private async Task<HttpResponseMessage> CallRestApi(string endpoint, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Clear();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var client = _clientFactory.CreateClient();
                return await client.SendAsync(request);
            }
            catch
            {
                throw;
            }
        }
    }
}
