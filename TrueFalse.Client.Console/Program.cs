using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.ConsoleApp
{
    class Program
    {
        private static App _app;

        static void Main(string[] args)
        {
            _app = new App(new ServiceCollection());
            _app.Start();
        }
    }
}
