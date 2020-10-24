using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;

namespace TrueFalse.Application.Services
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Guid CreatePlayer(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentNullException(nameof(playerName));
            }

            var player = new Player(Guid.NewGuid(), playerName);
            return player.Id;
        }
    }
}
