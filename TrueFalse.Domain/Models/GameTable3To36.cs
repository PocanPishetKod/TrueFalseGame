using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    /// <summary>
    /// Игровой стол с 3 игроками и 36 картами
    /// </summary>
    public class GameTable3To36 : GameTable
    {
        public GameTable3To36(Player owner, string name, Guid id) : base(owner, name, id)
        {

        }

        protected override bool CanJoinPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
