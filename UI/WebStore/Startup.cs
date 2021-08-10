using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Domain.Identity;
using WebStore.Hubs;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.WebAPI;
using WebStore.Services.Data;
using WebStore.Services.InCookies;
using WebStore.Services.Services;
using WebStore.WebAPI.Client;
using WebStore.WebAPI.Client.Identity;

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

            //services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<ICartStore, InCookiesCartStore>();
            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IWantedStore, InCookiesWantedStore>();
            services.AddScoped<IWantedService, WantedService>();

            services.AddScoped<ICompareStore, InCookiesCompareStore>();
            services.AddScoped<ICompareService, CompareService>();

            services.AddHttpClient("WebStoreAPI", c => c.BaseAddress = new Uri(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IWorkerData, WorkerApiClient>()
                .AddTypedClient<IProductData, ProductApiClient>()
                .AddTypedClient<IOrderService, OrderApiClient>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddSignalR();

            services.AddServerSideBlazor();
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
                endpoints.MapHub<ChatHub>("/chat");

                endpoints.MapControllerRoute(
                    name : "areas",
                    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                
                endpoints.MapDefaultControllerRoute();

                endpoints.MapBlazorHub();
            });
        }
    }
}
