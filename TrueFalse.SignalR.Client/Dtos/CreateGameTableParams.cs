using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class CreateGameTableParams : RequestParams
    {
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public GameTableType GameTableType { get; set; }
    }
}
