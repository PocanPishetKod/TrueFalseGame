using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Moves
{
    public abstract class Move : BaseModel
    {
        public Player Initiator { get; protected set; }

        public abstract bool IsValid { get; }

        public Move(Player initiator)
        {
            Initiator = initiator;
        }
    }
}
