using System;
using System.Collections.Generic;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Core.StageResults
{
    /// <summary>
    /// Результат разбора типа команды.
    /// </summary>
    public class CommandTypeResolutionResult : CommandParseStageResult<Type>
    {
        /// <summary>
        /// Тип команды.
        /// </summary>
        public Type CommandType { get; }
        
        /// <inheritdoc />
        internal CommandTypeResolutionResult(Type result) : base(result)
        {
            CommandType = result;
        }
        
        internal CommandTypeResolutionResult(IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode) 
            : base(errors, errorCode)
        {
            CommandType = null;
        }

        /// <inheritdoc />
        internal CommandTypeResolutionResult(Type result, IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode) 
            : base(result, errors, errorCode)
        {
            CommandType = result;
        }

        /// <summary>
        /// Результат успешного определения типа.
        /// </summary>
        /// <param name="type">Тип модели.</param>
        /// <returns>Результат.</returns>
        public static CommandTypeResolutionResult Resolved(Type type)
        {
            return new CommandTypeResolutionResult(type);
        }

        /// <summary>
        /// Результат неудачного определения типа.
        /// </summary>
        /// <param name="errors">Массив ошибок.</param>
        /// <returns>Результат.</returns>
        public static CommandTypeResolutionResult Failed(params string[] errors)
        {
            return new CommandTypeResolutionResult(errors, CommandParseErrorCode.UnableResolveCommandType);
        }
    }
}