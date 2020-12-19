using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class MakeFirstMoveParams : RequestParams
    {
        public List<int> CardIds { get; set; }

        public int Rank { get; set; }
    }
}
