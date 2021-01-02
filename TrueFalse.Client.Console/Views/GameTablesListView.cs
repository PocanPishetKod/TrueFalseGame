using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Commands.Parameters;
using TrueFalse.Client.Domain.Models.GameTables;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.ConsoleApp.Views
{
    public class GameTablesListView : BaseView
    {
        private GameTablesViewModel _viewModel;

        public GameTablesListView(GameTablesViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool InputAsk()
        {
            while (true)
            {
                var result = Console.ReadLine();
                if (!result.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && !result.Equals("no", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Некорректный ввод");
                }
                else if (result.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
        }

        private void SubscribeOnGameTables()
        {
            _viewModel.GameTables.CollectionChanged += OnGameTablesCollectionChanged;
        }

        private void OnGameTablesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                var newItem = args.NewItems[0] as GameTable;
                Console.WriteLine($"{newItem.Id} | {newItem.Name} | {newItem.Players.Count} | Owner = {newItem.Name}");
            }
        }

        public override void Start()
        {
            SubscribeOnGameTables();
            Console.WriteLine("Показать игровые столы? (yes/no)");
            var input = InputAsk();
            Console.WriteLine();
            
            if (input)
            {
                _viewModel.LoadGameTablesCommand.Execute(new LoadGameTablesParams() { PageNum = 1, PerPage = 25 });
            }

            Console.ReadLine();
        }

        public override void Stop()
        {
            _viewModel.GameTables.CollectionChanged -= OnGameTablesCollectionChanged;
        }
    }
}
