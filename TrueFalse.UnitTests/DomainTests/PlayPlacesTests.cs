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

        private void AddAndRemovePlayersTestInternal(PlayPlaces places, int maxPlaces)
        {
            AddPlayers(places, maxPlaces);
            Assert.Equal(maxPlaces, places.Players.Count);
            Assert.True(places.IsFull);
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(new Player(Guid.NewGuid(), "Test")); });
            var player = places.Players.Last().Player;
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(player); });
            places.RemovePlayer(player);
            Assert.Equal(maxPlaces - 1, places.Players.Count);
            Assert.Null(places.Players.FirstOrDefault(p => p.Player.Id == player.Id));
            player = places.Players.Last().Player;
            Assert.Throws<TrueFalseGameException>(() => { places.PlantPlayer(player); });
            RemoveAllPlayers(places);
            Assert.Equal(0, places.Players.Count);
            Assert.Throws<TrueFalseGameException>(() => { places.RemovePlayer(player); });
        }

        [Fact]
        public void AddAndRemovePlayersTest()
        {
            AddAndRemovePlayersTestInternal(new Play3Places(), 3);
            AddAndRemovePlayersTestInternal(new Play4Places(), 4);
            AddAndRemovePlayersTestInternal(new Play5Places(), 5);
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

        private Player GetMiddlePlayer(PlayPlaces places)
        {
            var isFirst = true;
            foreach (var player in places.Players)
            {
                if (!isFirst)
                {
                    return player.Player;
                }

                isFirst = false;
            }

            throw new Exception("Что-то не так");
        }

        private void PlaceNumbersTestInternal(PlayPlaces places, int maxPlaces)
        {
            AddPlayers(places, maxPlaces);
            CheckPlaceNumbers(places);
            var player = places.Players.First().Player;
            places.RemovePlayer(player);
            CheckPlaceNumbers(places);
            places.PlantPlayer(player);
            places.RemovePlayer(GetMiddlePlayer(places));
            CheckPlaceNumbers(places);
        }

        [Fact]
        public void PlaceNumbersTest()
        {
            PlaceNumbersTestInternal(new Play3Places(), 3);
            PlaceNumbersTestInternal(new Play4Places(), 4);
            PlaceNumbersTestInternal(new Play5Places(), 5);
        }
    }
}
