using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos
{
    public class GameTablerPlayerDto
    {
        public int GameTablePlaceNumber { get; set; }

        public PlayerDto Player { get; set; }
    }
}
