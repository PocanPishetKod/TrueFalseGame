using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Repository.MongoDb;
using TrueFalse.Repository.Repositories;

namespace TrueFalse.Repository
{
    public static class DIRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new NullReferenceException("Не удалось взять конфигурацию из провайдера");
            }

            var mongoDbSettings = MongoDbSettings.Create(configuration);

            return services.AddTransient<IGameTableRepository, GameTableRepository>()
                .AddTransient<IPlayerRepository, MongoDbPlayerRepository>()
                .AddTransient<MongoDbContext>()
                .AddSingleton(mongoDbSettings);
        }

        public static IServiceCollection AddRepositoriesForUnitTests(this IServiceCollection services)
        {
            return services.AddSingleton<IGameTableRepository, GameTableRepository>()
                .AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();
        }
    }
}
