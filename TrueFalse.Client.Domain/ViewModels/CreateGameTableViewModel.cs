using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class CreateGameTableViewModel : BaseViewModel
    {
        private readonly IStateService _stateService;
        private readonly INavigator _navigator;
        private readonly IDispatcher _dispatcher;
        private readonly IBlockUIService _blockUIService;

        public ICreatableGameTable GameTable { get; private set; }

        public CreateGameTableViewModel(IStateService stateService, INavigator navigator, IDispatcher dispatcher, IBlockUIService blockUIService)
        {
            _stateService = stateService;
            _navigator = navigator;

            CreateDefaultGameTable();
        }

        private void CreateDefaultGameTable()
        {
            GameTable = new GameTable()
            {
                Owner = _stateService.GetSavedPlayer(),
                Type = GameTableType.Cards36And3Players
            };
        }

        public void Create()
        {
            
        }

        public void Cancel()
        {
            Navigate(nameof(GameTablesViewModel));
        }

        public override Task Navigate(string viewModelName)
        {
            if (viewModelName.Equals(nameof(GameTablesViewModel), StringComparison.CurrentCultureIgnoreCase))
            {
                _navigator.Navigate(nameof(GameTablesViewModel));
            }
            else
            {
                throw new Exception("Ошибка навигации");
            }

            return Task.CompletedTask;
        }
    }
}
