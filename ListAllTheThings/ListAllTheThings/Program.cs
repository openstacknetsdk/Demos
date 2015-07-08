using System;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace;

namespace ListAllTheThings
{
    class Program
    {
        static void Main(string[] args)
        {
            var identity = new CloudIdentity
            {
                Username = Environment.GetEnvironmentVariable("OPENSTACKNET_USER"),
                APIKey = Environment.GetEnvironmentVariable("OPENSTACKNET_APIKEY")
            };

            var serversProvider = new CloudServersProvider(identity);

            var servers = serversProvider.ListServers();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Id\t\t\t\t\tName");
            Console.ResetColor();
            Console.WriteLine();
            foreach (var server in servers)
            {
                Console.WriteLine("{0}\t{1}", server.Id, server.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
