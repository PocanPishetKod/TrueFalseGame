using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using Xunit;

namespace TrueFalse.UnitTests.DomainTests
{
    public class CardsPackTests
    {
        [Fact]
        public void CreateCardPackTest()
        {
            CardsPack cardsPack = new CardsPack36();
            Assert.Equal(36, cardsPack.Count());

            cardsPack = new CardsPack52();
            Assert.Equal(52, cardsPack.Count());

            var cards = cardsPack.TakeMany(cardsPack.Count());
            Assert.NotNull(cards);
            Assert.NotEmpty(cards);
            // Есть ли карты с одинаковыми идентификаторами
            Assert.Empty(cards.GroupBy(c => c.Id).Where(g => g.Count() > 1));
        }

        private void TakeCardsTestInternal(CardsPack cardsPack)
        {
            var cardsCount = cardsPack.Count();
            var cards = cardsPack.TakeMany(10);
            Assert.Equal(10, cards.Count);
            Assert.Equal(cardsCount - 10, cardsPack.Count());

            var card = cardsPack.TakeOne();
            Assert.NotNull(card);
            Assert.Equal(cardsCount - 11, cardsPack.Count());

            // Попытаемся взять больше чем есть
            Assert.Throws<TrueFalseGameException>(() => { cardsPack.TakeMany(100); });
        }

        [Fact]
        public void TakeCardsTest()
        {
            TakeCardsTestInternal(new CardsPack36());
            TakeCardsTestInternal(new CardsPack52());
        }

        private void ShuffleCardsTestInternal(CardsPack cardsPack)
        {
            var cardsCount = cardsPack.Count();
            cardsPack.Shuffle();
            Assert.Equal(cardsCount, cardsPack.Count());
        }

        [Fact]
        public void ShuffleCardsTest()
        {
            ShuffleCardsTestInternal(new CardsPack36());
            ShuffleCardsTestInternal(new CardsPack52());
        }
    }
}
