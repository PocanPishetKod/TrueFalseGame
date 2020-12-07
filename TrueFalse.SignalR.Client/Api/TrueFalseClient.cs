using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.SignalR.Client.Api
{
    public class TrueFalseClient : ITrueFalseApi, IClientConnection, IDisposable
    {
        private HubConnection _hubConnection;
        private readonly string _accessToken;

        public TrueFalseClient(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _accessToken = accessToken;
        }

        public void SetHandlers(IMainHubClient mainHubClient)
        {
            _hubConnection.On<OnBeliveMoveMadeParams>(nameof(mainHubClient.OnBeliveMoveMade), mainHubClient.OnBeliveMoveMade);
            _hubConnection.On<OnCreatedNewGameTableParams>(nameof(mainHubClient.OnCreatedNewGameTable), mainHubClient.OnCreatedNewGameTable);
            _hubConnection.On<OnDontBeliveMoveMadeParams>(nameof(mainHubClient.OnDontBeliveMoveMade), mainHubClient.OnDontBeliveMoveMade);
            _hubConnection.On<OnFirstMoveMadeParams>(nameof(mainHubClient.OnFirstMoveMade), mainHubClient.OnFirstMoveMade);
            _hubConnection.On<OnGameStartedParams>(nameof(mainHubClient.OnGameStarted), mainHubClient.OnGameStarted);
            _hubConnection.On<OnPlayerJoinedParams>(nameof(mainHubClient.OnPlayerJoined), mainHubClient.OnPlayerJoined);
            _hubConnection.On<OnPlayerLeavedParams>(nameof(mainHubClient.OnPlayerLeaved), mainHubClient.OnPlayerLeaved);
            _hubConnection.On<ReceiveCreateGameTableResultParams>(nameof(mainHubClient.ReceiveCreateGameTableResult), mainHubClient.ReceiveCreateGameTableResult);
            _hubConnection.On<ReceiveGameStartResultParams>(nameof(mainHubClient.ReceiveGameStartResult), mainHubClient.ReceiveGameStartResult);
            _hubConnection.On<ReceiveGameTablesParams>(nameof(mainHubClient.ReceiveGameTables), mainHubClient.ReceiveGameTables);
            _hubConnection.On<ReceiveJoinResultParams>(nameof(mainHubClient.ReceiveJoinResult), mainHubClient.ReceiveJoinResult);
            _hubConnection.On<ReceiveLeaveResultParams>(nameof(mainHubClient.ReceiveLeaveResult), mainHubClient.ReceiveLeaveResult);
            _hubConnection.On<ReceiveMakeBeliveMoveResultParams>(nameof(mainHubClient.ReceiveMakeBeliveMoveResult), mainHubClient.ReceiveMakeBeliveMoveResult);
            _hubConnection.On<ReceiveMakeDontBeliveMoveResultParams>(nameof(mainHubClient.ReceiveMakeDontBeliveMoveResult), mainHubClient.ReceiveMakeDontBeliveMoveResult);
            _hubConnection.On<ReceiveMakeFirstMoveResultParams>(nameof(mainHubClient.ReceiveMakeFirstMoveResult), mainHubClient.ReceiveMakeFirstMoveResult);
            _hubConnection.On<ReceiveNewGameTableParams>(nameof(mainHubClient.ReceiveNewGameTable), mainHubClient.ReceiveNewGameTable);
        }

        public async Task Connect()
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                throw new Exception("Подключение уже в статусе Connected");
            }

            _hubConnection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl("https://localhost:44307/main", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_accessToken);
                })
                .Build();

            _hubConnection.ServerTimeout = TimeSpan.FromMinutes(1);
            _hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(30);

            await _hubConnection.StartAsync();
        }

        public async Task Close()
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение не создано или уже было закрыто");
            }

            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                throw new Exception("Подключение уже было закрыто");
            }

            await Disconnect();
            Dispose();
        }

        public void Dispose()
        {
            if (_hubConnection != null)
            {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    _hubConnection.StopAsync();
                }

                _hubConnection.DisposeAsync();
            }
        }

        public async Task Disconnect()
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.Disconnect));
        }

        public async Task GetGameTables(GetGameTablesParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.GetGameTables), @params);
        }

        public async Task CreateGameTable(CreateGameTableParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.CreateGameTable), @params);
        }

        public async Task JoinToGameTable(JoinToGameTableParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.JoinToGameTable), @params);
        }

        public async Task LeaveFromGameTable()
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.LeaveFromGameTable));
        }

        public async Task StartGame()
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.StartGame));
        }

        public async Task MakeFirstMove(MakeFirstMoveParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.MakeFirstMove), @params);
        }

        public async Task MakeBeliveMove(MakeBeliveMoveParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.MakeBeliveMove), @params);
        }

        public async Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params)
        {
            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(ITrueFalseApi.MakeDontBelieveMove), @params);
        }
    }
}
