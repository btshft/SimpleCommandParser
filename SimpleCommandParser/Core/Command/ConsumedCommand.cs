using System;
using System.Collections.Generic;

namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Команда, которая была обработана.
    /// </summary>
    public class ConsumedCommand<TModel> : CommandParseResult<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Оригинальная команда.
        /// </summary>
        public ICommandParseResult<TModel> Original { get; }

        /// <summary>
        /// Значение команды.
        /// </summary>
        public TModel Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("Команда не была разобрана. Действие недопустимо.");
                return ((ParsedCommand<TModel>) Original).Value;
            }
        }

        /// <summary>
        /// Коллекция ошибок.
        /// </summary>
        public IReadOnlyCollection<CommandParseError> Errors => !HasErrors 
            ? Array.Empty<CommandParseError>() 
            : ((UnparsedCommand<TModel>) Original).Errors;

        /// <summary>
        /// Признак того, что команда была разобрана.
        /// </summary>
        public bool HasValue => Original is ParsedCommand<TModel>;

        /// <summary>
        /// Признак того, что команда не была разобрана.
        /// </summary>
        public bool HasErrors => Original is UnparsedCommand<TModel>;
        
        /// <summary>
        /// Инициализиуер экземпляр <see cref="ConsumedCommand{TModel}"/>.
        /// </summary>
        /// <param name="original">Оригинальная команда.</param>
        internal ConsumedCommand(ICommandParseResult<TModel> original)
            :base(CommandParseState.Consumed)
        {
            Original = original;
        }
    }
}