using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Windgram.Identity.STS
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        public Startup(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWindgramIdentitySTS(_configuration, _hostEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseWindgramIdentitySTS(_configuration, _hostEnvironment);
        }
    }
}
