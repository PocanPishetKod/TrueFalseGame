﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class OnPlayerLeavedParams
    {
        public Guid GameTableId { get; set; }

        public Guid PlayerId { get; set; }
    }
}
