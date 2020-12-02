using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class OnPlayerLeavedParams
    {
        public Guid PlayerId { get; set; }
    }
}
