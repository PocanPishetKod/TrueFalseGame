using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameTable4To52 : GameTable
    {
        private Play4Places _places;
        private CardsPack52 _cardsPack;

        protected override PlayPlaces PlayPlaces => _places;

        protected override CardsPack CardsPack => _cardsPack;

        public GameTable4To52(Player owner, string name, Guid id) : base(owner, name, id)
        {
            
        }

        protected override void Initialize()
        {
            _places = new Play4Places();
            _cardsPack = new CardsPack52();
        }
    }
}
