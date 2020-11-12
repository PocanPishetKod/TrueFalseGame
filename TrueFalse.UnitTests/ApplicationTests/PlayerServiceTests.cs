using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Application.Services;
using Xunit;

namespace TrueFalse.UnitTests.ApplicationTests
{
    public class PlayerServiceTests
    {
        private readonly PlayerService _playerService;

        public PlayerServiceTests(PlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Проверяет создание пользователя
        /// </summary>
        [Fact]
        public void CreatePlayerTest()
        {
            var playerName = "TestPlayer";
            var player = _playerService.CreatePlayer(playerName);

            Assert.NotNull(player);
            Assert.Equal(playerName, player.Name);
            Assert.False(new Guid().Equals(player.Id));
        }
    }
}
