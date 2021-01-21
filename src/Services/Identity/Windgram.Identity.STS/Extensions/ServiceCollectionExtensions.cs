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
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using IdentityModel;

namespace Windgram.Identity.Web.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramIdentitySTS(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var redisConnection = configuration.GetSection(Caching.CacheConfig.CONFIGURATION_KEY)["ConnectionString"];
            var identityConnection = configuration.GetConnectionString("IdentityConnection");
            var configurationConnection = configuration.GetConnectionString("ConfigurationConnection");
            var persistedGrantConnection = configuration.GetConnectionString("PersistedGrantConnection");

            services.AddWindgramMvcWithLocalization()
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
                .AddWindgramEventBusRabbitMQ(configuration, typeof(STS.Startup).Assembly)
                .AddWindgramHttpUserContext();

            return services;
        }
        public static IMvcBuilder AddWindgramMvcWithLocalization(this IServiceCollection services)
        {
            string defaultCulture = "zh-CN";
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("zh-CN")
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: defaultCulture, uiCulture: defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            return services.AddControllersWithViews()
                .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
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
                options.SignIn.RequireConfirmedAccount = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequiredUniqueChars = 0,
                    RequireNonAlphanumeric = false,
                    RequiredLength = 8
                };
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "Identity.STS";
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
            services.AddAuthentication()
                .AddGitHub(configuration["Github:ClientId"], configuration["Github:ClientSecret"])
                .AddMicrosoftAccount(options =>
                {
                    options.SaveTokens = true;
                    options.ClientId = configuration["Microsoft:ClientId"];
                    options.ClientSecret = configuration["Microsoft:ClientSecret"];
                });
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
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string clientId, string clientSecret)
        {
            // You must first create an app with GitHub and add its ID and Secret to your user-secrets.
            // https://github.com/settings/applications/
            // https://docs.github.com/en/developers/apps/authorizing-oauth-apps
            return builder.AddOAuth("GitHub", "Github", o =>
                  {
                      o.ClientId = clientId;
                      o.ClientSecret = clientSecret;
                      o.CallbackPath = new PathString("/signin-github");
                      o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                      o.TokenEndpoint = "https://github.com/login/oauth/access_token";
                      o.UserInformationEndpoint = "https://api.github.com/user";
                      o.ClaimsIssuer = "OAuth2-Github";
                      o.SaveTokens = true;
                      // Retrieving user information is unique to each provider.
                      o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                      o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                      o.ClaimActions.MapJsonKey("urn:github:name", "name");
                      o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                      o.ClaimActions.MapJsonKey("urn:github:url", "url");
                      o.ClaimActions.MapJsonKey(JwtClaimTypes.Picture, "avatar_url");
                      o.Events = new OAuthEvents
                      {
                          OnCreatingTicket = async context =>
                          {
                              // Get the GitHub user
                              var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                              request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                              request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                              var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                              response.EnsureSuccessStatusCode();

                              using (var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                              {
                                  context.RunClaimActions(user.RootElement);
                              }
                          }
                      };
                  });
        }
    }
}
