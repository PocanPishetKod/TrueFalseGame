﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class ReceiveJoinResultParams : ResponseParams
    {
        public int PlaceNumber { get; set; }
    }
}
