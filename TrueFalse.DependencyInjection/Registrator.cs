using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Application.Services;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Repository.Repositories;

namespace TrueFalse.DependencyInjection
{
    public static class Registrator
    {
        public static IServiceCollection AddTrueFalseGame(this IServiceCollection services)
        {
            return services.AddSingleton<GameTableService>()
                .AddSingleton<PlayerService>()
                .AddSingleton<IGameTableRepository, GameTableRepository>()
                .AddSingleton<IPlayerRepository, PlayerRepository>();
        }
    }
}
