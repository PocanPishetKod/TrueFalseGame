using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Commands;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Api;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly IStoreFolderPathProvider _storeFolderPathProvider;
        private readonly INavigator _navigator;
        private readonly IStateService _stateService;
        private readonly IBlockUIService _blockUIService;
        private readonly IHubClientConnection _hubClientConnection;
        private ICommand<string> _navigateCommand;
        private Task _authTask;
        private Task _connectTask;

        public MainMenuViewModel(IStoreFolderPathProvider storeFolderPathProvider, INavigator navigator, IStateService stateService,
            IBlockUIService blockUIService, IHubClientConnection hubClientConnection)
        {
            _storeFolderPathProvider = storeFolderPathProvider;
            _navigator = navigator;
            _stateService = stateService;
            _blockUIService = blockUIService;
            _hubClientConnection = hubClientConnection;

            AuthenticateBackground();
        }

        private void OpenConnection(string token)
        {
            _connectTask = _hubClientConnection.WithAccessToken(token).Connect();
        }

        private void AuthenticateBackground()
        {
            if (!_stateService.IsAuthenticated)
            {
                var authService = new AuthService(new SaveService(_storeFolderPathProvider.ProvidePath()));
                _authTask = authService.Authenticate().ContinueWith(task =>
                {
                    _stateService.SetPlayer(task.Result);
                    OpenConnection(task.Result.Token);
                });
            }
        }

        public override async Task Navigate<TViewModel>()
        {
            if (typeof(TViewModel).Name.Equals(nameof(GameTablesViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                if (_authTask != null && !_authTask.IsCompleted)
                {
                    _blockUIService.StartBlocking();
                    await _authTask;
                    _blockUIService.StopBlocking();
                }

                if (!_connectTask.IsCompleted)
                {
                    _blockUIService.StartBlocking();
                    await _connectTask;
                    _blockUIService.StopBlocking();
                }

                _navigator.Navigate<GameTablesViewModel>();
            }
            else
            {
                throw new Exception("Ошибка навигации");
            }
        }
    }
}
