using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Core
{
    public interface IRestClient
    {
        Task<AuthenticationResponse> Authenticate();
        Task<StationResponse> GetStationInformation(string deviceId, string moduleId, string token, string type);
    }
}
