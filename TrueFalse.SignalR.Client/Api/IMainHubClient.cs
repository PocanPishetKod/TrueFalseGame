using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.SignalR.Client.Api
{
    /// <summary>
    /// Интерфейс клиентских методов списка игровых столов
    /// </summary>
    public interface IMainHubClient
    {
        /// <summary>
        /// Принимает игровые столы от сервера
        /// </summary>
        /// <param name="gameTables"></param>
        /// <returns></returns>
        Task ReceiveGameTables(ReceiveGameTablesParams @params);

        /// <summary>
        /// Получает результат создания игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveCreateGameTableResult(ReceiveCreateGameTableResultParams @params);

        /// <summary>
        /// Получает уведомление о создании нового игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnCreatedNewGameTable(OnCreatedNewGameTableParams @params);

        /// <summary>
        /// Получает уведомление о присоединении игрока за игровой стол
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnPlayerJoined(OnPlayerJoinedParams @params);

        /// <summary>
        /// Получает результат попытки присоединения к игровому столу
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveJoinResult(ReceiveJoinResultParams @params);

        /// <summary>
        /// Получает уведомление о выходе пользователя из-за игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnPlayerLeaved(OnPlayerLeavedParams @params);

        /// <summary>
        /// Получает результат попытки выхода из-за игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveLeaveResult(ReceiveLeaveResultParams @params);

        /// <summary>
        /// Получает уведомление о начале игры
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnGameStarted(OnGameStartedParams @params);

        /// <summary>
        /// Получает результат попытки начать игру
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveGameStartResult(ReceiveGameStartResultParams @params);

        /// <summary>
        /// Получает уведомление о совершении первого хода
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnFirstMoveMade(OnFirstMoveMadeParams @params);

        /// <summary>
        /// Получает результат совершения первого хода
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveMakeFirstMoveResult(ReceiveMakeFirstMoveResultParams @params);

        /// <summary>
        /// Получает уведомление о совершении хода типа "Верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnBeliveMoveMade(OnBeliveMoveMadeParams @params);

        /// <summary>
        /// Получает результат совершения хода типа "Верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveMakeBeliveMoveResult(ReceiveMakeBeliveMoveResultParams @params);

        /// <summary>
        /// Получает уведомление о совершении хода типа "Не верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task OnDontBeliveMoveMade(OnDontBeliveMoveMadeParams @params);

        /// <summary>
        /// Получает результат совершения хода типа "Не верю"
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveMakeDontBeliveMoveResult(ReceiveMakeDontBeliveMoveResultParams @params);
    }
}
