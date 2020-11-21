using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Application.Dtos;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;
using TrueFalse.Domain.Models.GameTables;

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
                Players = gt.Players.Select(p => new GameTablePlayerDto() 
                { 
                    GameTablePlaceNumber = p.GameTablePlaceNumber,
                    Player = new PlayerDto() { Id = p.Player.Id, Name = p.Player.Name} 
                }).ToList()
            }).ToList();
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
                Players = gameTable.Players.Select(p => new GameTablePlayerDto()
                {
                    GameTablePlaceNumber = p.GameTablePlaceNumber,
                    Player = new PlayerDto() { Id = p.Player.Id, Name = p.Player.Name }
                }).ToList()
            };
        }
    }
}
