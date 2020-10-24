﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.Hubs.GameTablesList
{
    public class ReceiveGameTableParams
    {
        public IReadOnlyCollection<GameTableDto> GameTables { get; set; }
    }
}
