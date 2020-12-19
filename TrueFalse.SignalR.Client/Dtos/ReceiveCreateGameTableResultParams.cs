using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class ReceiveCreateGameTableResultParams : ResponseParams
    {
        public Guid? GameTableId { get; set; }
    }
}
