using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Признак наличия параметров.
        /// </summary>
        public bool HasArguments => Arguments.Count > 0;

        /// <summary>
        /// Признак того, что в параметрах заданы только ключи.
        /// </summary>
        public bool HasOnlyArgumentValues => Arguments.All(a => string.IsNullOrEmpty(a.Key));
        
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
            /// Признак того, что аргумент - опция.
            /// </summary>
            public bool IsOption => string.IsNullOrEmpty(Value) && !string.IsNullOrEmpty(Key);
            
            /// <summary>
            /// Инициализирует экземпляр <see cref="ArgumentToken"/>.
            /// </summary>
            /// <param name="key">Ключ.</param>
            /// <param name="value">Значение.</param>
            public ArgumentToken(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
    }
    
}