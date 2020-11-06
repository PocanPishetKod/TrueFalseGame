using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public static class GameTableFactory
    {
        public static GameTable Create(Player player, string gameTableName, int playersCount, int cardsCount)
        {
            if (playersCount == 3 && cardsCount == 36)
            {
                return new GameTable3To36(player, gameTableName, Guid.NewGuid());
            }
            else if (playersCount == 4 && cardsCount == 36)
            {
                return new GameTable4To36(player, gameTableName, Guid.NewGuid());
            }
            else if (playersCount == 4 && cardsCount == 52)
            {
                return new GameTable4To52(player, gameTableName, Guid.NewGuid());
            }
            else if (playersCount == 5 && cardsCount == 52)
            {
                return new GameTable5To52(player, gameTableName, Guid.NewGuid());
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Нет типа комнаты с {playersCount} пользователей и с {cardsCount} картами");
            }
        }
    }
}
