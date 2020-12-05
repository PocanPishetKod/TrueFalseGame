using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Application;
using TrueFalse.Auth;
using TrueFalse.Repository;

namespace TrueFalse.DependencyInjection
{
    public static class Registrator
    {
        public static IServiceCollection AddTrueFalseGame(this IServiceCollection services)
        {
            return services.AddTrueFalseApplication()
                .AddRepositories()
                .AddAuth();
        }

        public static IServiceCollection AddTrueFalseGameForUnitTasts(this IServiceCollection services)
        {
            return services.AddTrueFalseApplication()
                .AddRepositoriesForUnitTests();
        }
    }
}
