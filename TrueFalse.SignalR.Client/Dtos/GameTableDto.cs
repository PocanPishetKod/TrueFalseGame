using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class GameTableDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PlayerDto Owner { get; set; }

        public List<GameTablePlayerDto> Players { get; set; }
    }
}
