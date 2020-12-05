using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Application.Dtos.Results;
using TrueFalse.Application.Services;
using TrueFalse.Auth.Extensions;
using TrueFalse.SignalR.Core.Dtos;

namespace TrueFalse.SignalR.Api.Hubs.Main
{
    public class MainHub : Hub<IMainHubClient>
    {
        private readonly GameTableService _gameTableService;
        private readonly ILogger<MainHub> _logger;

        private static readonly ConcurrentDictionary<Guid, string> _userConnectionIdStore;

        static MainHub()
        {
            _userConnectionIdStore = new ConcurrentDictionary<Guid, string>();
        }

        public MainHub(GameTableService gameTableService, ILogger<MainHub> logger)
        {
            _gameTableService = gameTableService;
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

        private async Task DisconnectInternal()
        {
            var gameTableId = _gameTableService.GetGameTableIdByPlayerId(Context.User.GetUserId());
            if (gameTableId.HasValue)
            {
                _gameTableService.Leave(Context.User.GetUserId());

                await RemoveFromGroup(gameTableId.Value.ToString(), Context.ConnectionId);
                await NotifyAboutPlayerLeaved(gameTableId.Value, Context.User.GetUserId());
            }

            if (!_userConnectionIdStore.TryRemove(Context.User.GetUserId(), out var removedValue))
            {
                _logger.LogError("Идентификатор подключения уже был удален из словаря");
            }
        }

        #region Notificators

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

        private async Task NotifyBelieveMoveMade(Guid gameTableId, IReadOnlyCollection<int> cardIds, Guid nextMoverId)
        {
            await Clients.GroupExcept(gameTableId.ToString(), new string[1] { Context.ConnectionId }).OnBeliveMoveMade(new OnBeliveMoveMadeParams()
            {
                CardIds = cardIds.ToList(),
                NextMoverId = nextMoverId
            });
        }

        private async Task NotifyDontBeliveMoveMade(MakeDontBeliveMoveResult moveResult)
        {
            if (_userConnectionIdStore.TryGetValue(moveResult.LoserId, out var loserConnectionId))
            {
                await Clients.GroupExcept(moveResult.GameTableId.ToString(), new string[2] { loserConnectionId, Context.ConnectionId }).OnDontBeliveMoveMade(new OnDontBeliveMoveMadeParams()
                {
                    LoserId = moveResult.LoserId,
                    CheckedCard = moveResult.CheckedCard,
                    NextMoverId = moveResult.NextMoverId,
                    HiddenTakedLoserCards = moveResult.TakedLoserCards.Select(c => c.Id).ToList()
                });

                await Clients.Client(loserConnectionId).OnDontBeliveMoveMade(new OnDontBeliveMoveMadeParams()
                {
                    LoserId = moveResult.LoserId,
                    CheckedCard = moveResult.CheckedCard,
                    NextMoverId = moveResult.NextMoverId,
                    TakedLoserCards = moveResult.TakedLoserCards.ToList()
                });
            }
            else
            {
                _logger.LogError("Игрок не найден в группе");
            }
        }

        #endregion

        #region overriden

        public async override Task OnConnectedAsync()
        {
            try
            {
                if (_userConnectionIdStore.TryGetValue(Context.User.GetUserId(), out var oldConnectionId))
                {
                    var gameTableId = _gameTableService.GetGameTableIdByPlayerId(Context.User.GetUserId());
                    if (gameTableId.HasValue)
                    {
                        if (!_userConnectionIdStore.TryUpdate(Context.User.GetUserId(), Context.ConnectionId, oldConnectionId))
                        {
                            throw new Exception("Что-то пошло не так. Старое значение было изменено в другом потоке");
                        }

                        await AddToGroup(gameTableId.ToString(), Context.ConnectionId);
                    }
                }
                else
                {
                    _userConnectionIdStore.TryAdd(Context.User.GetUserId(), Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при подключении игрока");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_userConnectionIdStore.TryGetValue(Context.User.GetUserId(), out var connectionId))
            {
                await DisconnectInternal();
            }

            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Api

        public async Task Disconnect()
        {
            if (_userConnectionIdStore.TryGetValue(Context.User.GetUserId(), out var connectionId))
            {
                await DisconnectInternal();
                Context.Abort();
            }
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
            try
            {
                var result = _gameTableService.MakeBelieveMove(Context.User.GetUserId(), @params.CardIds);

                await NotifyBelieveMoveMade(result.GameTableId, @params.CardIds, result.NextMoverId);
                await Clients.Caller.ReceiveMakeBeliveMoveResult(new ReceiveMakeBeliveMoveResultParams()
                {
                    Succeeded = true,
                    NextMoverId = result.NextMoverId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка хода типа \"Верю\". Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveMakeBeliveMoveResult(new ReceiveMakeBeliveMoveResultParams()
                {
                    Succeeded = false
                });
            }
        }

        public async Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params)
        {
            try
            {
                var result = _gameTableService.MakeDontBeliveMove(Context.User.GetUserId(), @params.SelectedCardId);

                await NotifyDontBeliveMoveMade(result);
                await Clients.Caller.ReceiveMakeDontBeliveMoveResult(new ReceiveMakeDontBeliveMoveResultParams()
                {
                    Succeeded = true,
                    CheckedCard = result.CheckedCard,
                    LoserId = result.LoserId,
                    NextMoverId = result.NextMoverId,
                    HiddenTakedLoserCards = result.LoserId != Context.User.GetUserId() ? result.TakedLoserCards.Select(c => c.Id).ToList() : null,
                    TakedLoserCards = result.LoserId == Context.User.GetUserId() ? result.TakedLoserCards.ToList() : null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка хода типа \"Не верю\". Игрок с Id = {Context.User.GetUserId()}");

                await Clients.Caller.ReceiveMakeDontBeliveMoveResult(new ReceiveMakeDontBeliveMoveResultParams()
                {
                    Succeeded = false
                });
            }
        }

        #endregion
    }
}
