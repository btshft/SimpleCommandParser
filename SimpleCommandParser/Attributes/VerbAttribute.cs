using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Атрибут действия команды.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class VerbAttribute : Attribute
    {
        /// <summary>
        /// Действие команды.
        /// </summary>
        public string Verb { get; }

        /// <summary>
        /// Инициализирует атрибут действия команды.
        /// </summary>
        /// <param name="verb">Действие команды.</param>
        public VerbAttribute(string verb)
        {
            if (string.IsNullOrWhiteSpace(verb))
                throw new ArgumentNullException(nameof(verb));
            
            Verb = verb;
        }
    }
}