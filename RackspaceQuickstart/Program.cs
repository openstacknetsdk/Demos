using System;
using System.Collections.Generic;

namespace Rackspace.Quickstart
{
    internal class Program
    {
        private static Dictionary<int, IExampleTest> _tests = new Dictionary<int, IExampleTest>
        {
            {1, new CDN.Test()}
        };

        private static void Main()
        {
            Console.Write("Enter your Rackspace username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your Rackspace API Key: ");
            string apikey = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Available Examples: ");
            Console.WriteLine("\t1. Content Delivery Network (CDN)");
            Console.WriteLine();

            Console.Write("Enter the example number to execute: ");
            string requestedExample = Console.ReadLine();
            int exampleId;
            if (!(int.TryParse(requestedExample, out exampleId) && _tests.ContainsKey(exampleId)))
            {
                Console.WriteLine("Invalid example requested. Exiting.");
            }
            else
            {
                _tests[exampleId].Run(username, apikey).Wait();
            }

            Console.Write("Press any key to exit...");
            Console.ReadLine();
        }
    }
}