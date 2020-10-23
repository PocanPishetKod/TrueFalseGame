using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;

namespace TrueFalse.Application.Services
{
    public class GameTableService
    {
        private readonly IGameTableRepository _gameTableRepository;

        public GameTableService(IGameTableRepository gameTableRepository)
        {
            _gameTableRepository = gameTableRepository;
        }
    }
}
