using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Auth.Key;
using TrueFalse.Auth.Services;

namespace TrueFalse.Auth
{
    public static class DIRegistrator
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new NullReferenceException("Не удалось взять конфигурацию из провайдера");
            }

            var jwtConfig = JwtConfiguration.Create(configuration);

            new AuthConfigurator().ConfigureAuth(services, new SymmetricSecurityKeyGenerator(), jwtConfig);

            return services.AddTransient<JwtService>();
        }
    }
}
