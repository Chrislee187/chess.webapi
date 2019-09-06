using System;
using System.IO;
using chess.engine;
using chess.webapi.Filters;
using chess.webapi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Serilog;
using Serilog.Sinks.File;

namespace chess.webapi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(opts =>
            {
                opts.Filters.Add(typeof(ActionPerformanceFilter)); 

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Chess.WebAPI";
            });

            services.AddScoped<IChessService, ChessGameService>();

            services.AddChessDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseCors(builder => {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(s => true);
//                    .WithOrigins("http://localhost:5000");
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
