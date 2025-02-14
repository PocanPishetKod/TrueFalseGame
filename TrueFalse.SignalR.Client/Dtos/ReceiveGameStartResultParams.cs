﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class ReceiveGameStartResultParams : ResponseParams
    {
        public Guid? MoverId { get; set; }

        public List<PlayerCardsInfoDto> PlayerCardsInfo { get; set; }
    }
}
