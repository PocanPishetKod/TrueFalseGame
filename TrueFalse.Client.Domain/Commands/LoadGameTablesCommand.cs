using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Commands.Parameters;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.Domain.Commands
{
    public class LoadGameTablesCommand : ICommand<LoadGameTablesParams>
    {
        private readonly GameTablesViewModel _viewModel;

        public LoadGameTablesCommand(GameTablesViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            _viewModel = viewModel;
        }

        public void Execute(LoadGameTablesParams param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            _viewModel.LoadGameTables();
        }
    }
}
