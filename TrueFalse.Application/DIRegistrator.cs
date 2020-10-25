using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Application.Services;

namespace TrueFalse.Application
{
    public static class DIRegistrator
    {
        public static IServiceCollection AddTrueFalseApplication(this IServiceCollection services)
        {
            return services.AddSingleton<GameTableService>()
                .AddSingleton<PlayerService>()
                .AddSingleton<CreateGameTableChecker>();
        }
    }
}
