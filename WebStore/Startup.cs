using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Dal.Context;
using WebStore.Dal.DataInit;
using WebStore.Data;
using WebStore.Domain.Identity;
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
            services.AddDbContext<WebStoreContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("Default"),
                    o => o.MigrationsAssembly("WebStore.Dal"))
                );

            services.AddTransient<WebStoreDataInit>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(o =>
            {
#if DEBUG
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 3;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredUniqueChars = 3;
#endif
                o.User.RequireUniqueEmail = false;
                o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                o.Lockout.AllowedForNewUsers = false;
                o.Lockout.MaxFailedAccessAttempts = 10;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = "KanadeiarWebStore";
                o.Cookie.HttpOnly = true;
                o.ExpireTimeSpan = TimeSpan.FromDays(10);

                o.LoginPath = "/Account/Login";
                o.LogoutPath = "/Account/Logout";
                o.AccessDeniedPath = "/Account/AccessDenied";

                o.SlidingExpiration = true;
            });

            services.AddSingleton<TestData>();

            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, DatabaseProductData>();
            //services.AddSingleton<IWorkerData, InMemoryWorkerData>();
            services.AddScoped<IWorkerData, DatabaseWorkerData>();

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

            app.UseAuthentication();
            app.UseAuthorization();

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
