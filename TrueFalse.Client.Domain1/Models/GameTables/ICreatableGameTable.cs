using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.Models.GameTables
{
    public interface ICreatableGameTable
    {
        GameTableType Type { get; set; }

        string Name { get; set; }
    }
}
