using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    /// <summary>
    /// Игровой стол с 3 игроками и 36 картами
    /// </summary>
    public class GameTable3To36 : GameTable
    {
        public GameTable3To36(Player owner, string name, Guid id) : base(owner, name, id)
        {
            
        }

        protected override CardsPack CreateNewCardsPack()
        {
            return new CardsPack36();
        }

        protected override PlayPlaces CreatePlayPlaces()
        {
            return new Play3Places();
        }
    }
}
