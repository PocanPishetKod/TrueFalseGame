﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class JoinToGameTableParams : RequestParams
    {
        public Guid GameTableId { get; set; }
    }
}
