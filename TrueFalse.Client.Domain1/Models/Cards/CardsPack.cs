using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Models.Cards
{
    public class CardsPack
    {
        public int Count { get; private set; }

        public CardsPack(int cardsCount)
        {
            Count = cardsCount;
        }
    }
}
