using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Application.Services;
using TrueFalse.Hubs.Main.Dtos;

namespace TrueFalse.Hubs.Main
{
    public class MainHub : Hub<IMainHubClient>
    {
        private readonly GameTableService _gameTableService;
        private readonly PlayerService _playerService;

        public MainHub(GameTableService gameTableService, PlayerService playerService)
        {
            _gameTableService = gameTableService;
            _playerService = playerService;
        }

        private async Task NotifyAboutPlayerJoined()
        {

        }

        private async Task NotifyAboutPlayerLeaved()
        {

        }

        private async Task NotifyGameStarted()
        {

        }

        private async Task NotifyGameTableCreated(GameTableDto gameTable)
        {
            await Clients.AllExcept(Context.ConnectionId).OnCreatedNewGameTable(new OnCreatedNewGameTableParams()
            {
                GameTable = gameTable
            });
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task GetGameTables(GetGameTablesParams @params)
        {
            var result = _gameTableService.GetGameTablesTest();

            await Clients.Caller.ReceiveGameTables(new ReceiveGameTableParams()
            {
                GameTables = result.ToList()
            });
        }

        public async Task CreateGameTable(CreateGameTableParams @params)
        {
            try
            {
                var dto = _gameTableService.CreateGameTable(@params.OwnerId, @params.Name, @params.PlayersCount, @params.CardsCount);

                await Clients.Caller.ReceiveCreateGameTableResult(new ReceiveCreateGameTableResultParams()
                {
                    GameTableId = dto.Id,
                    IsSucceeded = true
                });

                await NotifyGameTableCreated(dto);
            }
            catch (Exception)
            {
                await Clients.Caller.ReceiveCreateGameTableResult(new ReceiveCreateGameTableResultParams()
                {
                    GameTableId = null,
                    IsSucceeded = false
                });
            }
        }

        public async Task JoinToGameTable(JoinToGameTableParams @params)
        {
            try
            {
                
            }
            catch (Exception)
            {

            }
        }

        public async Task LeaveFromGameTable()
        {

        }

        public async Task StartGame()
        {

        }

        public async Task MakeFirstMove(MakeFirstMoveParams @params)
        {

        }

        public async Task MakeBeliveMove(MakeBeliveMoveParams @params)
        {

        }

        public async Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params)
        {

        }
    }
}
