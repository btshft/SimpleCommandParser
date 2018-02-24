using System;

namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Модель результата успешной обработки команды.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ParsedCommand<TModel> : ICommandParseResult<TModel> where TModel : class, new()
    {
        /// <inheritdoc />
        public CommandParseState ParseState { get; }
        
        /// <summary>
        /// Модель команды.
        /// </summary>
        public TModel Value { get; }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="ParsedCommand{TModel}"/>.
        /// </summary>
        /// <param name="value">Модель команды.</param>
        public ParsedCommand(TModel value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            Value = value;
            ParseState = CommandParseState.Parsed;
        }
    }
}