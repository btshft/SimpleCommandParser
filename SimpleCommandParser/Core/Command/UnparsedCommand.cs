using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Модель результата команды, которую не удалось обработать.
    /// </summary>
    /// <typeparam name="TModel">Тип модели.</typeparam>
    public class UnparsedCommand<TModel> : CommandParseResult<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Коллекция ошибок разбора.
        /// </summary>
        public IReadOnlyCollection<CommandParseError> Errors { get; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="UnparsedCommand{TModel}"/>.
        /// </summary>
        /// <param name="errors">Перечень ошибок.</param>
        internal UnparsedCommand(IEnumerable<CommandParseError> errors)
            : base(CommandParseState.Unparsed)
        {
            Errors = errors?.ToArray() ?? Array.Empty<CommandParseError>();
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="UnparsedCommand{TModel}"/>.
        /// </summary>
        /// <param name="error">Код ошибки.</param>
        internal UnparsedCommand(CommandParseError error)
            : base(CommandParseState.Unparsed)
        {
            Errors = new[] { error };
        }
    }
}