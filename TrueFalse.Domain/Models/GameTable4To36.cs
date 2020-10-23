using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    public class GameTable4To36 : GameTable
    {
        public GameTable4To36(Player owner, string name, Guid id) : base(owner, name, id)
        {

        }

        protected override bool CanJoinPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
