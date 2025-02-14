﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class OnFirstMoveMadeParams
    {
        public List<int> CardIds { get; set; }

        public int Rank { get; set; }

        public Guid NextMoverId { get; set; }

        public Guid MoverId { get; set; }

        public List<MoveType> NextPossibleMoves { get; set; }
    }
}
