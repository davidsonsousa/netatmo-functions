using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Core
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public RestClient(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<NetatmoResponse> GetStationInformation(string macAddress, string token, string type)
        {
            try
            {
                var dateBegin = DateTimeOffset.Now.Add(-new TimeSpan(0, 30, 0)).ToUnixTimeSeconds().ToString();
                var endpoint = $"https://api.netatmo.com/api/getmeasure?device_id={macAddress}&scale=1hour&date_begin={dateBegin}&optimize=false&real_time=false&type={type}";
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
