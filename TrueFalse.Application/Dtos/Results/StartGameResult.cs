using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos.Results
{
    public class StartGameResult
    {
        public Guid GameTableId { get; set; }

        public Guid MoverId { get; set; }

        public List<PlayerCardsInfo> PlayerCards { get; set; }
    }
}
