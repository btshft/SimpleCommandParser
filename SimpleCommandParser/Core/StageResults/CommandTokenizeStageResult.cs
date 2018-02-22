using System.Collections.Generic;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Tokenizer;

namespace SimpleCommandParser.Core.StageResults
{
    /// <summary>
    /// Результат разбора команды на токены.
    /// </summary>
    public class CommandTokenizeStageResult : CommandParseStageResult<TokenizedCommand>
    {
        /// <summary>
        /// Команда в виде токенов.
        /// </summary>
        public TokenizedCommand Command { get; }
        
        /// <inheritdoc />
        protected CommandTokenizeStageResult(TokenizedCommand result) : base(result)
        {
            Command = result;
        }

        /// <inheritdoc />
        protected CommandTokenizeStageResult(IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode) 
            : base(errors, errorCode)
        {
            Command = null;
        }

        /// <inheritdoc />
        protected CommandTokenizeStageResult(TokenizedCommand result, IReadOnlyCollection<string> errors, CommandParseErrorCode? errorCode) 
            : base(result, errors, errorCode)
        {
            Command = result;
        }
        
        /// <summary>
        /// Результат успешного разбора на токены.
        /// </summary>
        /// <param name="command">Модель команды.</param>
        /// <returns>Результат.</returns>
        public static CommandTokenizeStageResult Tokenized(TokenizedCommand command)
        {
            return new CommandTokenizeStageResult(command);
        }

        /// <summary>
        /// Результат неудачного разбора на токены.
        /// </summary>
        /// <param name="errors">Массив ошибок.</param>
        /// <returns>Результат.</returns>
        public static CommandTokenizeStageResult Failed(params string[] errors)
        {
            return new CommandTokenizeStageResult(errors, CommandParseErrorCode.BrokenInput);
        }
    }
}