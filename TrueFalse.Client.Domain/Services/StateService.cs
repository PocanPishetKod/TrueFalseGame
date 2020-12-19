﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Services
{
    public class StateService : IStateService
    {
        private GameTable _currentGameTable;
        private SavedPlayer _savedPlayer;

        public void SetPlayer(SavedPlayer savedPlayer)
        {
            if (_savedPlayer != null)
            {
                throw new Exception("Игрок уже назначен");
            }

            _savedPlayer = savedPlayer;
        }

        public void SetGameTable(GameTable gameTable)
        {
            if (_currentGameTable != null)
            {
                throw new Exception("Игровой стол уже установлен");
            }

            _currentGameTable = gameTable;
        }

        public SavedPlayer GetSavedPlayer()
        {
            return _savedPlayer;
        }
    }
}
