using System;
using Nancy.Hosting.Self;

namespace RateApplication.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultServiceName = "RateApplication/api/";
            var defaultPort = 8080;
            var usage =
                $"Usage: {AppDomain.CurrentDomain.FriendlyName} [service name (default=\"{defaultServiceName}\")] [port (default={defaultPort})]";

            if (args.Length > 2)
            {
                Console.WriteLine(usage);
                return;
            }

            var serviceName = defaultServiceName;
            var port = defaultPort;

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

            var uri = new Uri($"http://localhost:{port}/{serviceName}");

            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = {CreateAutomatically = true}
            };
            
            Bootstrapper.SkillsManager = new SkillsManager(5);

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