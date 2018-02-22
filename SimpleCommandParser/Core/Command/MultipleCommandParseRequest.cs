using System;

namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Модель запроса разбора множества команд.
    /// </summary>
    public class MultipleCommandParseRequest : ICommandParseRequest
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="MultipleCommandParseRequest"/>.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <param name="modelTypes">Типы моделей.</param>
        internal MultipleCommandParseRequest(string input, Type[] modelTypes)
        {
            ModelTypes = modelTypes;
            Input = input;
        }

        /// <inheritdoc />
        public string Input { get; }

        /// <summary>
        /// Типы моделей.
        /// </summary>
        public Type[] ModelTypes { get; }
    }
}