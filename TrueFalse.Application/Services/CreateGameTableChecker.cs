using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Application.Services
{
    public class CreateGameTableChecker
    {
        private readonly IGameTableRepository _gameTableRepository;

        public CreateGameTableChecker(IGameTableRepository gameTableRepository)
        {
            _gameTableRepository = gameTableRepository;
        }

        public bool CanCreateGameTable(Player player)
        {
            return _gameTableRepository.GetByPlayer(player) == null && _gameTableRepository.GetByOwner(player) == null;
        }
    }
}
