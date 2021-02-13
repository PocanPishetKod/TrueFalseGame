using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class Game
    {
        public List<GameRound> GameRounds { get; private set; }

        public GameRound CurrentRound => GameRounds.LastOrDefault();

        public CardsPack CardsPack { get; private set; }

        public Player CurrentMover { get; private set; }

        public void Start(int cardsCount, Player mover)
        {
            CardsPack = new CardsPack(cardsCount);
            CurrentMover = mover;
            GameRounds = new List<GameRound>() { new GameRound() };
        }

        public void MakeFirstMove(FirstMove move, Player nextMover)
        {
            CurrentRound.Moves.Add(move);
            CurrentMover = nextMover;
        }

        public void MakeBeliveMove(BeliveMove move, Player nextMover)
        {

        }

        public void MakeDontBeliveMove(DontBeliveMove move, Player nextMover)
        {

        }
    }
}
