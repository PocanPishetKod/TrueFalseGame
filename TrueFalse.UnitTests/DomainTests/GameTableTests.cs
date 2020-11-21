using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;
using Xunit;

namespace TrueFalse.UnitTests.DomainTests
{
    public class GameTableTests
    {
        private void CreateGameTableTestInternal(GameTable gameTable, Player owner, string gameTableName)
        {
            Assert.NotEqual(new Guid(), gameTable.Id);
            Assert.Equal(1, gameTable.Players.Count);
            Assert.Equal(owner.Id, gameTable.Players.First().Player.Id);
            Assert.Equal(owner.Id, gameTable.Owner.Id);
            Assert.Equal(gameTableName, gameTable.Name);
        }

        [Fact]
        public void CreateGameTableTest()
        {
            var owner = new Player(Guid.NewGuid(), "Test");
            var gameTableName = "Test";

            CreateGameTableTestInternal(new GameTable3To36(owner, gameTableName, Guid.NewGuid()), owner, gameTableName);
            CreateGameTableTestInternal(new GameTable4To36(owner, gameTableName, Guid.NewGuid()), owner, gameTableName);
            CreateGameTableTestInternal(new GameTable4To52(owner, gameTableName, Guid.NewGuid()), owner, gameTableName);
            CreateGameTableTestInternal(new GameTable5To52(owner, gameTableName, Guid.NewGuid()), owner, gameTableName);
        }

        private void FillGameTable(GameTable gameTable)
        {
            while (!gameTable.IsFull)
            {
                gameTable.Join(new Player(Guid.NewGuid(), "Test"));
            }
        }

        private void LeaveAll(GameTable gameTable)
        {
            while (gameTable.Players.Count != 0)
            {
                var toDeletePlayer = gameTable.Players.Last();
                gameTable.Leave(toDeletePlayer.Player);
            }
        }

        private void JoinLeaveTestInternal(GameTable gameTable, int maxPlayers, Player owner)
        {
            Assert.Throws<TrueFalseGameException>(() => { gameTable.Join(owner); });
            FillGameTable(gameTable);
            Assert.True(gameTable.IsFull);
            Assert.Equal(maxPlayers, gameTable.Players.Count);
            Assert.Throws<TrueFalseGameException>(() => { gameTable.Join(new Player(Guid.NewGuid(), "Test")); });
            var toRemovePlayer = gameTable.Players.First(p => p.Player.Id != owner.Id);
            gameTable.Leave(toRemovePlayer.Player);
            Assert.False(gameTable.IsFull);
            Assert.Equal(maxPlayers - 1, gameTable.Players.Count);
            gameTable.Leave(owner);
            Assert.False(gameTable.IsFull);
            Assert.NotNull(gameTable.Owner);
            Assert.NotEqual(owner.Id, gameTable.Owner.Id);
            Assert.Equal(gameTable.Players.First(gp => gp.GameTablePlaceNumber == gameTable.Players.Min(p => p.GameTablePlaceNumber)).Player.Id, gameTable.Owner.Id);
            LeaveAll(gameTable);
            Assert.False(gameTable.IsFull);
            Assert.True(gameTable.IsInvalid);
        }

        [Fact]
        public void JoinLeaveTest()
        {
            var owner = new Player(Guid.NewGuid(), "Test");

            JoinLeaveTestInternal(new GameTable3To36(owner, "Test", Guid.NewGuid()), 3, owner);
            JoinLeaveTestInternal(new GameTable4To36(owner, "Test", Guid.NewGuid()), 4, owner);
            JoinLeaveTestInternal(new GameTable4To52(owner, "Test", Guid.NewGuid()), 4, owner);
            JoinLeaveTestInternal(new GameTable5To52(owner, "Test", Guid.NewGuid()), 5, owner);
        }

        private void InvalidTestInternal(GameTable gameTable)
        {
            FillGameTable(gameTable);
            LeaveAll(gameTable);
            Assert.True(gameTable.IsInvalid);
            var player = new Player(Guid.NewGuid(), "Test");
            Assert.Throws<TrueFalseGameException>(() => { gameTable.Join(player); });
            Assert.Throws<TrueFalseGameException>(() => { gameTable.Leave(player); });
            Assert.Throws<TrueFalseGameException>(() => { gameTable.StartNewGame(); });
            Assert.Throws<TrueFalseGameException>(() => { gameTable.MakeFirstMove(new FirstMove(new List<PlayingCard>(), PlayingCardRank.Ace, player.Id)); });
            Assert.Throws<TrueFalseGameException>(() => { gameTable.MakeBeleiveMove(new BelieveMove(new List<PlayingCard>(), player.Id)); });
            Assert.Throws<TrueFalseGameException>(() => { gameTable.MakeDontBeleiveMove(new DontBelieveMove(3, player.Id)); });
        }

        [Fact]
        public void InvalidTest()
        {
            var owner = new Player(Guid.NewGuid(), "Test");

            InvalidTestInternal(new GameTable3To36(owner, "Test", Guid.NewGuid()));
            InvalidTestInternal(new GameTable4To36(owner, "Test", Guid.NewGuid()));
            InvalidTestInternal(new GameTable4To52(owner, "Test", Guid.NewGuid()));
            InvalidTestInternal(new GameTable4To52(owner, "Test", Guid.NewGuid()));
        }
    }
}
