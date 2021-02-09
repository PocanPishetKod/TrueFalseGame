using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Exceptions;
using TrueFalse.Client.Domain.Interfaces;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Games;
using TrueFalse.Client.Domain.Services;
using TrueFalse.SignalR.Client.Api;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.ViewModels
{
    public class GameTableViewModel : BaseViewModel
    {
        private readonly IStateService _stateService;
        private readonly INavigator _navigator;
        private readonly IDispatcher _dispatcher;
        private readonly IMainHubApi _mainHubApi;
        private readonly IBlockUIService _blockUIService;

        public GameTableViewModel(IStateService stateService, INavigator navigator, IDispatcher dispatcher, IMainHubApi mainHubApi, IBlockUIService blockUIService)
        {
            _stateService = stateService;
            _navigator = navigator;
            _dispatcher = dispatcher;
            _mainHubApi = mainHubApi;
            _blockUIService = blockUIService;
        }

        public void StartGame()
        {
            var gameTable = _stateService.GetGameTable();

            if (gameTable.IsStarted)
            {
                throw new TrueFalseGameException("Игра уже идет");
            }

            _blockUIService.StartBlocking();
            if (_stateService.GetGameTable().Owner.Id == _stateService.GetSavedPlayer().Id)
            {
                _mainHubApi.StartGame(new StartGameParams())
                .Then((response) =>
                {
                    if (response.Succeeded)
                    {
                        _dispatcher.Invoke(() =>
                        {
                            gameTable.CurrentGame = new Game()
                            {
                                CardsPack = new CardsPack() { Count = response.PlayerCardsInfo.Sum(pc => pc.CardsCount) },
                                CurrentMover = gameTable.Players.First(p => p.Player.Id == response.MoverId.Value).Player,
                                GameRounds = new List<GameRound>() { new GameRound() }
                            };
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
        }

        public void MakeFirstMove()
        {

        }

        public void MakeBeliveMove()
        {

        }

        public void MakeDontBeliveMove()
        {

        }

        public void Leave()
        {

        }

        public override Task Navigate(string viewModelName)
        {
            throw new NotImplementedException();
        }
    }
}
