using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Games;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;
using Xunit;

namespace TrueFalse.UnitTests.DomainTests
{
    public class GameTests
    {
        private List<GameTablePlayer> ProvideGameTablePlayers(int count)
        {
            var result = new List<GameTablePlayer>();
            for (int i = 1; i <= count; i++)
            {
                result.Add(new GameTablePlayer(new Player(Guid.NewGuid(), $"Test{i}"), i));
            }

            return result;
        }

        [Fact]
        public void CreateGameTest()
        {
            var players = ProvideGameTablePlayers(3);
            var cardsPack = new CardsPack36();
            var game = new Game(cardsPack, players);
            Assert.NotEmpty(game.Players);
            Assert.False(game.IsStarted);
            Assert.False(game.IsEnded);
            Assert.Null(game.CurrentRound);
            Assert.Null(game.CurrentMover);
        }

        [Fact]
        public void StartGameTest()
        {
            var players = ProvideGameTablePlayers(3);
            var cardsPack = new CardsPack36();
            var game = new Game(cardsPack, players);

            game.Start();

            Assert.True(game.IsStarted);
            Assert.False(game.IsEnded);
            Assert.NotNull(game.CurrentMover);
            Assert.NotNull(game.CurrentRound);

            var mover = game.Players.First(p => p.Priority == game.Players.Min(pp => pp.Priority));
            Assert.Equal(mover.Player.Id, game.CurrentMover.Id);

            foreach (var player in game.Players)
            {
                Assert.NotEmpty(player.Cards);
            }
        }

        [Fact]
        public void FirstMoveTest()
        {
            var players = ProvideGameTablePlayers(3);
            var cardsPack = new CardsPack36();
            var game = new Game(cardsPack, players);
            game.Start();

            var mover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            var cardsCount = mover.Cards.Count;

            game.MakeFirstMove(new FirstMove(mover.Cards.Take(4).ToList(), PlayingCardRank.Ten, mover.Player.Id));

            Assert.Equal(cardsCount - 4, mover.Cards.Count);
            Assert.NotEqual(mover.Player.Id, game.CurrentMover.Id);
            Assert.Throws<TrueFalseGameException>(() => { game.MakeFirstMove(new FirstMove(mover.Cards.Take(4).ToList(), PlayingCardRank.Ten, game.CurrentMover.Id)); });
        }

        [Fact]
        public void BeliveMoveTest()
        {
            var players = ProvideGameTablePlayers(3);
            var cardsPack = new CardsPack36();
            var game = new Game(cardsPack, players);
            game.Start();

            var mover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            var cardsCount = mover.Cards.Count;

            Assert.Throws<TrueFalseGameException>(() => { game.MakeBeleiveMove(new BelieveMove(mover.Cards.Take(3).ToList(), mover.Player.Id)); });

            game.MakeFirstMove(new FirstMove(mover.Cards.Take(4).ToList(), PlayingCardRank.Ten, mover.Player.Id));

            mover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            cardsCount = mover.Cards.Count;

            game.MakeBeleiveMove(new BelieveMove(mover.Cards.Take(3).ToList(), mover.Player.Id));
            Assert.Equal(cardsCount - 3, mover.Cards.Count);
            Assert.NotEqual(mover.Player.Id, game.CurrentMover.Id);
            Assert.Throws<TrueFalseGameException>(() => { game.MakeBeleiveMove(new BelieveMove(mover.Cards.Take(3).ToList(), game.CurrentMover.Id)); });
        }

        [Fact]
        public void DontBeliveMoveTest()
        {
            var players = ProvideGameTablePlayers(3);
            var cardsPack = new CardsPack36();
            var game = new Game(cardsPack, players);
            game.Start();

            var mover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            var cardsCount = mover.Cards.Count;

            game.MakeFirstMove(new FirstMove(mover.Cards.Take(4).ToList(), PlayingCardRank.Ten, mover.Player.Id));

            var previousMover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            cardsCount = previousMover.Cards.Count;

            var cards = previousMover.Cards.Take(3).ToList();
            game.MakeBeleiveMove(new BelieveMove(cards, previousMover.Player.Id));

            mover = game.Players.First(p => p.Player.Id == game.CurrentMover.Id);
            cardsCount = mover.Cards.Count;

            game.MakeDontBeleiveMove(new DontBelieveMove(cards.First().Id, mover.Player.Id), out var takedLoserCards, out Guid loserId);

            if (cards.First().Rank == PlayingCardRank.Ten)
            {
                Assert.Equal(mover.Player.Id, loserId);
            }
            else
            {
                Assert.Equal(previousMover.Player.Id, loserId);
            }
        }
    }
}
