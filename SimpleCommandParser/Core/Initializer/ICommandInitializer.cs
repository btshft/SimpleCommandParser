using SimpleCommandParser.Core.StageResults;

namespace SimpleCommandParser.Core.Initializer
{
    /// <summary>
    /// Интерфейс инициализатора для модели команды.
    /// </summary>
    public interface ICommandInitializer
    {
        /// <summary>
        /// Выполняет инициализацию команды.
        /// </summary>
        /// <param name="request">Запрос на инициализацию.</param>
        /// <returns>Результат инициализации.</returns>
        CommandInitializationStageResult Initialize(ICommandInitializationRequest request);
    }
}