using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Атрибут аргумента команды.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Инициализирует атрибут аргумента команды.
        /// </summary>
        /// <param name="key">Ключ аргумента.</param>
        /// <param name="required">Признак обязательности.</param>
        public ArgumentAttribute(string key, bool required)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            
            Key = key;
            Required = required;
        }

        /// <summary>
        /// Инициализирует обязательный атрибут аргумента команды.
        /// </summary>
        /// <param name="key">Ключ аргумента.</param>
        public ArgumentAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            
            Key = key;
            Required = true;
        }
        
        /// <summary>
        /// Ключ аргумента.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// Признак обязательности аргумента.
        /// </summary>
        public bool Required { get; }
    }
}