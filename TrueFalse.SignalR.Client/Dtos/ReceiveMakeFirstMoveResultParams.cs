﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class ReceiveMakeFirstMoveResultParams : ResponseParams
    {
        public Guid? NextMoverId { get; set; }

        public Guid MoverId { get; set; }
    }
}
