using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raimun.ConsoleApp.Configurations;
using Raimun.ConsoleApp.Services;
using Raimun.Core;
using Raimun.Core.Common.Interfaces;
using Raimun.Data;
using System;

namespace Raimun.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    IConfiguration Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();
                    services.AddCore()
                    .AddData(Configuration);

                    services.AddScoped<IWeatherService, WeatherService>();
                    services.AddHostedService<WeatherLocationReceiver>();

                    services.Configure<WeatherConfig>(Configuration.GetSection("WeatherConfig"));
                    services.Configure<RabbitMQConfig>(Configuration.GetSection("RabbitMQ"));
                })
                ;
        }

    }
}
