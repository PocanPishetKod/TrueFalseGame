using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Models.Cards
{
    public class PlayingCard : BaseModel
    {
        public int Id { get; set; }

        public PlayingCardSuit? Suit { get; set; }

        public PlayingCardRank? Rank { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
