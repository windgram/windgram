using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Windgram.Blogging.ApplicationCore.Queries;
using Windgram.Caching;

namespace Windgram.Blogging.ApplicationCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindgramBloggingApplication(this IServiceCollection services, string connectionString)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);

            services.AddScoped<ITagQueries>(sp => new TagQueries(connectionString, sp.GetRequiredService<ICacheManager>()));
            services.AddScoped<IPostQueries>(sp => new PostQueries(connectionString, sp.GetRequiredService<ICacheManager>()));
            return services;
        }
    }
}
