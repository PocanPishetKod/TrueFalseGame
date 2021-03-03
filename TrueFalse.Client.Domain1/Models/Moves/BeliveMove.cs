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
    public class BeliveMove : Move
    {
        private PlayingCard _selectedCard;
        public PlayingCard SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard != null)
                {
                    return;
                }

                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public override bool IsValid => _selectedCard != null;

        public BeliveMove(Player initiator) : base(initiator)
        {
            
        }
    }
}
