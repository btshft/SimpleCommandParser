namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Состояние команды.
    /// </summary>
    public enum CommandParseState
    {
        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Команда распознана.
        /// </summary>
        Parsed = 1,

        /// <summary>
        /// Команда не распознана.
        /// </summary>
        Unparsed = 2,
        
        /// <summary>
        /// Команда была обработана.
        /// </summary>
        Consumed = 3,
    }
}