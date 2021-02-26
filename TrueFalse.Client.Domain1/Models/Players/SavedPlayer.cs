using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Models.Players
{
    [Serializable]
    public class SavedPlayer : Player
    {
        public string Token { get; set; }
    }
}
