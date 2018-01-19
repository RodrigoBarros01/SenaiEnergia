using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SenaiEnergia.Domain;
using SenaiEnergia.Infraestructure;

namespace SenaiEnergia
{
    public class Startup : IStartup
    {
        public Action<DbContextOptionsBuilder> DatabaseConfigurationAction { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            DatabaseConfigurationAction = GetDatabaseConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        //IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidationActionFilter));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                });

            services.AddDbContext<Db>(DatabaseConfigurationAction)
                    .AddAutoMapper(typeof(Startup));

            Mapper.AssertConfigurationIsValid();
            services.AddMediatR(typeof(Startup));
            //services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyMethod().WithHeaders("accept", "authorization", "content-type", "origin", "x-custom-header").AllowCredentials()));

            services.AddCors(config =>
            {
                config.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
                });
            });

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors("AllowAll");

            app.UseMvc();
        }

        public Action<DbContextOptionsBuilder> GetDatabaseConfiguration() => options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }
}
