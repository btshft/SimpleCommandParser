namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Базовый класс результата разбора команды.
    /// </summary>
    /// <typeparam name="TModel">Тип модели.</typeparam>
    public abstract class CommandParseResult<TModel> : ICommandParseResult<TModel>
        where TModel : class, new()
    {
        /// <inheritdoc />
        public CommandParseState ParseState { get; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseResult{TModel}"/>.
        /// </summary>
        /// <param name="parseState">Состояние разбора команды.</param>
        protected CommandParseResult(CommandParseState parseState)
        {
            ParseState = parseState;
        }
    }
}