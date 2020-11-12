using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Application.Services;
using Xunit;

namespace TrueFalse.UnitTests.ApplicationTests
{
    public class GameTableServiceTests
    {
        private readonly GameTableService _gameTableService;
        private readonly PlayerService _playerService;

        public GameTableServiceTests(GameTableService gameTableService, PlayerService playerService)
        {
            _gameTableService = gameTableService;
            _playerService = playerService;
        }

        /// <summary>
        /// Тестироует создание игровой комнаты
        /// </summary>
        [Fact]
        public void CreateGameTableTest()
        {
            var owner = _playerService.CreatePlayer("TestPlayer");

            var gameTableName = "TestGameTable";
            var gameTable = _gameTableService.CreateGameTable(owner.Id, gameTableName, 4, 36);

            Assert.NotNull(gameTable);
            Assert.False(new Guid().Equals(gameTable.Id));
            Assert.Equal(owner.Id, gameTable.Owner.Id);
            Assert.Equal(gameTableName, gameTable.Name);
            Assert.NotEmpty(gameTable.Players);
            Assert.Equal(owner.Id, gameTable.Players.First().Player.Id);
        }
    }
}
