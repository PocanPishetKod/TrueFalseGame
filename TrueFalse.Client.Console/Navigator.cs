using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.ConsoleApp
{
    public class Navigator : INavigator
    {
        private readonly App _app;

        public Navigator(App app)
        {
            _app = app;
        }

        public void Navigate(string viewModelName)
        {
            if (viewModelName.Equals(nameof(GameTablesViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _app.GoToGameTablesView();
            }
        }
    }
}
