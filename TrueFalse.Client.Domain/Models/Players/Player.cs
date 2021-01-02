using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Models.Players
{
    [Serializable]
    public class Player
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
