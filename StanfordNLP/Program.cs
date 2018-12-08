using Console = System.Console;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;

namespace StanfordNLP
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            string baseAddress = $"http://*:{port}/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Running server at: {baseAddress}");
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
