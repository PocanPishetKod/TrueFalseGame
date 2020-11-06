using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameTable5To52 : GameTable
    {
        public GameTable5To52(Player owner, string name, Guid id) : base(owner, name, id)
        {

        }

        protected override bool CanJoinPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
