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
        string CommandVerbPrefix { get; }
        
        /// <summary>
        /// Разделитель между ключом параметра и его значением.
        /// Если не задано, используется пробел.
        /// </summary>
        char? CommandArgumentKeyValueDelimeter { get; }
        
        /// <summary>
        /// Префикс ключа команды.
        /// </summary>
        char? CommandArgumentKeyPrefix { get;  }
    }
}