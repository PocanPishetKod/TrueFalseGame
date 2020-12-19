using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.ConsoleApp
{
    public class App
    {
        private const string SavesPath = @"C:\TrueFalse saves";

        public App()
        {
            SubscrybeOnEvents();
        }

        public void SubscrybeOnEvents()
        {

        }

        private void OnReceiveGameTables(ReceiveGameTablesParams @params)
        {
            foreach (var item in @params.GameTables)
            {
                Console.WriteLine($"Id = {item.Id} | Name = {item.Name}");
            }
        }

        private void OnReceiveNewGameTable(ReceiveNewGameTableParams @params)
        {

        }

        private string InputNameProcess()
        {
            while (true)
            {
                Console.WriteLine("Введите ник:");

                var result = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine("Введите корректный ник");
                }
                else
                {
                    return result;
                }
            }
        }

        private void RequestGameTablesProcess()
        {
            while (true)
            {
                Console.WriteLine("Показать игровые столы? (Да/Нет)");
                var input = Console.ReadLine();
                if (input.ToUpper() == "ДА")
                {
                    //_application.RequestGameTables();
                    break;
                }
            }
        }

        public void Start()
        {
            while (true)
            {
                var playerName = InputNameProcess();
                //_application.Authenticate(playerName).Wait();
                RequestGameTablesProcess();

                Console.WriteLine("Нажмите любую клавишу, чтобы выйти");
                Console.ReadLine();
                break;
            }
        }
    }
}
