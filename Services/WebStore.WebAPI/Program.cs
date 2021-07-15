using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace WebStore.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((host, log) => log
                        .ClearProviders()
                        .AddDebug()
                        .AddEventLog()
                        .AddConsole()
                        .AddFilter<ConsoleLoggerProvider>(level => level >= LogLevel.Information)
                    //.AddFilter<ConsoleLoggerProvider>((category, level) => level >= LogLevel.Information && category.EndsWith("Info"))
                )
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                        .UseStartup<Startup>()
                )
            ;
    }
}
