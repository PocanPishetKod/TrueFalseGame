using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Games;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.Models.GameTables
{
    public class GameTable : ICreatableGameTable
    {
        public Guid Id { get; set; }

        public Game CurrentGame { get; set; }

        public Player Owner { get; set; }

        public List<GameTablePlayer> Players { get; set; }

        public GameTableType Type { get; set; }

        public string Name { get; set; }

        public bool IsStarted => CurrentGame != null;

        public bool CanStart
        {
            get
            {
                switch (Type)
                {
                    case GameTableType.Cards36And3Players:
                        return Players.Count == 3;
                    case GameTableType.Cards36And4Players:
                    case GameTableType.Cards52And4Players:
                        return Players.Count == 4;
                    case GameTableType.Cards52And5Players:
                        return Players.Count == 4;
                    default:
                        throw new Exception($"Нет обработчика для значения {Type}");
                }
            }
        }
    }
}
