using System;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser
{
    /// <summary>
    /// Интерфейс компонента для разбора команд.
    /// </summary>
    public interface ICommandParser
    {
        /// <summary>
        /// Выполняет разбор команды.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <returns>Результат разбора команды.</returns>
        ICommandParseResult<TModel> ParseCommand<TModel>(string input)
            where TModel : class, new();

        /// <summary>
        /// Выполняет попытку разбора множества команд.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        /// <param name="supportedTypes">Поддерживаемые типы.</param>
        /// <returns>Результат разбора команды.</returns>
        ICommandParseResult<object> ParseCommands(string input, Type[] supportedTypes);
    }
}