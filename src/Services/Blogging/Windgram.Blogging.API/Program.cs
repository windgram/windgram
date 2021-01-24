using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Windgram.Blogging.Infrastructure;

namespace Windgram.Blogging.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host
                .MigrateDbContext<BloggingDbContext>((_, __) => { });

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
