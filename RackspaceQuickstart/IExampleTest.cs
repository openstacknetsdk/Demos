using System.Threading.Tasks;

namespace Rackspace.Quickstart
{
    public interface IExampleTest
    {
        Task Run(string username, string apiKey);
    }
}