using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.WebAPI;
using WebStore.Services.Data;
using WebStore.Services.Services;
using WebStore.WebAPI.Client.Identity;
using WebStore.WebAPI.Client.Orders;
using WebStore.WebAPI.Client.Person;
using WebStore.WebAPI.Client.Product;
using WebStore.WebAPI.Client.Values;

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
            services.AddIdentity<User, IdentityRole>()
                .AddIdentityWebStoreAPIClients()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(o =>
            {
//#if DEBUG
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 3;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredUniqueChars = 3;
//#endif
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

            services.AddScoped<ICartService, InCookiesCartService>();

            services.AddHttpClient("WebStoreAPI", c => c.BaseAddress = new Uri(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IWorkerData, WorkerApiClient>()
                .AddTypedClient<IProductData, ProductApiClient>()
                .AddTypedClient<IOrderService, OrderApiClient>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, IServiceProvider service*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.Map("/HelloGeekbrains",
                context => context.Run(async request => await request.Response.WriteAsync("Hello Geekbrains!")));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name : "areas",
                    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
