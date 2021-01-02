using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.ViewModels;

namespace TrueFalse.Client.ConsoleApp.Views
{
    public class MainMenuView : BaseView
    {
        private MainMenuViewModel _viewModel;

        public MainMenuView(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public int StartSelectMenuInput()
        {
            while (true)
            {
                try
                {
                    var result = int.Parse(Console.ReadLine());
                    if (result != 1)
                    {
                        Console.WriteLine("Нет такого пункта меню");
                    }
                    else
                    {
                        return result;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Не корректный ввод");
                }
            }
        }

        private void Navigate(int pageNum)
        {
            if (pageNum != 1)
            {
                throw new Exception("Нет такого пункта меню");
            }

            _viewModel.NavigateCommand.Execute(nameof(GameTablesViewModel));
        }

        public override void Start()
        {
            Console.WriteLine("Найти игру - 1");
            var input = StartSelectMenuInput();
            Navigate(input);
        }

        public override void Stop()
        {
            return;
        }
    }
}
