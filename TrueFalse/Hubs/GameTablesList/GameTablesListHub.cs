using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Services;

namespace TrueFalse.Hubs.GameTablesList
{
    public class GameTablesListHub : Hub<IGameTablesListClient>
    {
        private readonly GameTableService _gameTableService;

        public GameTablesListHub(GameTableService gameTableService)
        {
            _gameTableService = gameTableService;
        }

        public async Task GetGameTables(GetGameTablesParams @params)
        {
            var result = _gameTableService.GetGameTables();

            await Clients.Caller.ReceiveGameTables(new ReceiveGameTableParams()
            {
                GameTables = result
            });
        }

        public async Task CreateGameTable(CreateGameTableParams @params)
        {
            try
            {
                var id = _gameTableService.CreateGameTable(@params.OwnerId, @params.Name, @params.PlayersCount, @params.CardsCount);

                await Clients.Caller.ReceiveCreateGameTableResult(new ReceiveCreateGameTableResultParams()
                {
                    GameTableId = id,
                    IsSucceeded = true
                });
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
