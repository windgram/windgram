using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Windgram.Identity.ApplicationCore.Queries;
using Windgram.Caching;

namespace Windgram.Identity.ApplicationCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramIdentityApplication(this IServiceCollection services, string connectionString)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);

            services.AddScoped<IUserQueries>(sp => new UserQueries(connectionString, sp.GetService<ICacheManager>()));
            return services;
        }
    }
}
