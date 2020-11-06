using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Exceptions
{
    public class TrueFalseGameException : Exception
    {
        public TrueFalseGameException(string message) : base(message)
        {

        }
    }
}
