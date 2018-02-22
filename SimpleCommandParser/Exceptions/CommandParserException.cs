using System;
using System.Runtime.Serialization;

namespace SimpleCommandParser.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при разборе команды.
    /// </summary>
    [Serializable]
    public class CommandParserException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр исключения <see cref="CommandParserException"/>.
        /// </summary>
        internal CommandParserException()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр исключения <see cref="CommandParserException"/>.
        /// </summary>
        internal CommandParserException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Инициализирует новый экземпляр исключения <see cref="CommandParserException"/>.
        /// </summary>
        internal CommandParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
        /// <summary>
        /// Инициализирует новый экземпляр исключения <see cref="CommandParserException"/>.
        /// </summary>
        internal CommandParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}