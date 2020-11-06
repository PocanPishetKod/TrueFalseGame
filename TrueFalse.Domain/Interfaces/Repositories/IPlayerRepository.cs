using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        IReadOnlyCollection<Player> GetPlayers();

        Player GetById(Guid id);

        void Add(Player player);

        void Remove(Player player);
    }
}
