namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderEtensions
    {
        public static IApplicationBuilder UseWindgramAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
        public static IApplicationBuilder UseWindgramSwagger(this IApplicationBuilder app, string apiName, string apiVersion, string pathBase)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(string.IsNullOrEmpty(pathBase) ? string.Empty : pathBase)}/swagger/{apiVersion}/swagger.json", $"{apiName} {apiVersion}");
            });
            return app;
        }
    }
}
