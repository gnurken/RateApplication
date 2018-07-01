using System;
using Nancy.Hosting.Self;
using RateApplication.Backend.Nancy;
using RateApplication.Backend.Sqlite;

namespace RateApplication.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var defaultServiceName = "RateApplication/api/";
            var defaultPort = 8080;
            var defaultDatabase = "skills.sqlite";
            var usage =
                $"Usage: {AppDomain.CurrentDomain.FriendlyName} [service name (default=\"{defaultServiceName}\")] [port (default={defaultPort})] [SQLite database (default={defaultDatabase})]";

            if (args.Length > 3)
            {
                Console.WriteLine(usage);
                return;
            }

            var serviceName = defaultServiceName;
            var port = defaultPort;
            var database = defaultDatabase;

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
            if (args.Length >= 3)
            {
                database = args[2];
            }


            var uri = new Uri($"http://localhost:{port}/{serviceName}");

            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = {CreateAutomatically = true}
            };
            
            var dbContext = new SkillsDbContext($"Data Source={database};Version=3;");
            Bootstrapper.SkillsManager = new SqlSkillsManager(dbContext, 5);

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