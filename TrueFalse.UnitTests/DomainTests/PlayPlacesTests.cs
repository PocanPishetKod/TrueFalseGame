using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Players;
using Xunit;

namespace TrueFalse.UnitTests.DomainTests
{
    public class PlayPlacesTests
    {
        private void CreatePlayPlacesTestInternal(PlayPlaces places)
        {
            Assert.Empty(places.Players);
            Assert.False(places.IsFull);
        }

        [Fact]
        public void CreatePlayPlacesTest()
        {
            CreatePlayPlacesTestInternal(new Play3Places());
            CreatePlayPlacesTestInternal(new Play4Places());
            CreatePlayPlacesTestInternal(new Play5Places());
        }

        private void AddPlayers(PlayPlaces places, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var player = new Player(Guid.NewGuid(), i.ToString());
                places.PlantPlayer(player);
            }
        }

        private void RemoveAllPlayers(PlayPlaces places)
        {
            while (places.Players.Count > 0)
            {
                places.RemovePlayer(places.Players.Last().Player);
            }
        }

        [Fact]
        public void AddAndRemovePlayersTest()
        {
            PlayPlaces places = new Play3Places();
            AddPlayers(places, 3);
            Assert.Equal(3, places.Players.Count);
            Assert.True(places.IsFull);
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(new Player(Guid.NewGuid(), "Test")); });
            places.RemovePlayer(places.Players.Last().Player);
            Assert.Equal(2, places.Players.Count);
            var player = places.Players.Last().Player;
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(player); });
            RemoveAllPlayers(places);
            Assert.Equal(0, places.Players.Count);
            Assert.Throws<TrueFalseGameException>(() => { places.RemovePlayer(player); });

            places = new Play4Places();
            AddPlayers(places, 4);
            Assert.Equal(4, places.Players.Count);
            Assert.True(places.IsFull);
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(new Player(Guid.NewGuid(), "Test")); });
            player = places.Players.Last().Player;
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(player); });
            places.RemovePlayer(places.Players.Last().Player);
            Assert.Equal(3, places.Players.Count);
            RemoveAllPlayers(places);
            Assert.Equal(0, places.Players.Count);
            Assert.Throws<TrueFalseGameException>(() => { places.RemovePlayer(player); });

            places = new Play5Places();
            AddPlayers(places, 5);
            Assert.Equal(5, places.Players.Count);
            Assert.True(places.IsFull);
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(new Player(Guid.NewGuid(), "Test")); });
            player = places.Players.Last().Player;
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(player); });
            places.RemovePlayer(places.Players.Last().Player);
            Assert.Equal(4, places.Players.Count);
            RemoveAllPlayers(places);
            Assert.Equal(0, places.Players.Count);
            Assert.Throws<TrueFalseGameException>(() => { places.RemovePlayer(player); });
        }

        private void CheckPlaceNumbers(PlayPlaces places)
        {
            var placeNumber = 1;
            foreach (var player in places.Players)
            {
                Assert.Equal(placeNumber, player.GameTablePlaceNumber);
                placeNumber++;
            }
        }

        [Fact]
        public void PlaceNumbersTest()
        {
            PlayPlaces places = new Play3Places();
            AddPlayers(places, 3);
            CheckPlaceNumbers(places);

            places = new Play4Places();
            AddPlayers(places, 4);
            CheckPlaceNumbers(places);

            places = new Play5Places();
            AddPlayers(places, 5);
            CheckPlaceNumbers(places);

            // todo подумать насчет изменения порядка номеров при удалении игрока
        }
    }
}
