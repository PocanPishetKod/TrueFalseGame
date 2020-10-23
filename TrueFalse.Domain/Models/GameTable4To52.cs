using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    public class GameTable4To52 : GameTable
    {
        public GameTable4To52(Player owner, string name, Guid id) : base(owner, name, id)
        {

        }

        protected override bool CanJoinPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
