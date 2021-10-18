using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CQRS_MediatoR.DAL.SqlContext;

namespace CQRS_MediatoR.DAL.Configuration
{
    public static class ModuleInitializer
    {
        public static IServiceCollection ConfigureDal(this IServiceCollection services, IConfiguration configuration)
        {
            SetSettings(services, configuration);
            AddDependenciesToContainer(services);

            return services;
        }

        private static void SetSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CQRSContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
            });
        }

        private static void AddDependenciesToContainer(IServiceCollection services)
        {
            //services.AddTransient<IUserCertificateRepository, UserCertificateRepository>();
          

        }

    }
}
