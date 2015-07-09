using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marvin.JsonPatch;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace;
using OpenStack;
using OpenStack.ContentDeliveryNetworks.v1;
using Flavor = OpenStack.ContentDeliveryNetworks.v1.Flavor;

namespace Rackspace.Quickstart.CDN
{
    public class Test : IExampleTest
    {
        public async Task Run(string username, string apiKey)
        {
            // Authenticate
            var identity = new CloudIdentity
            {
                APIKey = apiKey,
                Username = username
            };
            var authProvider = new CloudIdentityProvider(identity);
            var cdnService = new ContentDeliveryNetworkService(authProvider, "DFW");

            // List Flavors
            Console.WriteLine();
            Console.WriteLine("List Flavors...");
            IEnumerable<Flavor> flavors = await cdnService.ListFlavorsAsync();
            foreach (Flavor f in flavors)
            {
                Console.WriteLine("{0}\t{1}", f.Id, f.Providers.First().Name);
            }

            // Get Flavor
            Console.WriteLine();
            Console.WriteLine("Get Flavor...");
            Flavor flavor = await cdnService.GetFlavorAsync(flavors.First().Id);
            Console.WriteLine("{0}\t{1}", flavor.Id, flavor.Providers.First().Name);

            // Create Service
            Console.WriteLine();
            Console.WriteLine("Create Service...");
            ServiceDefinition serviceDefinition = new ServiceDefinition("example_site", flavor.Id,
                domains: new[] {new ServiceDomain("www.example.com")},
                origins: new[] {new ServiceOrigin("example.com")});
            string serviceId = await cdnService.CreateServiceAsync(serviceDefinition);
            await cdnService.WaitForServiceDeployedAsync(serviceId);
            Console.WriteLine("Service Created: {0}", serviceId);

            // List Services
            Console.WriteLine();
            Console.WriteLine("List Services..");
            IPage<Service> services = await cdnService.ListServicesAsync();
            foreach (Service s in services)
            {
                Console.WriteLine("{0}\t{1}", s.Id, s.Name);   
            }

            // Get Service
            Console.WriteLine();
            Console.WriteLine("Get Service");
            Service service = await cdnService.GetServiceAsync(serviceId);
            Console.WriteLine("{0}\t{1}", service.Id, service.Name);
            
            // Purge All Service Assets
            Console.WriteLine();
            Console.WriteLine("Purge All Service Assets...");
            await cdnService.PurgeCachedAssetsAsync(serviceId);

            // Purge a Specific Service Asset
            Console.WriteLine();
            Console.WriteLine("Purge a Specific Service Asset...");
            await cdnService.PurgeCachedAssetAsync(serviceId, "index.html");

            // Update Service
            Console.WriteLine();
            Console.WriteLine("Update Service...");
            var patch = new JsonPatchDocument<ServiceDefinition>();
            patch.Replace(svc => svc.Name, "newServiceName");
            await cdnService.UpdateServiceAsync(serviceId, patch);
            await cdnService.WaitForServiceDeployedAsync(serviceId);

            // Delete Service
            Console.WriteLine();
            Console.WriteLine("Delete Service...");
            await cdnService.DeleteServiceAsync(serviceId);
            await cdnService.WaitForServiceDeletedAsync(serviceId);
        }
    }
}