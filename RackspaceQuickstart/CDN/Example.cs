using System.Collections.Generic;
using Marvin.JsonPatch;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace;
using OpenStack;
using OpenStack.ContentDeliveryNetworks.v1;
using Flavor = OpenStack.ContentDeliveryNetworks.v1.Flavor;

namespace Rackspace.Quickstart.CDN
{
    public class Example
    {
        public async static void ExampleDoc()
        {
            // Authenticate
            var identity = new CloudIdentity
            {
                APIKey = "{apikey}",
                Username = "{username}"
            };
            var authProvider = new CloudIdentityProvider(identity);
            var cdnService = new ContentDeliveryNetworkService(authProvider, "{region}");

            // List Flavors
            IEnumerable<Flavor> flavors = await cdnService.ListFlavorsAsync();

            // Get Flavor
            Flavor flavor = await cdnService.GetFlavorAsync("{flavorId}");

            // Create Service
            ServiceDefinition serviceDefinition = new ServiceDefinition("example_site", "{flavorId}",
                domains: new[] {new ServiceDomain("www.example.com")},
                origins: new[] {new ServiceOrigin("example.com")});
            string serviceId = await cdnService.CreateServiceAsync(serviceDefinition);
            await cdnService.WaitForServiceDeployedAsync(serviceId);

            // List Services
            IPage<Service> services = await cdnService.ListServicesAsync();

            // Get Service
            Service service = await cdnService.GetServiceAsync("{serviceId}");

            // Purge All Service Assets
            await cdnService.PurgeCachedAssetsAsync("{serviceId}");

            // Purge a Specific Service Asset
            await cdnService.PurgeCachedAssetAsync("{serviceId}", "{relativeUrlOfAsset}");

            // Update Service
            var patch = new JsonPatchDocument<ServiceDefinition>();
            patch.Replace(svc => svc.Name, "newServiceName");
            await cdnService.UpdateServiceAsync("{serviceId}", patch);
            await cdnService.WaitForServiceDeployedAsync("{serviceId}");

            // Delete Service
            await cdnService.DeleteServiceAsync("{serviceId}");
            await cdnService.WaitForServiceDeletedAsync("{serviceId}");
        }
    }
}