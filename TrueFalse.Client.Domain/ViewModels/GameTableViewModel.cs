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

            _mainHubApi.GameStarted += OnGameStarted;
        }

        private void OnGameStarted(OnGameStartedParams @params)
        {
            
        }

        public void StartGame()
        {
            var gameTable = _stateService.GetGameTable();

            if (gameTable.IsStarted)
            {
                throw new TrueFalseGameException("Игра уже идет");
            }

            if (_stateService.GetGameTable().Owner.Id == _stateService.GetSavedPlayer().Id)
            {
                _blockUIService.StartBlocking();

                _mainHubApi.StartGame(new StartGameParams())
                    .Then((response) =>
                    {
                        if (response.Succeeded)
                        {
                            _stateService.GetGameTable().StartGame(response.MoverId.Value, response.PlayerCardsInfo);
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
            if (_stateService.GetSavedPlayer().Id != _stateService.GetGameTable().CurrentGame?.CurrentMover.Id)
            {
                return;
            }

            if (_stateService.FirstMove == null)
            {
                return;
            }

            _blockUIService.StartBlocking();

            _mainHubApi.MakeFirstMove(new MakeFirstMoveParams()
            {
                Rank = (int)_stateService.FirstMove.Rank,
                CardIds = _stateService.FirstMove.SelectedCards.Select(c => c.Id).ToList()
            })
                .Then(response =>
                {
                    if (response.Succeeded)
                    {
                        _stateService.GetGameTable().MakeFirstMove(_stateService.FirstMove, response.NextMoverId.Value);
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
