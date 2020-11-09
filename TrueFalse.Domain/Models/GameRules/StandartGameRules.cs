using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Domain.Models.GameRules
{
    public class StandartGameRules
    {
        private IReadOnlyDictionary<string, IReadOnlyCollection<ICheckingGameRule<IMove>>> _checkingRules;
        private IReadOnlyDictionary<string, IReadOnlyCollection<IExecutingGameRule<IMove>>> _executingRules;

        public StandartGameRules()
        {
            CreateRules();
        }

        private void CreateRules()
        {
            _checkingRules = new Dictionary<string, IReadOnlyCollection<ICheckingGameRule<IMove>>>();
            _executingRules = new Dictionary<string, IReadOnlyCollection<IExecutingGameRule<IMove>>>();
        }

        /// <summary>
        /// Возвращает зарегистрированные проверяющие правила по типу хода. Если правил не найдено: выбрасывается исключение
        /// </summary>
        /// <typeparam name="TMove"></typeparam>
        /// <returns></returns>
        private IReadOnlyCollection<ICheckingGameRule<TMove>> GetCheckingGameRules<TMove>() where TMove : IMove
        {
            if (_checkingRules.TryGetValue(typeof(TMove).Name, out var rules))
            {
                return rules.Cast<ICheckingGameRule<TMove>>().ToList();
            }

            throw new TrueFalseGameException($"Правил для обработки хода типа {typeof(TMove).Name} не было зарегистрировано");
        }

        /// <summary>
        /// Возвращает зарегистрированные исполняющие правила по типу хода. Если правил не найдено: выбрасывается исключение
        /// </summary>
        /// <typeparam name="TMove"></typeparam>
        /// <returns></returns>
        private IReadOnlyCollection<IExecutingGameRule<TMove>> GetExecutingGameRules<TMove>() where TMove : IMove
        {
            if (_executingRules.TryGetValue(typeof(TMove).Name, out var rules))
            {
                return rules.Cast<IExecutingGameRule<TMove>>().ToList();
            }

            throw new TrueFalseGameException($"Правил для обработки хода типа {typeof(TMove).Name} не было зарегистрировано");
        }

        /// <summary>
        /// Проверяет ход на соответствие правилам игры
        /// </summary>
        /// <param name="move"></param>
        /// <param name="gameTable"></param>
        /// <returns></returns>
        public bool CheckMove(IMove move, GameTable gameTable)
        {
            var rules = GetCheckingGameRules<IMove>();
            foreach (var rule in rules)
            {
                if (!rule.Check(move, gameTable))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Исполняет ход в соответствии с правилами игры
        /// </summary>
        /// <param name="move"></param>
        /// <param name="gameTable"></param>
        public void ExecuteMove(IMove move, GameTable gameTable)
        {
            var rules = GetExecutingGameRules<IMove>();
            foreach (var rule in rules)
            {
                rule.Execute(move, gameTable);
            }
        }
    }
}
