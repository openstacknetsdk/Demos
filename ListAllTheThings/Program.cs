using System;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace;
using OpenStack.ContentDeliveryNetworks.v1;
using OpenStack.Synchronous;

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

            var authProvider = new CloudIdentityProvider(identity);
            

            Console.WriteLine("--- Servers ---");
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

            Console.WriteLine("--- CDN Services ---");
            var cdnService = new ContentDeliveryNetworkService(authProvider, "DFW");
            var cdns = cdnService.ListServices();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Id\t\t\t\t\tName");
            Console.ResetColor();
            Console.WriteLine();
            foreach (var cdn in cdns)
            {
                Console.WriteLine("{0}\t{1}", cdn.Id, cdn.Name);
            }
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
