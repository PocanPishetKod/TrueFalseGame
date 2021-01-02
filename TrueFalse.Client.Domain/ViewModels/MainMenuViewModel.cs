using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Commands;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Services;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly IStoreFolderPathProvider _storeFolderPathProvider;
        private readonly INavigator _navigator;
        private readonly IStateService _stateService;
        private ICommand<string> _navigateCommand;

        public MainMenuViewModel(IStoreFolderPathProvider storeFolderPathProvider, INavigator navigator, IStateService stateService)
        {
            _storeFolderPathProvider = storeFolderPathProvider;
            _navigator = navigator;
            _stateService = stateService;

            AuthenticateBackground();
        }

        public ICommand<string> NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new NavigateCommand(this);
                }

                return _navigateCommand;
            }
        }

        private void AuthenticateBackground()
        {
            var authService = new AuthService(new SaveService(_storeFolderPathProvider.ProvidePath()));
            authService.Authenticate().ContinueWith(task =>
            {
                _stateService.SetPlayer(task.Result);
            });
        }

        public override void Navigate(string viewModelName)
        {
            if (viewModelName.Equals(nameof(GameTablesViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate(nameof(GameTablesViewModel));
            }
            else
            {
                throw new Exception("Ошибка навигации");
            }
        }
    }
}
