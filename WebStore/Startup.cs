using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Dal.Context;
using WebStore.Dal.DataInit;
using WebStore.Data;
using WebStore.Infrastructure.Interface;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<WebStoreDataInit>();

            services.AddSingleton<TestData>();

            services.AddSingleton<IWorkerData, InMemoryWorkerData>();
            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, DatabaseProductData>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            using (var scope = service.CreateScope())
                scope.ServiceProvider.GetRequiredService<WebStoreDataInit>().RecreateDatabase().InitData();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<DebugMiddleware>();

            app.Map("/HelloGeekbrains",
                context => context.Run(async request => await request.Response.WriteAsync("Hello Geekbrains!")));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
