using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebStore.Dal.Context;
using WebStore.Dal.DataInit;
using WebStore.Dal.Interfaces;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.WebAPI;
using WebStore.Services.Data;
using WebStore.Services.Services;

namespace WebStore.WebAPI
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
            var databaseName = Configuration["Database"];
            switch (databaseName)
            {
                case "MSSQL": 
                    services.AddDbContext<WebStoreContext>(opt => 
                        opt.UseSqlServer(Configuration.GetConnectionString("MSSQL"),
                            o => o.MigrationsAssembly("WebStore.Dal")));
                    break;
                case "SQLite":
                    services.AddDbContext<WebStoreContext>(opt =>
                        opt.UseSqlite(Configuration.GetConnectionString("SQLite"),
                            o => o.MigrationsAssembly("WebStore.Dal.Sqlite")));
                    break;
            }

            services.AddTransient<IWebStoreDataInit, WebStoreDataInit>();

            services.AddIdentity<User, IdentityRole>()
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

            services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<IProductData, DatabaseProductData>();
            services.AddScoped<IWorkerData, DatabaseWorkerData>();
            services.AddScoped<IOrderService, DatabaseOrderService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.WebAPI", Version = "v1" });

                const string webStoreDomainXml = "WebStore.Domain.xml";
                const string webStoreWebApiXml = "WebStore.WebAPI.xml";
                const string debugPath = "bin/debug/net5.0";

                if(File.Exists(webStoreDomainXml))
                    c.IncludeXmlComments(webStoreDomainXml);
                else if(File.Exists(Path.Combine(debugPath, webStoreDomainXml)))
                    c.IncludeXmlComments(Path.Combine(debugPath, webStoreDomainXml));

                if (File.Exists(webStoreWebApiXml))
                    c.IncludeXmlComments(webStoreWebApiXml);
                else if (File.Exists(Path.Combine(debugPath, webStoreWebApiXml)))
                    c.IncludeXmlComments(Path.Combine(debugPath, webStoreWebApiXml));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            using (var scope = service.CreateScope())
                scope.ServiceProvider.GetRequiredService<IWebStoreDataInit>()
                    //.RecreateDatabase()
                    .InitData();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
