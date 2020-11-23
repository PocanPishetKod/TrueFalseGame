using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class MakeFirstMoveParams
    {
        public List<int> CardIds { get; set; }

        public int Rank { get; set; }
    }
}
