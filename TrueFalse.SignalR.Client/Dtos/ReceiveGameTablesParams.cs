using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{ 
    public class ReceiveGameTablesParams : ResponseParams
    {
        public List<GameTableDto> GameTables { get; set; }
    }
}
