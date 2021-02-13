using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Exceptions;
using TrueFalse.Client.Domain.Models.Cards;

namespace TrueFalse.Client.Domain.Models.Moves
{
    public class DontBeliveMove : Move
    {
        private PlayingCard _selectedCard;
        public PlayingCard SelectedCard
        {
            get => _selectedCard;
            set
            {
                if (_selectedCard != null)
                {
                    throw new TrueFalseGameException("Карта уже выбрана");
                }

                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }
    }
}
