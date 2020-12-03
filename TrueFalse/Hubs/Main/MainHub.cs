using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Application.Services;
using TrueFalse.Auth.Extensions;
using TrueFalse.Hubs.Main.Dtos;

namespace TrueFalse.Hubs.Main
{
    public class MainHub : Hub<IMainHubClient>
    {
        private readonly GameTableService _gameTableService;
        private readonly PlayerService _playerService;
        private readonly ILogger<MainHub> _logger;

        public MainHub(GameTableService gameTableService, PlayerService playerService, ILogger<MainHub> logger)
        {
            _gameTableService = gameTableService;
            _playerService = playerService;
            _logger = logger;
        }

        private async Task AddToGroup(string groupName, string connectionId)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        private async Task RemoveFromGroup(string groupName, string connectionId)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
        }

        private async Task NotifyAboutPlayerJoined(Guid gameTableId, Guid playerId)
        {
            await Clients.GroupExcept(gameTableId.ToString(), new string[1] { Context.ConnectionId }).OnPlayerJoined(new OnPlayerJoinedParams()
            {
                GameTableId = gameTableId,
                PlayerId = playerId
            });
        }

        private async Task NotifyAboutPlayerLeaved(Guid gameTableId, Guid playerId)
        {
            await Clients.GroupExcept(gameTableId.ToString(), new string[1] { Context.ConnectionId }).OnPlayerLeaved(new OnPlayerJoinedParams()
            {
                GameTableId = gameTableId,
                PlayerId = playerId
            });
        }

        private async Task NotifyGameStarted(Guid gameTableId, Guid moverId)
        {
            await Clients.GroupExcept(gameTableId.ToString(), new string[1] { Context.ConnectionId }).OnGameStarted(new OnGameStartedParams()
            {
                MoverId = moverId
            });
        }

        private async Task NotifyGameTableCreated(GameTableDto gameTable)
        {
            await Clients.AllExcept(Context.ConnectionId).OnCreatedNewGameTable(new OnCreatedNewGameTableParams()
            {
                GameTable = gameTable
            });
        }

        private async Task NotifyFirstMoveMade(Guid gameTableId, IReadOnlyCollection<int> cardIds, Guid nextMoverId)
        {
            await Clients.GroupExcept(gameTableId.ToString(), new string[1] { Context.ConnectionId }).OnFirstMoveMade(new OnFirstMoveMadeParams()
            {
                NextMoverId = nextMoverId,
                CardIds = cardIds.ToList()
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка создания игрового стола. PlayerId = {@params.OwnerId}");

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
                _gameTableService.Join(@params.GameTableId, Context.User.GetUserId());

                await AddToGroup(@params.GameTableId.ToString(), Context.ConnectionId);
                await NotifyAboutPlayerJoined(@params.GameTableId, Context.User.GetUserId());
                await Clients.Caller.ReceiveJoinResult(new ReceiveJoinResultParams()
                {
                    Succeeded = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка присоединения к игровому столу. Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveJoinResult(new ReceiveJoinResultParams()
                {
                    Succeeded = false
                });
            }
        }

        public async Task LeaveFromGameTable()
        {
            try
            {
                var gameTableId = _gameTableService.Leave(Context.User.GetUserId());

                await RemoveFromGroup(gameTableId.ToString(), Context.ConnectionId);
                await NotifyAboutPlayerLeaved(gameTableId, Context.User.GetUserId());
                await Clients.Caller.ReceiveLeaveResult(new ReceiveLeaveResultParams()
                {
                    Succeeded = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка выхода из-за игрового стола. Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveLeaveResult(new ReceiveLeaveResultParams()
                {
                    Succeeded = false
                });
            }
        }

        public async Task StartGame()
        {
            try
            {
                var result = _gameTableService.StartGame(Context.User.GetUserId());

                await NotifyGameStarted(result.GameTableId, result.MoverId);
                await Clients.Caller.ReceiveGameStartResult(new ReceiveGameStartResultParams()
                {
                    Succeeded = true,
                    MoverId = result.MoverId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка старта игры. Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveGameStartResult(new ReceiveGameStartResultParams()
                {
                    Succeeded = false
                });
            }
        }

        public async Task MakeFirstMove(MakeFirstMoveParams @params)
        {
            try
            {
                var result = _gameTableService.MakeFirstMove(Context.User.GetUserId(), @params.CardIds, @params.Rank);

                await NotifyFirstMoveMade(result.GameTableId, @params.CardIds, result.NextMoverId);
                await Clients.Caller.ReceiveMakeFirstMoveResult(new ReceiveMakeFirstMoveResultParams()
                {
                    Succeeded = true,
                    NextMoverId = result.NextMoverId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка хода типа \"Первый ход\". Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveMakeFirstMoveResult(new ReceiveMakeFirstMoveResultParams()
                {
                    Succeeded = false
                });
            }
        }

        public async Task MakeBeliveMove(MakeBeliveMoveParams @params)
        {

        }

        public async Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params)
        {

        }
    }
}
