using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;
using TrueFalse.Hubs.Main.Dtos;

namespace TrueFalse.Hubs.Main
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
        Task ReceiveGameTables(ReceiveGameTableParams @params);

        /// <summary>
        /// Принимает новую комнату
        /// </summary>
        /// <param name="gameTable"></param>
        /// <returns></returns>
        Task ReceiveNewGameTable(ReceiveNewGameTableParams @params);

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
        Task OnPlayerLeaved(OnPlayerJoinedParams @params);

        /// <summary>
        /// Получает результат попытки выхода из-за игрового стола
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveLeaveResult(ReceiveLeaveResultParams @params);

        /// <summary>
        /// Получает уведомление о начале игры
        /// </summary>
        /// <returns></returns>
        Task OnGameStarted();

        /// <summary>
        /// Получает результат попытки начать игру
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task ReceiveGameStartResult(ReceiveGameStartResultParams @params);
    }
}
