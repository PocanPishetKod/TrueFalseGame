using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class OnPlayerJoinedParams
    {
        public Guid GameTableId { get; set; }

        public PlayerDto Player { get; set; }

        public int PlaceNumber { get; set; }
    }
}
