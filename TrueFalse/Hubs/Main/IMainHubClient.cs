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
    }
}
