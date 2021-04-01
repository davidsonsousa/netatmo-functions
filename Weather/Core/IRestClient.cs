using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Core
{
    public interface IRestClient
    {
        Task<NetatmoResponse> GetStationInformation(string macAddress, string token, string type);
    }
}
