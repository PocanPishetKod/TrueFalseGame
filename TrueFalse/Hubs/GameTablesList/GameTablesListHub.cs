using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Application.Services;
using TrueFalse.Hubs.GameTablesList.Dtos;

namespace TrueFalse.Hubs.GameTablesList
{
    public class GameTablesListHub : Hub<IGameTablesListClient>
    {
        private readonly GameTableService _gameTableService;
        private readonly PlayerService _playerService;

        public GameTablesListHub(GameTableService gameTableService, PlayerService playerService)
        {
            _gameTableService = gameTableService;
            _playerService = playerService;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        private async Task GameTableCreated(GameTableDto gameTable)
        {
            await Clients.AllExcept(Context.ConnectionId).OnCreatedNewGameTable(new OnCreatedNewGameTableParams()
            {
                GameTable = gameTable
            });
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

                await GameTableCreated(dto);
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
    }
}
