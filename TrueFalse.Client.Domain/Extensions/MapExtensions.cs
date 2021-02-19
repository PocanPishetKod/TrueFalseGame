using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.Extensions
{
    public static class MapExtensions
    {
        public static GameTable ToModel(this GameTableDto dto)
        {
            return new GameTable()
            {
                Id = dto.Id,
                Name = dto.Name,
                Players = dto.Players.Select(dp => dp.ToModel()).ToList(),
                Owner = dto.Owner.ToModel(),
                Type = dto.Type
            };
        }

        public static Player ToModel(this PlayerDto dto)
        {
            return new Player()
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public static GameTablePlayer ToModel(this GameTablePlayerDto dto)
        {
            return new GameTablePlayer(dto.Player.ToModel(), dto.GameTablePlaceNumber);
        }
    }
}
