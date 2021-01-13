using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;
using MediatR;

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

            return services;
        }
    }
}
