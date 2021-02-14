using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class OnBeliveMoveMadeParams
    {
        public List<int> CardIds { get; set; }

        public Guid NextMoverId { get; set; }

        public Guid MoverId { get; set; }
    }
}
