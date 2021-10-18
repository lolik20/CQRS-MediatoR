using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using CQRS_MediatoR.Api.Middlewares;
using CQRS_MediatoR.Api.Validation;
using CQRS_MediatoR.DAL.Configuration;
using Mapper = CQRS_MediatoR.Api.Configuration.Mapper;
using OpenApiSchema = Microsoft.OpenApi.Models.OpenApiSchema;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;
using CQRS_MediatoR.BL.Configuration;
using CQRS_MediatoR.BL.Hubs;

namespace CQRS_MediatoR.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<UtgLCContext>(options =>
            //{
            //	options.UseNpgsql(configuration.GetConnectionString("UTGLCDatabase"));
            //});

            services.AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter());
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .RegisterFluentValidation();

            services.AddSwaggerDocument(c =>
            {
                //c.AddSecurity("JWT", new OpenApiSecurityScheme
                //{
                //	Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //	Name = "Cookie",
                //	In = OpenApiSecurityApiKeyLocation.Cookie,
                //	Type = OpenApiSecuritySchemeType.ApiKey
                //	//Scheme = "Bearer"
                //});
                c.OperationProcessors.Add(
                   new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                c.PostProcess = doc =>
                {
                    doc.Info.Version = "v1";
                    doc.Info.Title = "Utg LC Api";
                    doc.Info.Description = "The documentation Utg LC API";
                };
            });
            services.AddLogging();

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "UTGJWTLC";
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = false;

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.ConfigureDal(configuration)
              .ConfigureBL();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.Advanced.AllowAdditiveTypeMapCreation = true;
                Mapper.ConfigureMappings(cfg);
            });

            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Prod")
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }



            //services.AddHangfire(x => x.UsePostgreSqlStorage(configuration.GetConnectionString("UTGLCDatabase")));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder builder, IWebHostEnvironment env)
        {

            builder.UseRequestResponseLogging();

            if (env.IsDevelopment())
            {
                builder.UseDeveloperExceptionPage();
            }
            builder.UseCors("MyPolicy");
            //builder.UseHangfireServer();
            //ConfigureJobs();
            builder.UseRouting();


            builder.UseAuthentication();
            builder.UseAuthorization();
            builder.UseMiddleware<ErrorHandlingMiddleware>();
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("api/chat");
            });


            var swaggerPath = $"/clients/swagger";

            builder.UseOpenApi(options =>
            {
                //options.PostProcess = (document, _) => document.Schemes = new[] { OpenApiSchema };
                options.Path = $"{swaggerPath}/v1/swagger.json";
            });
            builder.UseSwaggerUi3(options =>
            {
                options.Path = swaggerPath;
                options.DocumentPath = $"/{swaggerPath}/" + "v1" + "/swagger.json";
            });

            builder.UseOpenApi(options =>
                options.Path = swaggerPath
            );
            builder.UseSwaggerUi3(options =>
            {
                options.Path = swaggerPath;
                options.DocumentPath = $"{swaggerPath}/ " + "v1" + "/swagger.json";
            });

            DAL.Configuration.Startup.Initialize(builder);
        }


        //private void ConfigureJobs()
        //{
        //	var timtetable = configuration.GetValue<string>("Jobs:AcquaintancePeriod:Timetable")?.ToString();
        //	RecurringJob.AddOrUpdate<AcquaintancePeriodJob>(nameof(AcquaintancePeriodJob), x => x.Start(), timtetable, queue: "default");

        //	//timtetable = configuration.GetValue<string>("Jobs:RemoveDrafts:Timetable")?.ToString();
        //	//RecurringJob.AddOrUpdate<RemoveDraftsJob>(nameof(RemoveDraftsJob), x => x.Start(), timtetable, queue: "default");
        //}

    }
}
