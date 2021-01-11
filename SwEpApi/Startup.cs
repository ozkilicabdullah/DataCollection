using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SwEpApi.Services;
using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Antiforgery;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SwEpApi.Extensions;
using SwEpApi.Services.Tenants;
using SwEpApi.Providers;
using System.Threading;
using Serilog.Events;

namespace SwEpApi
{
    public class Startup
    {

        public readonly IConfiguration Configuration;
        private readonly IHostingEnvironment HostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {

            ThreadPool.SetMinThreads(250, 250);

            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            TypesToRegister = Assembly.Load("SwEpApi")
                                      .GetTypes()
                                      .Where(x => !string.IsNullOrEmpty(x.Namespace))
                                      .Where(x => x.IsClass)
                                      .Where(x => x.Namespace.StartsWith("SwEpApi.Services.Tenants")).ToList();
        }      

        public List<Type> TypesToRegister { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            #region Serilog Elastic Log Configuration

            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.", LogEventLevel.Error | LogEventLevel.Fatal)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration.GetValue<string>("ElasticSearch")))
            {
                TemplateName = "serilog-events-template",
                IndexFormat = "swepapi-log-{0:yyyy.MM}",                
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
            })            
            .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            #endregion

            #region Tenants

            // Tenant Services
            TypesToRegister.ForEach(x => services.AddScoped(x));            
            services.AddScopedDynamic<ITenantService>(TypesToRegister);
            services.AddScoped(typeof(IServicesProvider<>), typeof(ServicesProvider<>));

            #endregion

            #region Configure Services           

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = HostingEnvironment.IsDevelopment() 
                    ? Configuration.GetConnectionString("RedisDevelopment") : Configuration.GetConnectionString("Redis");
                options.InstanceName = "SwEpApiRedisCache";
            });
            
            services.AddHttpContextAccessor();

            services.AddMemoryCache();
            services.AddScoped<IRequestActionService, RequestActionService>();
            services.AddSingleton<ISwTaskServiceClient, SwTaskServiceClient>();

            services.AddSingleton<IConnectionService, ConnectionService>();

            services.Configure<ApiBehaviorOptions>(options => {options.SuppressModelStateInvalidFilter = true; });
            
            #endregion

            #region Antiforgery

            // Angular's default header name for sending the XSRF token.
            //services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            #endregion

            #region Cors Policy

            // Add Cors
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()                        
                       .AllowAnyMethod();
            }));

            // Add framework services.
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });

            #endregion

            #region Authentication

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", jwtBearerOptions =>
            {               
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.IncludeErrorDetails = true;
                jwtBearerOptions.SaveToken = true;                

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,                    
                    ValidateIssuerSigningKey = true,
                    AuthenticationType = "Bearer",
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                };
                
            });

            #endregion

            #region MVC

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #endregion

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.ConfigureCustomExceptionMiddleware();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseAuthentication();          

        }
    }
}
