using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CQRS_MediatoR.BL.Configuration
{
    public static class ModuleInitializer
    {
        public static IServiceCollection ConfigureBL(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSignalR(); 
            var memoryCache = new MemoryCache(
                Options.Create(new MemoryCacheOptions())
            );
            services.AddSingleton<IMemoryCache>(memoryCache);
            return services;
        }

	}
}
