using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    public static class GameTableFactory
    {
        public static GameTable Create(Player player, string gameTableName, int maxPlayersCount, int cardsCount)
        {
            if (maxPlayersCount == 3 && cardsCount == 36)
            {
                return new GameTable3To36(player, gameTableName, Guid.NewGuid());
            }
            else if (maxPlayersCount == 4 && cardsCount == 36)
            {
                return new GameTable4To36(player, gameTableName, Guid.NewGuid());
            }
            else if (maxPlayersCount == 4 && cardsCount == 52)
            {
                return new GameTable4To52(player, gameTableName, Guid.NewGuid());
            }
            else if (maxPlayersCount == 5 && cardsCount == 52)
            {
                return new GameTable5To52(player, gameTableName, Guid.NewGuid());
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Нет типа комнаты с {maxPlayersCount} пользователей и с {cardsCount} картами");
            }
        }
    }
}
