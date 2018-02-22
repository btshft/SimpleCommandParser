using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Tokenizer;

namespace SimpleCommandParser.Core.Initializer
{
    /// <summary>
    /// Запрос на инициализацию модели.
    /// </summary>
    public interface ICommandInitializationRequest
    {
        /// <summary>
        /// Тип модели.
        /// </summary>
        Type CommandType { get; }
        
        /// <summary>
        /// Экземпляр модели для заполнения.
        /// </summary>
        object CommandInstance { get; }
        
        /// <summary>
        /// Распознанная команда.
        /// </summary>
        TokenizedCommand TokenizedCommand { get; }
        
        /// <summary>
        /// Запрос на разбор команды.
        /// </summary>
        ICommandParseRequest ParseRequest { get; }
    }
    
    /// <summary>
    /// Модель запроса на инициализацию модели.
    /// </summary>
    internal class CommandInitializationRequest : ICommandInitializationRequest
    {
        /// <inheritdoc />
        public Type CommandType { get; }

        /// <inheritdoc />
        public object CommandInstance { get; }

        /// <inheritdoc />
        public TokenizedCommand TokenizedCommand { get; }

        /// <inheritdoc />
        public ICommandParseRequest ParseRequest { get; }

        public CommandInitializationRequest(
            Type commandType, object commandInstance, TokenizedCommand tokenizedCommand, ICommandParseRequest parseRequest)
        {
            CommandType = commandType;
            CommandInstance = commandInstance;
            TokenizedCommand = tokenizedCommand;
            ParseRequest = parseRequest;
        }
    }
}