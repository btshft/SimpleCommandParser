using System.Collections.Generic;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Core.StageResults
{
    /// <summary>
    /// Результат выполнения этапа разбора команды.
    /// </summary>
    public interface ICommandParseStageResult
    {
        /// <summary>
        /// Список ошибок.
        /// </summary>
        IReadOnlyCollection<string> Errors { get; }
       
        /// <summary>
        /// Текст ошибки.
        /// </summary>
        string ErrorsText { get; }
        
        /// <summary>
        /// Код ошибки.
        /// </summary>
        CommandParseErrorCode? ErrorCode { get; }
        
        /// <summary>
        /// Признак успешности выполнения этапа.
        /// </summary>
        bool IsSucceed { get; }
    }

    /// <summary>
    /// Результат выполнения этапа разбора команды.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public interface ICommandParseStageResult<out TResult> : ICommandParseStageResult
    {
        /// <summary>
        /// Результат выполнения этапа.
        /// </summary>
        TResult Result { get; }
    }
}