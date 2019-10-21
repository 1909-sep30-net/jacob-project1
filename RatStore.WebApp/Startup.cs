using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RatStore.Logic;

/* Scaffold command:
dotnet ef dbcontext scaffold Name=RatStoreDB Microsoft.EntityFrameworkCore.SqlServer --startup-project ../RatStore.WebApp --force --output-dir Entities
*/

namespace RatStore.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.Entities.jacobproject0Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("RatStoreDB"));
            });

            services.AddScoped<IDataStore, DatabaseStore>();
            services.AddScoped<Models.IBaseViewModel, Models.BaseViewModel>();

            // This lets dependency injection use Session
            services.AddHttpContextAccessor();

            // These set up session to use the cache
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // This activates the session (?)
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=RatStore}/{action=Index}/{id?}");
            });
        }
    }
}
