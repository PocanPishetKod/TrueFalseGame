using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Services
{
    /// <summary>
    /// Класс для сохранения и чтения сохраненных данных
    /// </summary>
    public class SaveService
    {
        private const string PlayerDataFileName = "PlayerData.dat";
        private readonly string _savesPath;

        public SaveService(string savesPath)
        {
            if (string.IsNullOrWhiteSpace(savesPath))
            {
                throw new ArgumentNullException(nameof(savesPath));
            }

            _savesPath = savesPath;
        }

        /// <summary>
        /// Сохраняет данные о пользователе в файл
        /// </summary>
        /// <param name="playerData"></param>
        public void SavePlayerData(SavedPlayer playerData)
        {
            if (playerData == null)
            {
                throw new ArgumentNullException(nameof(playerData));
            }

            var bf = new BinaryFormatter();

            using (var dataFile = new FileStream($"{_savesPath}/{PlayerDataFileName}", FileMode.Create))
            {
                bf.Serialize(dataFile, playerData);
            }
        }

        /// <summary>
        /// Возвращает сохраненные данные о пользователе
        /// </summary>
        /// <returns></returns>
        public SavedPlayer GetPlayerData()
        {
            if (!File.Exists($"{_savesPath}/{PlayerDataFileName}"))
            {
                return null;
            }

            var bf = new BinaryFormatter();

            using (var dataFile = new FileStream($"{_savesPath}/{PlayerDataFileName}", FileMode.Open))
            {
                try
                {
                    var result = bf.Deserialize(dataFile) as SavedPlayer;
                    return result;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
