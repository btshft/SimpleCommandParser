using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Core.ParseStrategy
{
    /// <summary>
    /// Стратегия разбора команды.
    /// </summary>
    public interface ICommandParseStrategy
    {
        /// <summary>
        /// Выполняет разбор команды.
        /// </summary>
        /// <param name="request">Модель запроса.</param>
        /// <typeparam name="TModel">Тип модели.</typeparam>
        /// <returns>Результат разбора команды.</returns>
        ICommandParseResult<TModel> ParseCommand<TModel>(SingleCommandParseRequest<TModel> request)
            where TModel : class, new();

        /// <summary>
        /// Выполняет разбор множества команд.
        /// </summary>
        /// <param name="request">Модель запроса.</param>
        /// <returns>Результат разбора команды.</returns>
        ICommandParseResult<object> ParseCommands(MultipleCommandParseRequest request);
    }
}