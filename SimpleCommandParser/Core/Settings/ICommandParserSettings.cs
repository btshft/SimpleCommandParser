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
        
        /// <summary>
        /// При разборе команд указание ключей параметров является обязательным.
        /// Отключение этой настройки позволит обрабатывать команды вида
        /// '/command value1 value2', при этом параметры value1 и value2 будут установлены
        /// на свойства модели в соответствии с порядком расположения атрибутов (при использовании стандартного парсера).
        /// </summary>
        /// <remarks>
        ///     Пока в тестовом режиме. Флаги не поддерживаются, как и собственный порядок мапинга опций.
        /// </remarks>
        bool RequireArgumentKeyPrefix { get; }
    }
}