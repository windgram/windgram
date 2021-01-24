using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Microsoft.AspNetCore.Builder
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWindgramBloggingAPI(this IApplicationBuilder app, IHostEnvironment env, IConfiguration configuration)
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
