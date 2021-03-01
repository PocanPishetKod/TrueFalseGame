﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Games;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.Models.GameTables
{
    public class GameTable : ICreatableGameTable
    {
        public Guid Id { get; set; }

        public Game CurrentGame { get; set; }

        public Player Owner { get; set; }

        public List<GameTablePlayer> Players { get; set; }

        public GameTableType Type { get; set; }

        public string Name { get; set; }

        public bool IsStarted => CurrentGame != null;

        public bool IsInvalid => Owner == null;

        public bool CanStart
        {
            get
            {
                switch (Type)
                {
                    case GameTableType.Cards36And3Players:
                        return Players.Count == 3;
                    case GameTableType.Cards36And4Players:
                    case GameTableType.Cards52And4Players:
                        return Players.Count == 4;
                    case GameTableType.Cards52And5Players:
                        return Players.Count == 4;
                    default:
                        throw new Exception($"Нет обработчика для значения {Type}");
                }
            }
        }

        public bool IsFull => CanStart;

        public void SetNextPossibleMoves(IReadOnlyCollection<MoveType> moveTypes)
        {
            if (!IsStarted)
            {
                return;
            }

            CurrentGame.SetNextPossibleMoves(moveTypes);
        }

        public void StartGame(Guid moverId, IReadOnlyCollection<PlayerCardsInfoDto> playerCardsInfos)
        {
            if (IsStarted)
            {
                return;
            }

            CurrentGame = new Game();
            CurrentGame.Start(playerCardsInfos.Sum(pc => pc.CardsCount), Players.First(p => p.Player.Id == moverId).Player);
        }

        public void JoinPlayer(Player player, int placeNumber)
        {
            if (IsStarted || IsFull)
            {
                return;
            }

            Players.Add(new GameTablePlayer(player, placeNumber));
        }

        public void LeavePlayer(Player player)
        {
            if (!IsStarted)
            {
                return;
            }

            var gamePlayer = Players.FirstOrDefault(gp => gp.Player.Id == player.Id);
            if (gamePlayer == null)
            {
                throw new NullReferenceException($"Игрока с Id = {player.Id} и именем {player.Name} нет за столом");
            }

            Players.Remove(gamePlayer);

            if (player.Id == Owner.Id)
            {
                Owner = null;
            }
        }

        public void MakeFirstMove(FirstMove move, Guid nextMoverId)
        {
            if (!IsStarted || CurrentGame.CurrentMover.Id != move.Initiator.Id)
            {
                return;
            }

            CurrentGame.MakeFirstMove(move, Players.First(p => p.Player.Id == nextMoverId).Player);
        }
    }
}
