using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<AppSetting>(x => new AppSetting
            {
                EnableDeveloperExceptions = _configuration.GetValue<bool>("EnableDeveloperExceptions")
            });
            services.AddTransient<FormattingService>();
            
            services.AddMvc();
            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionString = _configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });
            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionString = _configuration.GetConnectionString("IdentityDataContext");
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            AppSetting setting)
        {

            app.UseExceptionHandler("/error.html");
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            if (setting.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseFileServer();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapGet("/invalid", context =>
                {
                    throw new Exception("Error!");
                });

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("How are you?");
                //});

                //endpoints.MapGet("/hello", async context =>
                //{
                //    await context.Response.WriteAsync("Hello ASP.NET Core!");
                //});
            });
        }
    }
}
