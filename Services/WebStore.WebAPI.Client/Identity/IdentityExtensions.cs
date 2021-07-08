using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain.Identity;

namespace WebStore.WebAPI.Client.Identity
{
    public static class IdentityExtensions
    {
        /// <summary> Добавить сразу пачку клиентов системы Identity к сервисам </summary>
        /// <param name="services">Сервисы</param>
        /// <returns>Сервис</returns>
        public static IServiceCollection AddIdentityWebStoreAPIClients(this IServiceCollection services)
        {
            services.AddHttpClient("WebStoreAPI", (s, c) => 
                    c.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>()["WebAPI"]))
                .AddTypedClient<IUserStore<User>, UserApiClient>()
                .AddTypedClient<IUserRoleStore<User>, UserApiClient>()
                .AddTypedClient<IUserPasswordStore<User>, UserApiClient>()
                .AddTypedClient<IUserEmailStore<User>, UserApiClient>()
                .AddTypedClient<IUserPhoneNumberStore<User>, UserApiClient>()
                .AddTypedClient<IUserTwoFactorStore<User>, UserApiClient>()
                .AddTypedClient<IUserClaimStore<User>, UserApiClient>()
                .AddTypedClient<IUserLoginStore<User>, UserApiClient>()
                .AddTypedClient<IRoleStore<IdentityRole>, RoleApiClient>();

            return services;
        }

        public static IdentityBuilder AddIdentityWebStoreAPIClients(this IdentityBuilder builder)
        {
            builder.Services.AddIdentityWebStoreAPIClients();

            return builder;
        }
    }
}
