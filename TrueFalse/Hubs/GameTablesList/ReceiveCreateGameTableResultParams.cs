using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.GameTablesList
{
    public class ReceiveCreateGameTableResultParams
    {
        public Guid? GameTableId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
