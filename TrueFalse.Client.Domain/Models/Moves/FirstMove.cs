using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Moves
{
    public class FirstMove : Move
    {
        private PlayingCardRank _rank;
        public PlayingCardRank Rank
        {
            get => _rank;
            set
            {
                _rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }

        public ObservableCollection<PlayingCard> SelectedCards { get; private set; }

        public Player NextMover { get; set; }

        public FirstMove(Player initiator) : base(initiator)
        {
            SelectedCards = new ObservableCollection<PlayingCard>();
        }
    }
}
