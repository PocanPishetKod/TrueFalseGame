using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameTable5To52 : GameTable
    {
        private Play5Places _places;
        private CardsPack52 _cardsPack;

        protected override PlayPlaces PlayPlaces => _places;

        protected override CardsPack CardsPack => _cardsPack;

        public GameTable5To52(Player owner, string name, Guid id) : base(owner, name, id)
        {
            
        }

        protected override void Initialize()
        {
            _places = new Play5Places();
            _cardsPack = new CardsPack52();
        }
    }
}
