using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public class GamePlayer
    {
        private List<PlayingCard> _cards;

        public int Priority { get; set; }

        public Player Player { get; private set; }

        public IReadOnlyCollection<PlayingCard> Cards => _cards;

        public GamePlayer(Player player, int priority)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            Player = player;
            Priority = priority;
            _cards = new List<PlayingCard>();
        }

        /// <summary>
        /// Дает карты игроку
        /// </summary>
        /// <param name="cards"></param>
        public void GiveCards(IReadOnlyCollection<PlayingCard> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            _cards.AddRange(cards);
        }

        /// <summary>
        /// Забирает карты у игрока
        /// </summary>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public IReadOnlyCollection<PlayingCard> TakeCards(IReadOnlyCollection<int> cardIds)
        {
            if (_cards.Count == 0)
            {
                throw new TrueFalseGameException("У игрока нет карт");
            }

            var result = new List<PlayingCard>();
            foreach (var id in cardIds)
            {
                var card = _cards.FirstOrDefault(c => c.Id == id);
                if (card == null)
                {
                    throw new TrueFalseGameException($"У игрока с Id = {Player.Id} нет карты с Id = {id}");
                }

                result.Add(card);
                _cards.Remove(card);
            }

            return result;
        }
    }
}
