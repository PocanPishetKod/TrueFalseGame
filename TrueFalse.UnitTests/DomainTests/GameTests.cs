using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Games;
using TrueFalse.Domain.Models.GameTables;
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
        }
    }
}
