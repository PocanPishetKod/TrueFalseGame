using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.Domain.Commands
{
    public class NavigateCommand : ICommand<string>
    {
        private readonly BaseViewModel _viewModel;

        public NavigateCommand(BaseViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            _viewModel = viewModel;
        }

        public void Execute(string viewModelName)
        {
            if (string.IsNullOrWhiteSpace(viewModelName))
            {
                throw new ArgumentNullException(nameof(viewModelName));
            }

            _viewModel.Navigate(viewModelName);
        }
    }
}
