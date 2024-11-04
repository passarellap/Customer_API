using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace AnagraficaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(environmentName))
                environmentName = args.Length == 1 ? args[0] : "Production";

            IHost webHost = CreateHostBuilder(args)
                            .UseEnvironment(environmentName)
                            .Build();

            // Read configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            // Create Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Debug($"Environment Name : {environmentName}");
            Log.Debug($"AppContext.BaseDirectory : {AppContext.BaseDirectory}");

            //Start webHost
            try
            {
                Log.Information("Starting web host");
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, config) =>
            {
                config.ClearProviders();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .ConfigureKestrel(serverOptions =>
                {
                    //serverOptions.Limits.MaxConcurrentConnections = 100;
                    //serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                    //serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
                    //serverOptions.Limits.MinRequestBodyDataRate =
                    //    new MinDataRate(bytesPerSecond: 100,
                    //        gracePeriod: TimeSpan.FromSeconds(10));
                    //serverOptions.Limits.MinResponseDataRate =
                    //    new MinDataRate(bytesPerSecond: 100,
                    //        gracePeriod: TimeSpan.FromSeconds(10));
                    //serverOptions.Listen(IPAddress.Loopback, 5000);
                    //serverOptions.Listen(IPAddress.Loopback, 5001,
                    //    listenOptions =>
                    //    {
                    //        listenOptions.UseHttps("testCert.pfx",
                    //            "testPassword");
                    //    });
                    //serverOptions.Limits.KeepAliveTimeout =
                    //    TimeSpan.FromMinutes(2);
                    //serverOptions.Limits.RequestHeadersTimeout =
                    //    TimeSpan.FromMinutes(1);
                })
                .UseKestrel()
                .UseStartup<Startup>()
                ;
            })
            .UseSerilog();
    }
}
