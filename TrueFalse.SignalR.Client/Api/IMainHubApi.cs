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

        Promise<ReceiveLeaveResultParams> LeaveFromGameTable(LeaveFromGameTableParams @params);

        Promise<ReceiveGameStartResultParams> StartGame(StartGameParams @params);

        Promise<ReceiveMakeFirstMoveResultParams> MakeFirstMove(MakeFirstMoveParams @params);

        Promise<ReceiveMakeBeliveMoveResultParams> MakeBeliveMove(MakeBeliveMoveParams @params);

        Promise<ReceiveMakeDontBeliveMoveResultParams> MakeDontBelieveMove(MakeDontBeliveMoveParams @params);
    }
}
