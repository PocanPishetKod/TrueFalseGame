using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos.Results
{
    public class FirstMoveResult
    {
        public Guid GameTableId { get; set; }

        public Guid NextMoverId { get; set; }
    }
}
