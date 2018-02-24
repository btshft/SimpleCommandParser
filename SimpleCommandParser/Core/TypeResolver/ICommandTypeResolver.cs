using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Core.Tokenizer;

namespace SimpleCommandParser.Core.TypeResolver
{
    /// <summary>
    /// Провайдер типа модели.
    /// </summary>
    public interface ICommandTypeResolver
    {
        /// <summary>
        /// Выполняет определение типа модели для инициализации.
        /// </summary>
        /// <param name="request">Запрос разбора команды.</param>
        /// <param name="command">Распознанная команда.</param>
        /// <returns>Результат определения типа.</returns>
        CommandTypeResolutionResult Resolve(MultipleCommandParseRequest request, TokenizedCommand command);
    }
}