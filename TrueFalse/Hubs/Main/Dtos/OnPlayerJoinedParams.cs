using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class OnPlayerJoinedParams
    {
        public Guid GameTableId { get; set; }

        public Guid PlayerId { get; set; }
    }
}
