using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Windgram.Identity.API.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWindgramIdentityAPI(this IApplicationBuilder app, IHostEnvironment env, IConfiguration configuration)
        {
            var pathBase = configuration["PathBase"];
            if (!pathBase.IsNullOrEmpty())
            {
                app.UsePathBase(pathBase);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWindgramSwagger(configuration["ApiName"], configuration["ApiVersion"], pathBase);
            }

            app.UseRouting();
            app.UseCors(configuration["CorsPolicy"]);
            app.UseWindgramAuth();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
