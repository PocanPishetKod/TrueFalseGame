using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Services
{
    /// <summary>
    /// Сервис, управляющий состоянием игры
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// Устанавливает текущего пользователя игры
        /// </summary>
        /// <param name="savedPlayer"></param>
        void SetPlayer(SavedPlayer savedPlayer);

        /// <summary>
        /// Возвращает установленного пользователя
        /// </summary>
        /// <returns></returns>
        SavedPlayer GetSavedPlayer();

        /// <summary>
        /// Возвращает текущий игровой стол
        /// </summary>
        /// <returns></returns>
        GameTable GetGameTable();

        /// <summary>
        /// Находится ли игрок за игровым столом
        /// </summary>
        bool AlreadyPlaying { get; }

        /// <summary>
        /// Авторизован ли игрок
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Установить текущий игровой стол
        /// </summary>
        /// <param name="gameTable"></param>
        void SetGameTable(GameTable gameTable);

        /// <summary>
        /// Данные о ходе типа "Первый ход"
        /// </summary>
        FirstMove FirstMove { get; set; }
    }
}
