﻿using Interfaces;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            var config = ClientConfiguration.LoadFromFile("ClientConfiguration.xml");

            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Connected to silo");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    Console.WriteLine("Silo not available! Retrying in 3 seconds.");
                    Thread.Sleep(3000);
                }
            }

            Publisher();

            Console.ReadLine();
        }

        static void Publisher()
        {
            while (true)
            {
                Console.WriteLine("Press 'exit' to exit...");
                var input = Console.ReadLine();
                if (input == "exit") break;
                var publisherGrain = GrainClient.GrainFactory.GetGrain<IPublisherGrain>(Guid.Empty);
                publisherGrain.PublishMessageAsync(input);
            }
        }
    }
}
