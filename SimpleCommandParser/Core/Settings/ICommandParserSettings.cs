using System;

namespace SimpleCommandParser.Core.Settings
{
    /// <summary>
    /// Настройки компонента распознания команд.
    /// </summary>
    public interface ICommandParserSettings
    {
        /// <summary>
        /// Культура сравнения строк при разборе команд.
        /// </summary>
        StringComparison StringComparsion { get; }
        
        /// <summary>
        /// Префикс команды.
        /// </summary>
        string VerbPrefix { get; }
        
        /// <summary>
        /// Префикс перед ключом параметра команды.
        /// </summary>
        string ArgumentKeyPrefix { get; }
    }
}