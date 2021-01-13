using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    internal static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWindgramIdentitySTS(this IApplicationBuilder app, IConfiguration configuration, IHostEnvironment env)
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

            app.Use(async (ctx, next) =>
            {
                ctx.SetIdentityServerOrigin(configuration["IdentityUrl"]);
                await next();
            });
            app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseRouting();

            app.UseCors(configuration["CorsPolicy"]);
            app.UseCookiePolicy();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());
            });
            return app;
        }
    }
}