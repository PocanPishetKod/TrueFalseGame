using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Moves;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class GameRound
    {
        public List<Move> Moves { get; private set; }

        public GameRound()
        {
            Moves = new List<Move>();
        }
    }
}
