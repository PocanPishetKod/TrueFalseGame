﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public class Game
    {
        private ICollection<GameRound> Rounds { get; set; }

        private List<GamePlayer> GamePlayers { get; set; }

        private CardsPack CardsPack { get; set; }

        public Player Loser { get; private set; }

        public Player CurrentMover { get; private set; }

        public bool IsEnded => Loser != null;

        public bool IsStarted { get; private set; }

        public IReadOnlyCollection<IGamePlayerInfo> Players => GamePlayers;

        public GameRound CurrentRound 
        { 
            get
            {
                if (IsEnded)
                {
                    return null;
                }

                var lastRound = Rounds.LastOrDefault();
                if (lastRound != null && lastRound.Loser == null)
                {
                    return lastRound;
                }

                return null;
            }
        }

        public Game(CardsPack cardsPack, IReadOnlyCollection<GameTablePlayer> players)
        {
            if (cardsPack == null)
            {
                throw new ArgumentNullException(nameof(cardsPack));
            }

            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));
            }

            CardsPack = cardsPack;
            Rounds = new List<GameRound>();
            SetPlayers(players);
        }

        /// <summary>
        /// Устанавливает игроков в порядке ходов
        /// </summary>
        /// <param name="players"></param>
        private void SetPlayers(IReadOnlyCollection<GameTablePlayer> players)
        {
            GamePlayers = players.OrderBy(p => p.GameTablePlaceNumber)
                .Select(p => new GamePlayer(p.Player, p.GameTablePlaceNumber))
                .ToList();
        }

        /// <summary>
        /// Раздает игрокам карты
        /// </summary>
        private void DealCards()
        {
            var cardPerPlayer = CardsPack.Count() / GamePlayers.Count;
            foreach (var player in GamePlayers)
            {
                for (int i = 0; i < cardPerPlayer; i += cardPerPlayer)
                {
                    player.GiveCards(CardsPack.TakeMany(cardPerPlayer));
                }
            }
        }

        private void NextRound()
        {
            Rounds.Add(new GameRound());
        }

        private bool CanMakeMove()
        {
            return IsStarted && !IsEnded;
        }

        private bool ValidateCardsCount(IReadOnlyCollection<PlayingCard> cards)
        {
            return cards?.Count <= 4;
        }

        private bool ValidateFirstMove(FirstMove move)
        {
            if (!ValidateCardsCount(move.Cards))
            {
                return false;
            }

            if (!CardsPack.IsRankContains(move.Rank))
            {
                return false;
            }

            return true;
        }

        private GamePlayer GetNextMover()
        {
            var gamePlayer = GamePlayers.First(gp => gp.Player.Id == CurrentMover.Id);
            if (GamePlayers.Max(gp => gp.Priority) == gamePlayer.Priority)
            {
                return GamePlayers.First(gp => gp.Priority == GamePlayers.Min(p => p.Priority));
            }
            else
            {
                return GamePlayers
                    .Where(gp => gp.Priority > gamePlayer.Priority)
                    .First(gp => gp.Priority == GamePlayers.Where(p => p.Priority > gamePlayer.Priority).Min(p => p.Priority));
            }
        }

        /// <summary>
        /// Устанавливает следующего ходящего
        /// </summary>
        /// <param name="loser"></param>
        private void SetNextMover(GamePlayer loser = null)
        {
            if (CurrentMover == null)
            {
                CurrentMover = GamePlayers.First(p => p.Priority == GamePlayers.Min(pm => pm.Priority)).Player;
            }
            else if (loser == null || loser.Player.Id == CurrentMover.Id)
            {
                CurrentMover = GetNextMover().Player;
            }
        }

        private GamePlayer GetPreviousMover()
        {
            return GamePlayers.FirstOrDefault(gp => gp.Player.Id == CurrentRound.GetLastMove().InitiatorId);
        }

        /// <summary>
        /// Заканчивает игру
        /// </summary>
        /// <param name="loser"></param>
        private void End(GamePlayer loser)
        {
            if (IsEnded)
            {
                throw new TrueFalseGameException("Игра уже завершена");
            }

            if (!IsStarted)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            Loser = loser.Player;
            CurrentMover = null;
        }

        /// <summary>
        /// Запускает игру
        /// </summary>
        public void Start()
        {
            if (IsStarted)
            {
                throw new TrueFalseGameException("Игра уже запущена");
            }

            CardsPack.Shuffle();
            DealCards();
            NextRound();
            SetNextMover();
            IsStarted = true;
        }

        /// <summary>
        /// Делает ход "Первый ход"
        /// </summary>
        /// <param name="move"></param>
        public void MakeFirstMove(FirstMove move)
        {
            if (!CanMakeMove())
            {
                throw new TrueFalseGameException("В данный момент нельзя совершать ходы");
            }

            if (CurrentRound.MovesCount > 0)
            {
                throw new TrueFalseGameException("Первый ход уже был сделан");
            }

            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            var gamePlayer = GamePlayers.FirstOrDefault(p => p.Player.Id == move.InitiatorId);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {move.InitiatorId} нет за игровым столом");
            }

            if (CurrentMover.Id != gamePlayer.Player.Id)
            {
                throw new TrueFalseGameException($"Ход вне очереди со стороны пользователя с Id = {move.InitiatorId}");
            }

            if (!ValidateFirstMove(move))
            {
                throw new TrueFalseGameException("Не валидный ход");
            }

            gamePlayer.TakeCards(move.Cards.Select(c => c.Id).ToList());
            CurrentRound.AddMove(move);
            SetNextMover();
        }

        /// <summary>
        /// Делает ход "Верю"
        /// </summary>
        /// <param name="move"></param>
        public void MakeBeleiveMove(BelieveMove move, out IReadOnlyCollection<IPlayingCardInfo> takedLoserCards, out Guid loserId)
        {
            if (!CanMakeMove())
            {
                throw new TrueFalseGameException("В данный момент нельзя совершать ходы");
            }

            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (CurrentRound.MovesCount == 0)
            {
                throw new TrueFalseGameException("В данный момент нельзя совершать ход этого типа");
            }

            var gamePlayer = GamePlayers.FirstOrDefault(p => p.Player.Id == move.InitiatorId);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {move.InitiatorId} нет за игровым столом");
            }

            if (CurrentMover.Id != gamePlayer.Player.Id)
            {
                throw new TrueFalseGameException($"Ход вне очереди со стороны пользователя с Id = {move.InitiatorId}");
            }

            var lastCards = CurrentRound.GetLastCards();
            if (lastCards == null || lastCards.Count == 0)
            {
                throw new Exception("Ошибка логики кода. Ожидался не пустой список карт");
            }

            var selectedCard = lastCards.FirstOrDefault(c => c.Id == move.SelectedCardId);
            if (selectedCard == null)
            {
                throw new TrueFalseGameException("Указанныая пользователем карта не может быть выбрана для проверки так как ее нет в картах предыдущего хода");
            }

            var loserCards = CurrentRound.GetAllCards();
            takedLoserCards = loserCards;
            GamePlayer loser;
            if (selectedCard.Rank == CurrentRound.GetRank()) // Проиграл
            {
                loser = GetPreviousMover();
                loser.GiveCards(loserCards);
            }
            else // Выиграл
            {
                gamePlayer.GiveCards(loserCards);
                loser = gamePlayer;
            }

            CurrentRound.AddMove(move);
            CurrentRound.End(loser.Player);
            loserId = loser.Player.Id;

            if (GamePlayers.Where(gp => gp.Cards.Any()).Count() == 1) // Игра закончилась
            {
                End(loser);
            }
            else // Закончился раунд
            {
                NextRound();
                SetNextMover(loser);
            }
        }

        /// <summary>
        /// Делает ход "Не верю"
        /// </summary>
        /// <param name="move"></param>
        public void MakeDontBeleiveMove(DontBelieveMove move, out IReadOnlyCollection<IPlayingCardInfo> takedLoserCards, out Guid loserId)
        {
            if (!CanMakeMove())
            {
                throw new TrueFalseGameException("В данный момент нельзя совершать ходы");
            }

            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (CurrentRound.MovesCount == 0)
            {
                throw new TrueFalseGameException("В данный момент нельзя совершать ход этого типа");
            }

            var gamePlayer = GamePlayers.FirstOrDefault(p => p.Player.Id == move.InitiatorId);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {move.InitiatorId} нет за игровым столом");
            }

            if (CurrentMover.Id != gamePlayer.Player.Id)
            {
                throw new TrueFalseGameException($"Ход вне очереди со стороны пользователя с Id = {move.InitiatorId}");
            }

            var lastCards = CurrentRound.GetLastCards();
            if (lastCards == null || lastCards.Count == 0)
            {
                throw new Exception("Ошибка логики кода. Ожидался не пустой список карт");
            }

            var selectedCard = lastCards.FirstOrDefault(c => c.Id == move.SelectedCardId);
            if (selectedCard == null)
            {
                throw new TrueFalseGameException("Указанныая пользователем карта не может быть выбрана для проверки так как ее нет в картах последнего хода");
            }

            var loserCards = CurrentRound.GetAllCards();
            takedLoserCards = loserCards;
            GamePlayer loser;
            if (selectedCard.Rank == CurrentRound.GetRank()) // Проиграл
            {
                gamePlayer.GiveCards(loserCards);
                loser = gamePlayer;
            }
            else // Выиграл
            {
                loser = GetPreviousMover();
                loser.GiveCards(loserCards);
            }

            CurrentRound.AddMove(move);
            CurrentRound.End(loser.Player);
            loserId = loser.Player.Id;

            if (GamePlayers.Where(gp => gp.Cards.Any()).Count() == 1) // Игра закончилась
            {
                End(loser);
            }
            else // Закончился раунд
            {
                NextRound();
                SetNextMover(loser);
            }
        }

        /// <summary>
        /// Возвращает карты игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public IReadOnlyCollection<IPlayingCardInfo> GetPlayerCards(Guid playerId, IReadOnlyCollection<int> cardIds = null)
        {
            if (cardIds == null)
            {
                throw new ArgumentNullException(nameof(cardIds));
            }

            if (!IsStarted)
            {
                throw new TrueFalseGameException("Игра еще не началась. Карты не розданы");
            }

            if (IsEnded)
            {
                throw new TrueFalseGameException("Игра уже закончилась");
            }

            var gamePlayer = GamePlayers.FirstOrDefault(gp => gp.Player.Id == playerId);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {playerId} нет за игровым столом");
            }

            if (cardIds != null)
            {
                return gamePlayer.Cards.Where(c => cardIds.Contains(c.Id)).ToList();
            }

            return gamePlayer.Cards;
        }

        /// <summary>
        /// Возвращает карту из ходов текущего раунда по идентификатору
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public IPlayingCardInfo GetCardFromCurrentRoundById(int cardId)
        {
            if (CurrentRound == null)
            {
                throw new TrueFalseGameException("Игра еще не началась или уже закончилась");
            }

            return CurrentRound.GetCardById(cardId);
        }

        /// <summary>
        /// Удаляет пользователя из игры
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(Player player)
        {
            throw new NotImplementedException();

            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var gamePlayer = GamePlayers.FirstOrDefault(gp => gp.Player.Id == player.Id);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {player.Id} нет в данной игре");
            }

            if (!IsStarted || IsEnded)
            {
                GamePlayers.Remove(gamePlayer);// todo Убрать удаление игрока из коллекции, но проставить ему флаг типа он ливнул, чтобы не учитывать его при расчете некст хода
            }
            else
            {

            }
        }

        /// <summary>
        /// Возвращает список типов возможных ходов
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Type> GetNextPossibleMoves()
        {
            var result = new List<Type>();

            if (!CanMakeMove())
            {
                return result;
            }

            if (CurrentRound.MovesCount == 0)
            {
                result.Add(typeof(FirstMove));
            }
            else
            {
                result.Add(typeof(DontBelieveMove));
                result.Add(typeof(BelieveMove));
            }

            return result;
        }
    }
}
