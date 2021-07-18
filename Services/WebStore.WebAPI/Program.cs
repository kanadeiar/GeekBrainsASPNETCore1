using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace WebStore.WebAPI
{
    /// <summary> Главная программа </summary>
    public class Program
    {
        /// <summary> Вход </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary> Хост приложения </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging((host, log) => log
                //        .ClearProviders()
                //        .AddDebug()
                //        .AddEventLog()
                //        .AddConsole()
                //        .AddFilter<ConsoleLoggerProvider>(level => level >= LogLevel.Information)
                //    //.AddFilter<ConsoleLoggerProvider>((category, level) => level >= LogLevel.Information && category.EndsWith("Info"))
                //)
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                        .UseStartup<Startup>())
                .UseSerilog((host, log) => log
                    .ReadFrom.Configuration(host.Configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                    .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                    .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
                )
            ;
    }
}
