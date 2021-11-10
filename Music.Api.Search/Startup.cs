using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Music.Api.Search.Interfaces;
using Music.Api.Search.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Net.Http.Headers;
using Polly;

namespace Music.Api.Search
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // may use app settings for these values.
            var productName = new ProductInfoHeaderValue("testingapp", "1.0");
            var productComment = new ProductInfoHeaderValue("(+testingapp@test.com)");

            services.AddScoped<IArtistsService, ArtistsService>();
            services.AddScoped<IReleasesService, ReleasesService>();
            services.AddScoped<ISearchService, SearchService>();
            
            // http clients are setup to retry once with a wait of 500ms
            services.AddHttpClient("ArtistsService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Artists"]);
                config.DefaultRequestHeaders.UserAgent.Add(productName);
                config.DefaultRequestHeaders.UserAgent.Add(productComment);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(1, _ => TimeSpan.FromMilliseconds(500)));
            services.AddHttpClient("ReleasesService", config =>
            {
                config.BaseAddress = new Uri(Configuration["Services:Releases"]);
                config.DefaultRequestHeaders.UserAgent.Add(productName);
                config.DefaultRequestHeaders.UserAgent.Add(productComment);
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(1, _ => TimeSpan.FromMilliseconds(500)));
            
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
