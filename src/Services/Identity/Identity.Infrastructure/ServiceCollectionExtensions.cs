using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Reflection;
using Windgram.Identity.ApplicationCore.Interfaces;
using Windgram.Identity.Infrastructure.Repositories;

namespace Windgram.Identity.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramIdentityDbContexts(this IServiceCollection services,
                string identityConnectionString,
                string configurationConnectionString,
                string persistedGrantConnectionString,
                Version mySqlVerison)
        {
            var migrationsAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdentityContext>(options =>
            options.UseMySql(identityConnectionString, new MySqlServerVersion(mySqlVerison), mySql =>
            {
                mySql.CharSetBehavior(CharSetBehavior.NeverAppend);
                mySql.MigrationsAssembly(migrationsAssembly);
                mySql.EnableRetryOnFailure(5);
                mySql.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}");
            }), ServiceLifetime.Scoped);

            services.AddConfigurationDbContext<ConfigurationContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(configurationConnectionString, new MySqlServerVersion(mySqlVerison), mySql =>
                    {
                        mySql.CharSetBehavior(CharSetBehavior.NeverAppend);
                        mySql.MigrationsAssembly(migrationsAssembly);
                        mySql.EnableRetryOnFailure(5);
                        mySql.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}");
                    }));

            services.AddOperationalDbContext<PersistedGrantContext>(options =>
                options.ConfigureDbContext = b =>
                    b.UseMySql(persistedGrantConnectionString, new MySqlServerVersion(mySqlVerison), mySql =>
                    {
                        mySql.CharSetBehavior(CharSetBehavior.NeverAppend);
                        mySql.MigrationsAssembly(migrationsAssembly);
                        mySql.EnableRetryOnFailure(5);
                        mySql.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema ?? "dbo"}_{entity}");
                    }));
            services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));
            return services;
        }
    }
}
