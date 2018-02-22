using System;
using System.Collections.Generic;

namespace SimpleCommandParser.Core.Tokenizer
{
    /// <summary>
    /// Токенизированное представление команды.
    /// </summary>
    public class TokenizedCommand
    {
        /// <summary>
        /// Глагол (идет после символа начала команды и до разделителя);
        /// </summary>
        public string Verb { get; }

        /// <summary>
        /// Параметры.
        /// </summary>
        public IReadOnlyCollection<ArgumentToken> Arguments { get; }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="TokenizedCommand"/>.
        /// </summary>
        /// <param name="verb">Глагол.</param>
        /// <param name="arguments">Коллекция параметров.</param>
        public TokenizedCommand(string verb, IReadOnlyCollection<ArgumentToken> arguments)
        {
            Verb = verb;
            Arguments = arguments ?? Array.Empty<ArgumentToken>(); 
        }
        
        /// <summary>
        /// Токен параметра команды.
        /// </summary>
        public class ArgumentToken
        {
            /// <summary>
            /// Ключ аргумента.
            /// </summary>
            public string Key { get; }

            /// <summary>
            /// Значение аргумента.
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// Инициализирует экземпляр <see cref="ArgumentToken"/>.
            /// </summary>
            /// <param name="key">Ключ.</param>
            /// <param name="value">Значение.</param>
            public ArgumentToken(string key, string value)
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key));
                
                Key = key;
                Value = value;
            }
        }
    }
    
}