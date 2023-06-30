using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace S3UploaderLambda
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(config => config
                    .AddEnvironmentVariables())
                .Build();

            host.Run();
        }
    }
}