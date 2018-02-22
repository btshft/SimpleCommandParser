using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.StageResults;

namespace SimpleCommandParser.Core.Tokenizer
{
    /// <summary>
    /// Интерфейс компонента для выделения токенов из команды.
    /// </summary>
    public interface ICommandTokenizer
    {
        /// <summary>
        /// Выполняет разбор команды на токены.
        /// </summary>
        /// <param name="request">Модель запроса.</param>
        /// <returns>Результат разбора команды..</returns>
        CommandTokenizeStageResult Tokenize(ICommandParseRequest request);
    }
}