namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Запрос разбора команды.
    /// </summary>
    public interface ICommandParseRequest
    {
        /// <summary>
        /// Входная строка.
        /// </summary>
        string Input { get; }
    }
}