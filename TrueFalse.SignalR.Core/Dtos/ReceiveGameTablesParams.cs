using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.SignalR.Core.Dtos
{ 
    public class ReceiveGameTablesParams : ResponseParams
    {
        public List<GameTableDto> GameTables { get; set; }
    }
}
