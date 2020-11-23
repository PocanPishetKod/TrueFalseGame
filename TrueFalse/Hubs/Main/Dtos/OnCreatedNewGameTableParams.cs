using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class OnCreatedNewGameTableParams
    {
        public GameTableDto GameTable { get; set; }
    }
}
