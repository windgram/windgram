using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Windgram.Identity.Infrastructure;
using Windgram.Identity.STS.Configurations;

namespace Windgram.Identity.STS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host
                .MigrateDbContext<IdentityContext>((_, __) => { })
                .MigrateDbContext<ConfigurationContext>((context, services) =>
                {
                    new ConfigurationDbContextSeed()
                   .SeedAsync(context, services.GetRequiredService<IConfiguration>()).Wait();
                })
                .MigrateDbContext<PersistedGrantContext>((_, __) => { });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
