using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Commands;
using TrueFalse.Client.Domain.Commands.Parameters;
using TrueFalse.Client.Domain.Extensions;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Api;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class GameTablesViewModel : BaseViewModel
    {
        private readonly IStateService _stateService;
        private readonly INavigator _navigator;
        private readonly IBlockUIService _blockUIService;
        private readonly IDispatcher _dispatcher;
        private MainHubClient _mainHubClient;
        private ObservableCollection<GameTable> _gameTables;
        private ICommand<LoadGameTablesParams> _loadGameTablesCommand;

        public ObservableCollection<GameTable> GameTables => _gameTables;

        public GameTablesViewModel(IStoreFolderPathProvider storeFolderPathProvider, IStateService stateService,
            INavigator navigator, IBlockUIService blockUIService, IDispatcher dispatcher)
        {
            _gameTables = new ObservableCollection<GameTable>();
            _stateService = stateService;
            _navigator = navigator;
            _blockUIService = blockUIService;
            _dispatcher = dispatcher;

            Initialize();
        }

        public ICommand<LoadGameTablesParams> LoadGameTablesCommand
        {
            get
            {
                if (_loadGameTablesCommand == null)
                {
                    _loadGameTablesCommand = new LoadGameTablesCommand(this);
                }

                return _loadGameTablesCommand;
            }
        }

        private void Initialize()
        {
            var currentPlayer = _stateService.GetSavedPlayer();
            _mainHubClient = new MainHubClient(currentPlayer.Token);
            Task.Run(async () => await _mainHubClient.Connect());
        }

        public void LoadGameTables()
        {
            _mainHubClient.GetGameTables(new GetGameTablesParams())
                .Then((response) =>
                {
                    if (response.Succeeded)
                    {
                        response.GameTables.ForEach(g => _gameTables.Add(g.ToModel()));
                    }
                });
        }

        public void JoinToGameTable(GameTable gameTable)
        {
            if (_stateService.AlreadyPlaying)
            {
                throw new Exception("Пользователь уже находится в игровой комнате");
            }

            _blockUIService.StartBlocking();
            _mainHubClient.JoinToGameTable(new JoinToGameTableParams() { GameTableId = gameTable.Id })
                .Then((response) =>
            {
                if (response.Succeeded)
                {
                    _dispatcher.Invoke(() =>
                    {
                        _blockUIService.StopBlocking();

                        _stateService.SetGameTable(gameTable);
                        Navigate(nameof(GameTableViewModel));
                    });
                }
            });
        }

        public void GoToCreatingGameTable()
        {
            Navigate(nameof(CreateGameTableViewModel));
        }

        public override Task Navigate(string viewModelName)
        {
            if (viewModelName.Equals(nameof(MainMenuViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate(nameof(MainMenuViewModel));
            }
            else if (viewModelName.Equals(nameof(CreateGameTableViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate(nameof(CreateGameTableViewModel));
            }
            else if (viewModelName.Equals(nameof(GameTableViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate(nameof(GameTableViewModel));
            }
            else
            {
                throw new Exception("Ошибка навигации");
            }

            return Task.CompletedTask;
        }
    }
}
