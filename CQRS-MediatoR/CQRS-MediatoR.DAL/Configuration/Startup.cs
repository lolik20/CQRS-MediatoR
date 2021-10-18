using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CQRS_MediatoR.DAL.SqlContext;

namespace CQRS_MediatoR.DAL.Configuration
{
    public static class Startup
    {

        public static void Initialize(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<CQRSContext>();
            context.Database.Migrate();
        }

	}
}
