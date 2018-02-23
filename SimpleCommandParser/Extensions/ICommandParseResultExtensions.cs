using System;
using System.Collections.Generic;
using SimpleCommandParser.Core.Command;

namespace SimpleCommandParser.Extensions
{
    /// <summary>
    /// Набор расширений для типа <see cref="ICommandParseResult{TModel}"/>.
    /// </summary>
    public static class ICommandParseResultExtensions
    {
        /// <summary>
        /// Выполняет действие над моделью команды, если её удалось распарсить.
        /// </summary>
        /// <param name="commandParseResult">Экземпляр команды.</param>
        /// <param name="callback">Действие над результатом.</param>
        /// <param name="finalConsumer">Признак того, что данный обработчик является завершающим.</param>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <returns>Модель команды.</returns>
        public static ICommandParseResult<TResult> WhenParsed<TResult>(this ICommandParseResult<TResult> commandParseResult, 
            Action<TResult> callback, bool finalConsumer = true) 
            where TResult : class, new()
        {
            if (commandParseResult is ParsedCommand<TResult> pc)
            {
                callback(pc.Value);
                return finalConsumer
                    ? new ConsumedCommand<TResult>(commandParseResult)
                    : commandParseResult;
            }

            return commandParseResult;
        }

        /// <summary>
        /// Выполняет действие над моделью команды, если её удалось распарсить.
        /// </summary>
        /// <param name="commandParseResult">Экземпляр команды.</param>
        /// <param name="callback">Действие над результатом.</param>
        /// <param name="finalConsumer">Признак того, что данный обработчик является завершающим.</param>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <returns>Модель команды.</returns>
        public static ICommandParseResult<object> WhenParsed<TResult>(this ICommandParseResult<object> commandParseResult,
            Action<TResult> callback, bool finalConsumer = true)
        {
            if (commandParseResult is ParsedCommand<object> pc && pc.Value is TResult result)
            {
                callback(result);
                return finalConsumer
                    ? new ConsumedCommand<object>(commandParseResult)
                    : commandParseResult;
            }

            return commandParseResult;
        }
        
        /// <summary>
        /// Выполняет действие над моделью команды, если её не удалось распарсить.
        /// </summary>
        /// <param name="commandParseResult">Экземпляр команды.</param>
        /// <param name="callback">Действие над результатом.</param>
        /// <param name="finalConsumer">Признак того, что данный обработчик является завершающим.</param>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <returns>Модель команды.</returns>
        public static ICommandParseResult<TResult> WhenNotParsed<TResult>(this ICommandParseResult<TResult> commandParseResult, 
            Action<IReadOnlyCollection<CommandParseError>> callback, bool finalConsumer = true) 
            where TResult : class, new()
        {
            if (commandParseResult is UnparsedCommand<TResult> uc)
            {
                callback(uc.Errors);
                return finalConsumer
                    ? new ConsumedCommand<TResult>(commandParseResult)
                    : commandParseResult;
            }

            return commandParseResult;
        }
    }
}