using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;

namespace TrueFalse.Client.Domain.Models.Moves
{
    public class BeliveMove : Move
    {
        public ObservableCollection<PlayingCard> SelectedCards { get; private set; }

        public BeliveMove()
        {
            SelectedCards = new ObservableCollection<PlayingCard>();
        }
    }
}
