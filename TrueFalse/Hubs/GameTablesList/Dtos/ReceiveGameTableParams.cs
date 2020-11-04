using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.Hubs.GameTablesList.Dtos
{
    public class ReceiveGameTableParams
    {
        public List<GameTableDto> GameTables { get; set; }
    }
}
