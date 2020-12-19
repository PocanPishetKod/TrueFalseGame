using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
