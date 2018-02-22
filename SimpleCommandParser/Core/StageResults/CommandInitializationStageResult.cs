using System.Collections.Generic;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Core.StageResults
{
    /// <summary>
    /// Результат инициализации модели.
    /// </summary>
    public class CommandInitializationStageResult : CommandParseStageResult<object>
    {
        /// <summary>
        /// Инициализированная сущность команды.
        /// </summary>
        public object InitializedCommand { get; }
        
        /// <inheritdoc />
        internal CommandInitializationStageResult(object result) : base(result)
        {
            InitializedCommand = result;
        }

        /// <inheritdoc />
        internal CommandInitializationStageResult(IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode) 
            : base(errors, errorCode)
        {
            InitializedCommand = null;
        }

        /// <summary>
        /// Результат успешной иницициализации команды.
        /// </summary>
        /// <param name="instance">Экезмпляр команды.</param>
        /// <returns>Результат.</returns>
        public static CommandInitializationStageResult Initialized(object instance)
        {
            return new CommandInitializationStageResult(instance);
        }

        /// <summary>
        /// Результат неудачной иницициализации команды.
        /// </summary>
        /// <param name="errors">Массив ошибок.</param>
        /// <returns>Результат.</returns>
        public static CommandInitializationStageResult Failed(params string[] errors)
        {
            return new CommandInitializationStageResult(errors, CommandParseErrorCode.CommandInitializationFailed);
        }
    }
}