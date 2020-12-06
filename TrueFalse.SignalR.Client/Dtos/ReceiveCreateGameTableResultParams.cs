using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class ReceiveCreateGameTableResultParams
    {
        public Guid? GameTableId { get; set; }

        public bool IsSucceeded { get; set; }
    }
}
