using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<TParam>
    {
        void Execute(TParam param);
    }
}
