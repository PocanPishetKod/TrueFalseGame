using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Interfaces
{
    public interface IDispatcher
    {
        void Invoke(Action action);
    }
}
