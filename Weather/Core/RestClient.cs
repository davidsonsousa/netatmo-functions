using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Core
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public RestClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<AuthenticationResponse> Authenticate()
        {
            var endpoint = "https://api.netatmo.com/oauth2/token";
            var bodyInformation = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", Environment.GetEnvironmentVariable("Netatmo_ClientId")),
                new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("Netatmo_ClientSecret")),
                new KeyValuePair<string, string>("grant_type", Environment.GetEnvironmentVariable("Netatmo_GrantType")),
                new KeyValuePair<string, string>("username", Environment.GetEnvironmentVariable("Netatmo_Username")),
                new KeyValuePair<string, string>("password", Environment.GetEnvironmentVariable("Netatmo_Password")),
                new KeyValuePair<string, string>("scope", Environment.GetEnvironmentVariable("Netatmo_Scope"))
            };

            try
            {
                var response = await CallRestApiPost(endpoint, new FormUrlEncodedContent(bodyInformation));
                Guard.ForInvalidResponse(response);

                var contentResponse = await response.Content.ReadAsStringAsync();
                var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(contentResponse);
                return authenticationResponse;
            }
            catch
            {
                throw;
            }
        }

        public async Task<StationResponse> GetStationInformation(string deviceId, string moduleId, string token, string type)
        {
            if (string.IsNullOrWhiteSpace(moduleId))
            {
                moduleId = deviceId;
            }

            var dateBegin = DateTimeOffset.Now.Add(-new TimeSpan(0, 30, 0)).ToUnixTimeSeconds().ToString();
            var endpoint = $"https://api.netatmo.com/api/getmeasure?device_id={deviceId}&module_id={moduleId}&scale=1hour&date_begin={dateBegin}&optimize=false&real_time=false&type={type}";

            try
            {
                var response = await CallRestApiGet(endpoint, token);
                Guard.ForInvalidResponse(response);

                var contentResponse = await response.Content.ReadAsStringAsync();
                var stationResponse = JsonConvert.DeserializeObject<StationResponse>(contentResponse);
                return stationResponse;
            }
            catch
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> CallRestApiGet(string endpoint, string token)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.GetAsync(endpoint);
            }
            catch
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> CallRestApiPost(string endpoint, FormUrlEncodedContent formContent)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                return await client.PostAsync(endpoint, formContent);
            }
            catch
            {
                throw;
            }
        }
    }
}
