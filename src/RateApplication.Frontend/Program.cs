using System;
using Nancy.Hosting.Self;

namespace RateApplication.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultServiceName = "RateApplication/frontend/";
            var defaultPort = 80;
            var defaultApiUrl = "http://localhost:8080/RateApplication/api";
            var usage =
                $"Usage: {AppDomain.CurrentDomain.FriendlyName} [service name (default={defaultServiceName})] [port (default={defaultPort})] [api url (default={defaultApiUrl})]";

            if (args.Length > 3)
            {
                Console.WriteLine(usage);
                return;
            }

            var serviceName = defaultServiceName;
            var port = defaultPort;
            var apiUrl = defaultApiUrl;

            if (args.Length >= 1)
            {
                serviceName = args[0];
                if (!serviceName.EndsWith("/"))
                {
                    serviceName += "/";
                }
            }
            if (args.Length >= 2)
            {
                if (!int.TryParse(args[1], out port))
                {
                    Console.WriteLine("Could not parse second argument (port) as integer.");
                    Console.WriteLine(usage);
                    return;
                }
            }
            if (args.Length >= 2)
            {
                apiUrl = args[2];
                if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out Uri uriResult) || uriResult.Scheme != Uri.UriSchemeHttp)
                {
                    Console.WriteLine("Could not parse third argument (api url) as http url.");
                    Console.WriteLine(usage);
                    return;
                }
            }

            Module.SkillsApiUrl = apiUrl;

            var uri = new Uri($"http://localhost:{port}/{serviceName}");

            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = { CreateAutomatically = true }
            };

            using (var host = new NancyHost(hostConfiguration, uri))
            {
                host.Start();
                Console.WriteLine($"Running on {uri}");
                Console.WriteLine("Press enter to stop.");
                Console.ReadLine();
            }
        }
    }
}
