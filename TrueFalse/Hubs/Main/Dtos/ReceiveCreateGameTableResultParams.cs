using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class ReceiveCreateGameTableResultParams
    {
        public Guid? GameTableId { get; set; }

        public bool IsSucceeded { get; set; }
    }
}
