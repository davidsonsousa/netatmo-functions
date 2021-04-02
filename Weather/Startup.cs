using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Weather;
using Weather.Core;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Weather
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IRestClient, RestClient>();
        }
    }
}
