using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Exceptions;
using TrueFalse.Client.Domain.Interfaces;
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
            if (_stateService.GetGameTable().IsStarted)
            {
                throw new TrueFalseGameException("Игра уже идет");
            }

            if (!_stateService.GetGameTable().CanStart)
            {
                throw new TrueFalseGameException("Недостаточное количество пользователей");
            }

            if (_stateService.GetGameTable().Owner.Id == _stateService.GetSavedPlayer().Id)
            {
                _blockUIService.StartBlocking();

                _mainHubApi.StartGame(new StartGameParams())
                    .Then(response =>
                    {
                        if (response.Succeeded)
                        {
                            // todo добавить возвращение тасованных карт всем игрокам
                        }
                    })
                    .Finally(() =>
                    {
                        _blockUIService.StopBlocking();
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
