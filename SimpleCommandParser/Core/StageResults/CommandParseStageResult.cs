using System;
using System.Collections.Generic;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Core.StageResults
{
    /// <summary>
    /// Результат выполнения этапа разбора команды.
    /// </summary>
    public class CommandParseStageResult : ICommandParseStageResult
    {
        /// <summary>
        /// Успешный результат выполнения.
        /// </summary>
        public static CommandParseStageResult Succeed { get; } = new CommandParseStageResult();
        
        /// <inheritdoc />
        public IReadOnlyCollection<string> Errors { get; }

        /// <inheritdoc />
        public string ErrorsText => Errors.Count > 0 ? string.Join(";", Errors) : string.Empty;
        
        /// <inheritdoc />
        public CommandParseErrorCode? ErrorCode { get; }

        /// <inheritdoc />
        public bool IsSucceed => !(ErrorCode.HasValue || Errors.Count > 0);

        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseStageResult"/>.
        /// </summary>
        internal CommandParseStageResult()
        {
            Errors = Array.Empty<string>();
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseStageResult"/>.
        /// </summary>
        /// <param name="errors">Коллекция ошибок.</param>
        /// <param name="errorCode">Код ошибки.</param>
        internal CommandParseStageResult(IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode)
        {
            Errors = errors ?? Array.Empty<string>();
            ErrorCode = errorCode;
        }
   
        /// <summary>
        /// Инициализирует результат из ошибок.
        /// </summary>
        /// <param name="errorCode">Код ошибки.</param>
        /// <param name="errors">Перечень ошибок.</param>
        /// <returns>Результат обработки.</returns>
        public CommandParseStageResult FromErrors(CommandParseErrorCode errorCode, params string[] errors)
        {
            return new CommandParseStageResult(errors, errorCode);
        }

        /// <summary>
        /// Инициализирует результат со значением.
        /// </summary>
        /// <param name="result">Значение.</param>
        /// <typeparam name="TResult">Тип значения.</typeparam>
        /// <returns>Результат обработки.</returns>
        public CommandParseStageResult<TResult> FromResult<TResult>(TResult result)
        {
            return new CommandParseStageResult<TResult>(result);
        }
    }
    
    /// <summary>
    /// Результат выполнения этапа разбора команды.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public class CommandParseStageResult<TResult> : ICommandParseStageResult<TResult>
    {
        /// <inheritdoc />
        public IReadOnlyCollection<string> Errors { get; }

        /// <inheritdoc />
        public string ErrorsText => Errors.Count > 0 ? string.Join(";", Errors) : string.Empty;

        /// <inheritdoc />
        public CommandParseErrorCode? ErrorCode { get; }

        /// <inheritdoc />
        public bool IsSucceed => !(ErrorCode.HasValue || Errors.Count > 0);

        /// <inheritdoc />
        public TResult Result { get; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseStageResult"/>.
        /// </summary>
        /// <param name="result">Значение.</param>
        internal CommandParseStageResult(TResult result)
        {
            Result = result;
            Errors =  Array.Empty<string>();
            ErrorCode = null;
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseStageResult"/>.
        /// </summary>
        /// <param name="errors">Коллекция ошибок.</param>
        /// <param name="errorCode">Код ошибки.</param>
        internal CommandParseStageResult(IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode)
        {
            Result = default(TResult);
            Errors = errors ?? Array.Empty<string>();
            ErrorCode = errorCode;
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseStageResult"/>.
        /// </summary>
        /// <param name="result">Значение.</param>
        /// <param name="errors">Коллекция ошибок.</param>
        /// <param name="errorCode">Код ошибки.</param>
        internal CommandParseStageResult(TResult result, IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode)
        {
            Result = result;
            Errors = errors ?? Array.Empty<string>();
            ErrorCode = errorCode;
        }
       
        /// <summary>
        /// Преобразует контейнер с результатом к контейнеру без результата.
        /// </summary>
        /// <param name="result">Контейнер с результатом.</param>
        /// <returns>Контейнер без результата.</returns>
        public static implicit operator CommandParseStageResult(CommandParseStageResult<TResult> result)
        {
            return new CommandParseStageResult(result.Errors, result.ErrorCode);
        }

        /// <summary>
        /// Преобразует контейнер без результата к контейнеру с результатом по умолчанию.
        /// </summary>
        /// <param name="result">Контейнер без результата.</param>
        /// <returns>Контейнер с результатом по умолчанию.</returns>
        public static implicit operator CommandParseStageResult<TResult>(CommandParseStageResult result)
        {
            return new CommandParseStageResult<TResult>(default(TResult), result.Errors, result.ErrorCode);
        }
    }
}