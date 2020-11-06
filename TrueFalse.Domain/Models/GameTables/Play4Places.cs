using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.GameTables
{
    public class Play4Places : PlayPlaces
    {
        protected override int PlacesCount => 4;
    }
}
