using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Application.Dtos;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Application.Services
{
    public class PlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public PlayerDto CreatePlayer(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentNullException(nameof(playerName));
            }

            var player = new Player(Guid.NewGuid(), playerName);
            _playerRepository.Add(player);

            return new PlayerDto()
            {
                Id = player.Id,
                Name = player.Name
            };
        }
    }
}
