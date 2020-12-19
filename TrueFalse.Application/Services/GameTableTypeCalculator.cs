using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Domain.Models.GameTables;

namespace TrueFalse.Application.Services
{
    public static class GameTableTypeCalculator
    {
        public static GameTableType CalculateGameTableType(GameTable gameTable)
        {
            if (gameTable is GameTable3To36)
            {
                return GameTableType.Cards36And3Players;
            }
            else if (gameTable is GameTable4To36)
            {
                return GameTableType.Cards36And4Players;
            }
            else if (gameTable is GameTable4To52)
            {
                return GameTableType.Cards52And4Players;
            }
            else
            {
                return GameTableType.Cards52And5Players;
            }
        }
    }
}
