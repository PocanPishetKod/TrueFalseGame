using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.SignalR.Client.Api
{
    public interface IMainHubApi
    {
        Task Disconnect();

        Task GetGameTables(GetGameTablesParams @params);

        Task CreateGameTable(CreateGameTableParams @params);

        Task JoinToGameTable(JoinToGameTableParams @params);

        Task LeaveFromGameTable();

        Task StartGame();

        Task MakeFirstMove(MakeFirstMoveParams @params);

        Task MakeBeliveMove(MakeBeliveMoveParams @params);

        Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params);
    }
}
