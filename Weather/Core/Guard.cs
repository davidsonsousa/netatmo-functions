using System.Net.Http;
using System.Security.Authentication;
using Weather.Exceptions;
using Weather.Models;

namespace Weather.Core
{
    public static class Guard
    {
        public static void ForInvalidResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpStatusException(response.StatusCode);
            }
        }

        public static void ForAuthenticationFailure(AuthenticationResponse authResponse)
        {
            if (authResponse == null || !string.IsNullOrWhiteSpace(authResponse.Error))
            {
                throw new AuthenticationException("Could not authenticate function");
            }
        }
    }
}
