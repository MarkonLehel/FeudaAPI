using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FeudaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Changes all HTTP requests into HTTPS requests
            app.UseHttpsRedirection();

            //This serves a static file request from the wwwroot folder, or by default returns index.html.
            app.UseFileServer();

            //Creates the correct routing paths for endpoints on the server
            app.UseRouting();

            //TODO: setup UseEndpoints for signalR hubs and Razor pages(api docs, info)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //Set up SignalR for API here
            });

/*            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello there");
            });*/
        }
    }
}
