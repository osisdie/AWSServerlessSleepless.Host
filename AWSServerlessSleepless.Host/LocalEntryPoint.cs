using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ServerlessSleepless.Host
{
    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:5001")
                .UseStartup<Startup>()
                .Build();
    }
}
