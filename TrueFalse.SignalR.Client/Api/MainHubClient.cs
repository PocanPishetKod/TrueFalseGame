using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;
using TrueFalse.SignalR.Client.Promises;

namespace TrueFalse.SignalR.Client.Api
{
    public class MainHubClient : IMainHubApi, IHubClientConnection, IDisposable
    {
        private HubConnection _hubConnection;
        private string _accessToken;
        private bool _isDisposed;

        internal event Action<ReceiveGameTablesParams> ReceivedGameTables;
        internal event Action<ReceiveCreateGameTableResultParams> ReceivedCreateGameTableResult;
        internal event Action<ReceiveJoinResultParams> ReceivedJoinResult;
        internal event Action<ReceiveGameStartResultParams> ReceivedGameStartResult;

        public MainHubClient(string accessToken = null)
        {
            _accessToken = accessToken;
            _isDisposed = false;
        }

        public string AccessToken
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _accessToken = value;
            }
        }

        private void SetHandlers()
        {
            _hubConnection.On<OnBeliveMoveMadeParams>(nameof(OnBeliveMoveMade), OnBeliveMoveMade);
            _hubConnection.On<OnCreatedNewGameTableParams>(nameof(OnCreatedNewGameTable), OnCreatedNewGameTable);
            _hubConnection.On<OnDontBeliveMoveMadeParams>(nameof(OnDontBeliveMoveMade), OnDontBeliveMoveMade);
            _hubConnection.On<OnFirstMoveMadeParams>(nameof(OnFirstMoveMade), OnFirstMoveMade);
            _hubConnection.On<OnGameStartedParams>(nameof(OnGameStarted), OnGameStarted);
            _hubConnection.On<OnPlayerJoinedParams>(nameof(OnPlayerJoined), OnPlayerJoined);
            _hubConnection.On<OnPlayerLeavedParams>(nameof(OnPlayerLeaved), OnPlayerLeaved);
            _hubConnection.On<ReceiveCreateGameTableResultParams>(nameof(ReceiveCreateGameTableResult), ReceiveCreateGameTableResult);
            _hubConnection.On<ReceiveGameStartResultParams>(nameof(ReceiveGameStartResult), ReceiveGameStartResult);
            _hubConnection.On<ReceiveGameTablesParams>(nameof(ReceiveGameTables), ReceiveGameTables);
            _hubConnection.On<ReceiveJoinResultParams>(nameof(ReceiveJoinResult), ReceiveJoinResult);
            _hubConnection.On<ReceiveLeaveResultParams>(nameof(ReceiveLeaveResult), ReceiveLeaveResult);
            _hubConnection.On<ReceiveMakeBeliveMoveResultParams>(nameof(ReceiveMakeBeliveMoveResult), ReceiveMakeBeliveMoveResult);
            _hubConnection.On<ReceiveMakeDontBeliveMoveResultParams>(nameof(ReceiveMakeDontBeliveMoveResult), ReceiveMakeDontBeliveMoveResult);
            _hubConnection.On<ReceiveMakeFirstMoveResultParams>(nameof(ReceiveMakeFirstMoveResult), ReceiveMakeFirstMoveResult);
        }

        /// <summary>
        /// Принимает игровые столы от сервера
        /// </summary>
        /// <param name="gameTables"></param>
        /// <returns></returns>
        private void ReceiveGameTables(ReceiveGameTablesParams @params)
        {
            ReceivedGameTables?.Invoke(@params);
        }

        /// <summary>
        /// Получает результат создания игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveCreateGameTableResult(ReceiveCreateGameTableResultParams @params)
        {
            ReceivedCreateGameTableResult?.Invoke(@params);
        }

        /// <summary>
        /// Получает уведомление о создании нового игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnCreatedNewGameTable(OnCreatedNewGameTableParams @params)
        {

        }

        /// <summary>
        /// Получает уведомление о присоединении игрока за игровой стол
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnPlayerJoined(OnPlayerJoinedParams @params)
        {

        }

        /// <summary>
        /// Получает результат попытки присоединения к игровому столу
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveJoinResult(ReceiveJoinResultParams @params)
        {
            ReceivedJoinResult?.Invoke(@params);
        }

        /// <summary>
        /// Получает уведомление о выходе пользователя из-за игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnPlayerLeaved(OnPlayerLeavedParams @params)
        {

        }

        /// <summary>
        /// Получает результат попытки выхода из-за игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveLeaveResult(ReceiveLeaveResultParams @params)
        {

        }

        /// <summary>
        /// Получает уведомление о начале игры
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnGameStarted(OnGameStartedParams @params)
        {

        }

        /// <summary>
        /// Получает результат попытки начать игру
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveGameStartResult(ReceiveGameStartResultParams @params)
        {

        }

        /// <summary>
        /// Получает уведомление о совершении первого хода
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnFirstMoveMade(OnFirstMoveMadeParams @params)
        {

        }

        /// <summary>
        /// Получает результат совершения первого хода
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveMakeFirstMoveResult(ReceiveMakeFirstMoveResultParams @params)
        {

        }

        /// <summary>
        /// Получает уведомление о совершении хода типа "Верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnBeliveMoveMade(OnBeliveMoveMadeParams @params)
        {

        }

        /// <summary>
        /// Получает результат совершения хода типа "Верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveMakeBeliveMoveResult(ReceiveMakeBeliveMoveResultParams @params)
        {

        }

        /// <summary>
        /// Получает уведомление о совершении хода типа "Не верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void OnDontBeliveMoveMade(OnDontBeliveMoveMadeParams @params)
        {

        }

        /// <summary>
        /// Получает результат совершения хода типа "Не верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private void ReceiveMakeDontBeliveMoveResult(ReceiveMakeDontBeliveMoveResultParams @params)
        {

        }

        public async Task Connect()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                throw new Exception("Подключение уже в статусе Connected");
            }

            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("AccessToken не установлен");
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

            SetHandlers();

            await _hubConnection.StartAsync();
        }

        public async Task Close()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
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
            if (_hubConnection != null && !_isDisposed)
            {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    _hubConnection.StopAsync();
                }

                _hubConnection.DisposeAsync();
                _isDisposed = true;
            }
        }

        public async Task Disconnect()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(IMainHubApi.Disconnect));
        }

        public Promise<ReceiveGameTablesParams> GetGameTables(GetGameTablesParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            var promise = new Promise<ReceiveGameTablesParams>(@params.RequestId, () =>
            {
                _hubConnection.InvokeAsync(nameof(IMainHubApi.GetGameTables), @params);
            });

            ReceivedGameTables += promise.OnCopleted;

            return promise;
        }

        public Promise<ReceiveCreateGameTableResultParams> CreateGameTable(CreateGameTableParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            var promise = new Promise<ReceiveCreateGameTableResultParams>(@params.RequestId, () =>
            {
                _hubConnection.InvokeAsync(nameof(IMainHubApi.CreateGameTable), @params);
            });

            ReceivedCreateGameTableResult += promise.OnCopleted;

            return promise;
        }

        public Promise<ReceiveJoinResultParams> JoinToGameTable(JoinToGameTableParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            var promise = new Promise<ReceiveJoinResultParams>(@params.RequestId, () =>
            {
                _hubConnection.InvokeAsync(nameof(IMainHubApi.JoinToGameTable), @params);
            });

            ReceivedJoinResult += promise.OnCopleted;

            return promise;
        }

        public async Task LeaveFromGameTable(LeaveFromGameTableParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(IMainHubApi.LeaveFromGameTable), @params);
        }

        public Promise<ReceiveGameStartResultParams> StartGame(StartGameParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            var promise = new Promise<ReceiveGameStartResultParams>(@params.RequestId, () =>
            {
                _hubConnection.InvokeAsync(nameof(IMainHubApi.StartGame), @params);
            });

            ReceivedGameStartResult += promise.OnCopleted;

            return promise;
        }

        public async Task MakeFirstMove(MakeFirstMoveParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(IMainHubApi.MakeFirstMove), @params);
        }

        public async Task MakeBeliveMove(MakeBeliveMoveParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(IMainHubApi.MakeBeliveMove), @params);
        }

        public async Task MakeDontBelieveMove(MakeDontBeliveMoveParams @params)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Ресурсы объекта были освобождены");
            }

            if (_hubConnection == null)
            {
                throw new Exception("Подключение еще не было создано");
            }

            if (_hubConnection.State != HubConnectionState.Connected)
            {
                throw new Exception($"Подключение еще не установлено. Статус - {_hubConnection.State}");
            }

            await _hubConnection.InvokeAsync(nameof(IMainHubApi.MakeDontBelieveMove), @params);
        }
    }
}
