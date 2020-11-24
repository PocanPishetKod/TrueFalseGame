using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Logger
{
    public static class LoggerRegistrator
    {
        public static void AddLogging(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
        }
    }
}
