﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class CreateGameTableParams
    {
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public int PlayersCount { get; set; }

        public int CardsCount { get; set; }
    }
}
