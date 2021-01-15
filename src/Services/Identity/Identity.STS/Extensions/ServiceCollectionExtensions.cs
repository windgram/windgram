using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Security.Cryptography.X509Certificates;
using Windgram.Identity.ApplicationCore.Domain.Entities;
using Windgram.Caching.Redis;
using Windgram.EventBus.RabbitMQ;
using Windgram.Identity.ApplicationCore;
using Windgram.Identity.Infrastructure;
using Windgram.Identity.Web.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramIdentitySTS(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var redisConnection = configuration.GetSection(Windgram.Caching.CacheConfig.CONFIGURATION_KEY)["ConnectionString"];
            var identityConnection = configuration.GetConnectionString("IdentityConnection");
            var configurationConnection = configuration.GetConnectionString("ConfigurationConnection");
            var persistedGrantConnection = configuration.GetConnectionString("PersistedGrantConnection");

            services.AddControllersWithViews()
               .AddNewtonsoftJson(setup =>
               {
                   setup.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               })
               .AddFluentValidation();
            services
                .AddWindgramHealthChecks()
                .AddWindgramCookiePolicy()
                .AddWindgramCorsPolicy(configuration["CorsPolicy"])
                .AddWindgramIdentityDbContexts(identityConnection, configurationConnection, persistedGrantConnection, new Version(configuration["MySqlVersion"]))
                .AddWindgramIdentity()
                .AddWindgramIdentityServer(hostEnvironment, configuration)
                .AddWindgramIdentityApplication(identityConnection)
                .AddWindgramRedisCache(configuration)
                .AddWindgramDataProtection(redisConnection)
                .AddWindgramEventBusRabbitMQ(configuration, typeof(Windgram.Identity.STS.Startup).Assembly)
                .AddWindgramHttpUserContext();

            return services;
        }
        public static IServiceCollection AddWindgramCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = (cookieContext) =>
                {
                    cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
                };
                options.OnDeleteCookie = (cookieContext) =>
                {
                    cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
                };
            });
            return services;
        }
        public static IServiceCollection AddWindgramIdentity(this IServiceCollection services)
        {
            services.AddIdentity<UserIdentity, UserIdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequiredUniqueChars = 0,
                    RequireNonAlphanumeric = false,
                    RequiredLength = 8
                };
            }).AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "Identity.STS";
                o.LoginPath = "/Login";
            });
            return services;
        }
        public static IServiceCollection AddWindgramIdentityServer(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
        {
            var identityBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
             // this adds the config data from DB (clients, resources)
             .AddConfigurationStore<ConfigurationContext>()
             // this adds the operational data from DB (codes, tokens, consents)
             .AddOperationalStore<PersistedGrantContext>(options =>
             {
                 // this enables automatic token cleanup. this is optional.
                 options.EnableTokenCleanup = true;
             })
             .AddAspNetIdentity<UserIdentity>()
             .AddProfileService<UserProfileService>();
            if (environment.IsDevelopment())
            {
                identityBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                identityBuilder
               .AddSigningCredential(new X509Certificate2(configuration["CertificatePath"], configuration["CertificatePassword"]));
            }
            //services.AddAuthentication()
            //.AddGitHub(options =>
            //{
            //    options.ClientId = configuration["Github:ClientId"];
            //    options.ClientSecret = configuration["Github:ClientSecret"];
            //    options.AllowSignup = true;
            //    var scopesString = configuration["Github:Scopes"];
            //    if (!scopesString.IsNullOrEmpty())
            //    {
            //        var scopes = scopesString.Split(",").ToList();
            //        scopes.ForEach(s => options.Scope.Add(s));
            //    }
            //});
            return services;
        }

        public static IServiceCollection AddWindgramDataProtection(this IServiceCollection services, string redisConnection)
        {
            services.AddDataProtection(opts =>
            {
                opts.ApplicationDiscriminator = $"IdentitySTS";
            })
             .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConnection), "identity.sts.dpk");
            return services;
        }
    }
}
