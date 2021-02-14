using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Application.Dtos;
using TrueFalse.Application.Dtos.Results;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Application.Services
{
    public class GameTableService
    {
        private readonly IGameTableRepository _gameTableRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly CreateGameTableChecker _createGameTableChecker;

        public GameTableService(IGameTableRepository gameTableRepository, IPlayerRepository playerRepository, CreateGameTableChecker createGameTableChecker)
        {
            _gameTableRepository = gameTableRepository;
            _playerRepository = playerRepository;
            _createGameTableChecker = createGameTableChecker;
        }

        public IReadOnlyCollection<GameTableDto> GetGameTablesTest()
        {
            return new List<GameTableDto>()
            {
                new GameTableDto()
                { 
                    Id = Guid.NewGuid(),
                    Name = "Test1",
                    Owner = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 1" },
                    Type = GameTableType.Cards36And3Players,
                    Players = new List<GameTablePlayerDto>() 
                    {
                        new GameTablePlayerDto() { GameTablePlaceNumber = 1, Player = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 1" } },
                        new GameTablePlayerDto() { GameTablePlaceNumber = 2, Player = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 2" } }
                    }
                },
                new GameTableDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    Owner = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 3" },
                    Type = GameTableType.Cards36And4Players,
                    Players = new List<GameTablePlayerDto>()
                    {
                        new GameTablePlayerDto() { GameTablePlaceNumber = 1, Player = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 3" } },
                        new GameTablePlayerDto() { GameTablePlaceNumber = 2, Player = new PlayerDto() { Id = Guid.NewGuid(), Name = "Player 4" } }
                    }
                }
            };
        }

        public IReadOnlyCollection<GameTableDto> GetGameTables(int pageNum, int perPage)
        {
            return _gameTableRepository.GetGameTables(pageNum, perPage).Select(gt => new GameTableDto()
            {
                Id = gt.Id,
                Name = gt.Name,
                Owner = new PlayerDto() { Id = gt.Owner.Id, Name = gt.Owner.Name },
                Type = GameTableTypeCalculator.CalculateGameTableType(gt),
                Players = gt.Players.Select(p => new GameTablePlayerDto() 
                { 
                    GameTablePlaceNumber = p.GameTablePlaceNumber,
                    Player = new PlayerDto() { Id = p.Player.Id, Name = p.Player.Name} 
                }).ToList()
            }).ToList();
        }

        public Guid? GetGameTableIdByPlayerId(Guid playerId)
        {
            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByPlayer(player);

            return gameTable?.Id;
        }

        public GameTableDto CreateGameTable(Guid ownerId, string gameTableName, int playersCount, int cardsCount)
        {
            if (string.IsNullOrWhiteSpace(gameTableName))
            {
                throw new ArgumentNullException(nameof(gameTableName));
            }

            if (ownerId == new Guid())
            {
                throw new ArgumentException(nameof(ownerId));
            }

            var player = _playerRepository.GetById(ownerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {ownerId}");
            }

            if (!_createGameTableChecker.CanCreateGameTable(player))
            {
                throw new Exception($"Игрок с Id = {player.Id} не может создать комнату");
            }

            var gameTable = GameTableFactory.Create(player, gameTableName, playersCount, cardsCount);

            _gameTableRepository.Add(gameTable);

            return new GameTableDto()
            {
                Id = gameTable.Id,
                Name = gameTable.Name,
                Owner = new PlayerDto()
                { 
                    Id = gameTable.Owner.Id,
                    Name = gameTable.Owner.Name
                },
                Type = GameTableTypeCalculator.CalculateGameTableType(gameTable),
                Players = gameTable.Players.Select(p => new GameTablePlayerDto()
                {
                    GameTablePlaceNumber = p.GameTablePlaceNumber,
                    Player = new PlayerDto() { Id = p.Player.Id, Name = p.Player.Name }
                }).ToList()
            };
        }

        public void Join(Guid gameTableId, Guid playerId)
        {
            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            if (_gameTableRepository.IsAlreadyPlaying(player))
            {
                throw new Exception($"Игрок c Id = {playerId} уже играет за другим игровым столом");
            }

            var gameTable = _gameTableRepository.GetById(gameTableId);
            if (gameTable == null)
            {
                throw new Exception($"Игрового стола с Id = {gameTableId} не существует");
            }

            gameTable.Join(player);
        }

        public Guid Leave(Guid playerId)
        {
            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByPlayer(player);
            if (gameTable == null)
            {
                throw new Exception($"Игрок с Id = {playerId} не находится за игровым столом");
            }

            gameTable.Leave(player);

            return gameTable.Id;
        }

        public StartGameResult StartGame(Guid playerId)
        {
            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByOwner(player);
            if (gameTable == null)
            {
                throw new Exception($"Игрок с Id = {playerId} не является создателем игрового стола");
            }

            gameTable.StartNewGame(player);

            var result = new StartGameResult()
            {
                GameTableId = gameTable.Id,
                MoverId = gameTable.CurrentMover.Id,
                PlayerCards = new List<PlayerCardsInfo>()
            };

            foreach (var item in gameTable.Players)
            {
                var playerCards = gameTable.GetPlayerCards(item.Player.Id);

                var playerCardsItem = new PlayerCardsInfo()
                {
                    PlayerId = item.Player.Id,
                    CardsCount = playerCards.Count,
                    Cards = playerCards.Select(c => new PlayingCardDto() { Id = c.Id, Rank = (int)c.Rank, Suit = (int)c.Suit }).ToList()
                };

                result.PlayerCards.Add(playerCardsItem);
            }

            return result;
        }

        public MakeFirstMoveResult MakeFirstMove(Guid playerId, IReadOnlyCollection<int> cardIds, int rank)
        {
            if (cardIds == null)
            {
                throw new ArgumentNullException(nameof(cardIds));
            }

            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByPlayer(player);
            if (gameTable == null)
            {
                throw new Exception($"Игрок с Id = {playerId} не находится за игровым столом");
            }

            var cards = gameTable.GetPlayerCards(playerId, cardIds);

            var move = new FirstMove(cards.Select(c => new PlayingCard(c.Id, c.Suit, c.Rank)).ToList(), (PlayingCardRank)rank, playerId);

            gameTable.MakeFirstMove(move);

            return new MakeFirstMoveResult()
            {
                NextMoverId = gameTable.CurrentMover.Id
            };
        }

        public MakeBeleiveMoveResult MakeBelieveMove(Guid playerId, IReadOnlyCollection<int> cardIds)
        {
            if (cardIds == null)
            {
                throw new ArgumentNullException(nameof(cardIds));
            }

            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByPlayer(player);
            if (gameTable == null)
            {
                throw new Exception($"Игрок с Id = {playerId} не находится за игровым столом");
            }

            var cards = gameTable.GetPlayerCards(playerId, cardIds);

            var move = new BelieveMove(cards.Select(c => new PlayingCard(c.Id, c.Suit, c.Rank)).ToList(), playerId);

            gameTable.MakeBeleiveMove(move);

            return new MakeBeleiveMoveResult()
            {
                GameTableId = gameTable.Id,
                NextMoverId = gameTable.CurrentMover.Id
            };
        }

        public MakeDontBeliveMoveResult MakeDontBeliveMove(Guid playerId, int selectedCardId)
        {
            if (selectedCardId <= 0)
            {
                throw new ArgumentException(nameof(selectedCardId));
            }

            var player = _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new NullReferenceException($"Отсутствует пользователь с id = {playerId}");
            }

            var gameTable = _gameTableRepository.GetByPlayer(player);
            if (gameTable == null)
            {
                throw new Exception($"Игрок с Id = {playerId} не находится за игровым столом");
            }

            var move = new DontBelieveMove(selectedCardId, playerId);

            var checkedCard = gameTable.GetCardFromCurrentRoundById(selectedCardId);

            gameTable.MakeDontBeleiveMove(move, out var takedLoserCards, out var loserId);

            return new MakeDontBeliveMoveResult()
            {
                GameTableId = gameTable.Id,
                NextMoverId = gameTable.CurrentMover?.Id,
                CheckedCard = new PlayingCardDto()
                {
                    Id = checkedCard.Id,
                    Rank = (int)checkedCard.Rank,
                    Suit = (int)checkedCard.Suit
                },
                LoserId = loserId,
                TakedLoserCards = takedLoserCards.Select(c => new PlayingCardDto()
                {
                    Id = c.Id,
                    Rank = (int)c.Rank,
                    Suit = (int)c.Suit
                }).ToList()
            };
        }
    }
}
