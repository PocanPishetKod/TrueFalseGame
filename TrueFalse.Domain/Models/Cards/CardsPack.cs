using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Utils;

namespace TrueFalse.Domain.Models.Cards
{
    /// <summary>
    /// Колода карт
    /// </summary>
    public abstract class CardsPack
    {
        protected List<PlayingCard> _cards;

        protected CardsPack()
        {
            CreateCards();
        }

        /// <summary>
        /// Создает колоду карт в зависимости от реализации
        /// </summary>
        protected abstract void CreateCards();

        /// <summary>
        /// Определяет входил ли в данный тип колоды указанный rank
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public abstract bool IsRankContains(PlayingCardRank rank);

        /// <summary>
        /// Заполняет колоду начиная с указанного достоинства карты
        /// </summary>
        /// <param name="startRank"></param>
        protected void CreateCards(PlayingCardRank startRank)
        {
            for (int i = (int)startRank, id = 1; i <= EnumUtils.MaxValue<PlayingCardRank>(); i++)
            {
                for (int j = EnumUtils.MinValue<PlayingCardSuit>(); j <= EnumUtils.MaxValue<PlayingCardSuit>(); id++, j++)
                {
                    _cards.Add(new PlayingCard(id, (PlayingCardSuit)j, (PlayingCardRank)i));
                }
            }
        }

        /// <summary>
        /// Тасовать
        /// </summary>
        public void Shuffle()
        {
            PlayingCard temp;
            var random = new Random();
            for (int i = 0; i < _cards.Count; i++)
            {
                int index;
                do
                {
                    index = random.Next(0, _cards.Count);
                }
                while (index == i);

                temp = _cards[index];
                _cards[index] = _cards[i];
                _cards[i] = temp;
            }
        }

        /// <summary>
        /// Количество оставшихся карт в колоде
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _cards.Count;
        }

        /// <summary>
        /// Возвращает одну карту с концав колоды
        /// </summary>
        /// <returns></returns>
        public PlayingCard TakeOne()
        {
            if (_cards.Count == 0)
            {
                throw new TrueFalseGameException("Нельзя брать карты из пустой колоды");
            }

            var result = _cards[_cards.Count - 1];
            _cards.Remove(result);
            return result;
        }

        /// <summary>
        /// Возвращает указанное количество карт с конца колоды
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<PlayingCard> TakeMany(int count)
        {
            if (_cards.Count < count)
            {
                throw new TrueFalseGameException("Нельзя брать больше карт из колоды, чем в ней есть");
            }

            var startIndex = (_cards.Count - 1) - (count - 1);
            var result = new List<PlayingCard>(_cards.GetRange(startIndex, count));
            _cards.RemoveRange(startIndex, count);
            return result;
        }
    }
}
