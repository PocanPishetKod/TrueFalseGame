using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class OnBeliveMoveMadeParams
    {
        public List<int> CardIds { get; set; }

        public Guid NextMoverId { get; set; }
    }
}
