using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Repository.Repositories;

namespace TrueFalse.Repository
{
    public static class DIRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddSingleton<IGameTableRepository, GameTableRepository>()
                .AddSingleton<IPlayerRepository, PlayerRepository>();
        }
    }
}
