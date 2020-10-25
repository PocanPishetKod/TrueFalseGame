using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Application.Dtos;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;

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

        public IReadOnlyCollection<GameTableDto> GetGameTables()
        {
            return _gameTableRepository.GetGameTables().Select(gt => new GameTableDto()
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

        public Guid CreateGameTable(Guid ownerId, string gameTableName, int maxPlayersCount, int cardsCount)
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

            var gameTable = GameTableFactory.Create(player, gameTableName, maxPlayersCount, cardsCount);

            _gameTableRepository.Add(gameTable);

            return gameTable.Id;
        }
    }
}
