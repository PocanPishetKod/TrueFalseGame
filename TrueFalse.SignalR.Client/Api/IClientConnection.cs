using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Api
{
    public interface IClientConnection
    {
        Task Connect();

        Task Close();
    }
}
