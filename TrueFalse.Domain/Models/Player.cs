using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    public class Player
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Player(Guid id, string name)
        {
            if (id == new Guid())
            {
                throw new ArgumentException($"Id игрока не может быть значением по умолчанию");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name;
        }
    }
}
