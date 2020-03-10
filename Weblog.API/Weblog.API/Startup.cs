using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weblog.API.DbContexts;
using Weblog.API.Helpers;
using Weblog.API.Services;

namespace Weblog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string TrustedCorsPolicy = "_trustedCorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .ConfigureApiBehaviorOptions(setupAction =>
                    {
                        setupAction.InvalidModelStateResponseFactory = context =>
                        {
                            return ErrorHandler.UnprocessableEntity(context.ModelState,
                                                                    context.HttpContext);
                        };
                    });

            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                      .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.sepehr.hateoas+json");
                }
            });

            var connectionString = Configuration.GetConnectionString("WeblogDB");
            services.AddDbContext<WeblogContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IWeblogDataRepository, WeblogDataRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var corsOrigin = Configuration.GetValue<string>("CORS_Origin");
            services.AddCors(options =>
            {
                options.AddPolicy(TrustedCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(corsOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseCors(TrustedCorsPolicy);

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
