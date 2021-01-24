using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Windgram.Blogging.Infrastructure.Repositories;
using Windgram.Shared.Domain;

namespace Windgram.Blogging.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramBloggingDbContexts(this IServiceCollection services, string connectionString, Version mySqlVerison)
        {
            services.AddDbContext<BloggingDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(mySqlVerison), mySql =>
             {
                 mySql.CharSetBehavior(CharSetBehavior.NeverAppend);
                 mySql.MigrationsAssembly(typeof(BloggingDbContext).Assembly.GetName().Name);
                 mySql.EnableRetryOnFailure(5);
                 mySql.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}");
             }), ServiceLifetime.Scoped);

            services.AddScoped(typeof(IRepository<>), typeof(BloggingRepository<>));
            return services;
        }
    }
}
