using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;
using TrueFalse.SignalR.Client.Promises;

namespace TrueFalse.SignalR.Client.Api
{
    public interface IMainHubApi
    {
        Task Disconnect();

        Promise<ReceiveGameTablesParams> GetGameTables(GetGameTablesParams @params);

        Promise<ReceiveCreateGameTableResultParams> CreateGameTable(CreateGameTableParams @params);

        Promise<ReceiveJoinResultParams> JoinToGameTable(JoinToGameTableParams @params);

        Task LeaveFromGameTable(LeaveFromGameTableParams @params);

        Task StartGame(StartGameParams @params);

        Task MakeFirstMove(MakeFirstMoveParams @params);

        Task MakeBeliveMove(MakeBeliveMoveParams @params);

        Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params);
    }
}
