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
        private Play3Places _places;
        private CardsPack36 _cardsPack;

        protected override PlayPlaces PlayPlaces => _places;

        protected override CardsPack CardsPack => _cardsPack;

        public GameTable3To36(Player owner, string name, Guid id) : base(owner, name, id)
        {
            
        }

        protected override void Initialize()
        {
            _places = new Play3Places();
            _cardsPack = new CardsPack36();
        }
    }
}
