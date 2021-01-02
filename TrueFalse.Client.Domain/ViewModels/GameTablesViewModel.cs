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
        private MainHubClient _mainHubClient;
        private ObservableCollection<GameTable> _gameTables;
        private ICommand<LoadGameTablesParams> _loadGameTablesCommand;

        public ObservableCollection<GameTable> GameTables => _gameTables;

        public GameTablesViewModel(IStoreFolderPathProvider storeFolderPathProvider, IStateService stateService)
        {
            _gameTables = new ObservableCollection<GameTable>();
            _stateService = stateService;

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
            _mainHubClient.Connect().ConfigureAwait(false);
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

        public override void Navigate(string viewModelName)
        {
            throw new NotImplementedException();
        }
    }
}
