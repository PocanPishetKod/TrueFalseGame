using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameTable5To52 : GameTable
    {
        public GameTable5To52(Player owner, string name, Guid id) : base(owner, name, id)
        {
            
        }

        protected override CardsPack CreateNewCardsPack()
        {
            return new CardsPack52();
        }

        protected override PlayPlaces CreatePlayPlaces()
        {
            return new Play5Places();
        }
    }
}
