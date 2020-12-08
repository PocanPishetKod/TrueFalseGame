using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Api
{
    public interface IHubClientConnection
    {
        Task Connect();

        Task Close();
    }
}
