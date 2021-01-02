using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.ConsoleApp.Views;
using TrueFalse.Client.Domain;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Services;
using TrueFalse.Client.Domain.ViewModels;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.ConsoleApp
{
    public class App
    {
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private BaseView _currentView;

        public App(IServiceCollection services)
        {
            _services = services;
        }

        public void Start()
        {
            _services.AddSingleton<IStoreFolderPathProvider>(new StoreFolderProvider(@"C:\TrueFalse saves"))
                .AddTransient<INavigator, Navigator>()
                .AddTransient<MainMenuViewModel>()
                .AddTransient<GameTablesViewModel>()
                .AddSingleton<IStateService>(new StateService())
                .AddSingleton(this);

            _serviceProvider = _services.BuildServiceProvider();

            GoToMainView();
        }

        public void GoToMainView()
        {
            if (_currentView != null)
            {
                _currentView.Stop();
            }

            _currentView = new MainMenuView(_serviceProvider.GetService<MainMenuViewModel>());
            _currentView.Start();
        }

        public void GoToGameTablesView()
        {
            if (_currentView != null)
            {
                _currentView.Stop();
            }

            _currentView = new GameTablesListView(_serviceProvider.GetService<GameTablesViewModel>());
            _currentView.Start();
        }
    }
}
