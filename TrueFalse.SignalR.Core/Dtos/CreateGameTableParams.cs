using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class CreateGameTableParams : RequestParams
    {
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public int PlayersCount
        {
            get
            {
                switch (GameTableType)
                {
                    case GameTableType.Cards36And3Players:
                        return 3;
                    case GameTableType.Cards36And4Players:
                    case GameTableType.Cards52And4Players:
                        return 4;
                    case GameTableType.Cards52And5Players:
                        return 5;
                    default:
                        throw new Exception("Неверное значение GameTableType");
                }
            }
        }

        public int CardsCount
        {
            get
            {
                return GameTableType == GameTableType.Cards36And3Players || GameTableType == GameTableType.Cards36And4Players ? 36 : 52
            }
        }

        public GameTableType GameTableType { get; set; }
    }
}
