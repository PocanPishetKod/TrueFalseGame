using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class GameTablePlayerDto
    {
        public int GameTablePlaceNumber { get; set; }

        public PlayerDto Player { get; set; }
    }
}
