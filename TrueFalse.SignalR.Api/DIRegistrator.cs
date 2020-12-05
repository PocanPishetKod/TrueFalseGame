using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Api
{
    public static class DIRegistrator
    {
        public static IServiceCollection AddSignalRApi(this IServiceCollection services)
        {
            services.AddSignalR(config =>
            {
                config.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
                config.KeepAliveInterval = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
