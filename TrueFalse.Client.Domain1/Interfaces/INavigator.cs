using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.Domain.Interfaces
{
    public interface INavigator
    {
        void Navigate<TViewModel>() where TViewModel : BaseViewModel;
    }
}
