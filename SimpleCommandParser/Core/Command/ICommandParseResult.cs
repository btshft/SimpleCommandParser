namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Интерфейс результата разбора команды.
    /// </summary>
    /// <typeparam name="TModel">Тип модели.</typeparam>
    public interface ICommandParseResult<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Состояние разбора команды.
        /// </summary>
        CommandParseState ParseState { get; }
    }
}