using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models;

namespace TrueFalse.Domain.Interfaces.Repositories
{
    public interface IGameTableRepository
    {
        IReadOnlyCollection<GameTable> GetGameTables();

        GameTable GetById(Guid id);

        void Add(GameTable gameTable);

        void Remove(GameTable gameTable);

        void Update(GameTable gameTable);

        GameTable GetByOwner(Player player);

        GameTable GetByPlayer(Player player);
    }
}
