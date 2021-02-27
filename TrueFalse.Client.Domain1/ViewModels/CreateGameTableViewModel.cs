using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Api;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class CreateGameTableViewModel : BaseViewModel
    {
        private readonly IStateService _stateService;
        private readonly INavigator _navigator;
        private readonly IDispatcher _dispatcher;
        private readonly IBlockUIService _blockUIService;
        private readonly IMainHubApi _mainHubApi;

        public ICreatableGameTable GameTable { get; private set; }

        public CreateGameTableViewModel(IStateService stateService, INavigator navigator, IDispatcher dispatcher, IBlockUIService blockUIService, IMainHubApi mainHubApi)
        {
            _stateService = stateService;
            _navigator = navigator;
            _dispatcher = dispatcher;
            _blockUIService = blockUIService;
            _mainHubApi = mainHubApi;

            CreateDefaultGameTable();
        }

        private void CreateDefaultGameTable()
        {
            GameTable = new GameTable()
            {
                Owner = _stateService.GetSavedPlayer(),
                Type = GameTableType.Cards36And4Players,
                Name = GameTableNameGenerator.Generate()
            };
        }

        public void Create()
        {
            if (_stateService.AlreadyPlaying)
            {
                throw new Exception("Игрок уже находится за игровым столом");
            }

            if (string.IsNullOrWhiteSpace(GameTable.Name))
            {
                GameTable.Name = GameTableNameGenerator.Generate();
            }

            _blockUIService.StartBlocking();

            _mainHubApi.CreateGameTable(new CreateGameTableParams()
            { 
                Name = GameTable.Name,
                OwnerId = _stateService.GetSavedPlayer().Id,
                GameTableType = GameTable.Type
            })
                .Then(response =>
                {
                    if (response.Succeeded)
                    {
                        _stateService.SetGameTable(GameTable as GameTable);

                        _dispatcher.Invoke(() =>
                        {
                            Navigate<GameTableViewModel>();
                        });
                    }
                })
                .Finally(() =>
                {
                    _dispatcher.Invoke(() =>
                    {
                        _blockUIService.StopBlocking();
                    });
                });
        }

        public void Cancel()
        {
            Navigate<GameTablesViewModel>();
        }

        public override Task Navigate<TViewModel>()
        {
            if (typeof(TViewModel).Name.Equals(nameof(GameTablesViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate<GameTablesViewModel>();
            }
            else
            {
                throw new Exception("Ошибка навигации");
            }

            return Task.CompletedTask;
        }
    }
}
