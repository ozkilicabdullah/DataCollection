using DataCollection.Middleware;
using DataCollection.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataCollection.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddScopedDynamic<TInterface>(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddScoped<Func<string, TInterface>>(serviceProvider => tenant =>
            {
                
                var type = types.Where(x => x.Name.ToLower().StartsWith(tenant.ToLower()))
                                .FirstOrDefault();

                if (null == type)
                    throw new KeyNotFoundException("No instance found for the given tenant.");

                return (TInterface)serviceProvider.GetService(type);
            });
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}
