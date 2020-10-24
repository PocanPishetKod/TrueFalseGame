using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;

namespace TrueFalse.Application.Services
{
    public class GameTableService
    {
        private readonly IGameTableRepository _gameTableRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameTableService(IGameTableRepository gameTableRepository, IPlayerRepository playerRepository)
        {
            _gameTableRepository = gameTableRepository;
            _playerRepository = playerRepository;
        }

        public IReadOnlyCollection<GameTable> GetGameTables()
        {
            return _gameTableRepository.GetGameTables();
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

            var gameTable = GameTableFactory.Create(player, gameTableName, maxPlayersCount, cardsCount);

            _gameTableRepository.Add(gameTable);

            return gameTable.Id;
        }
    }
}
