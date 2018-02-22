using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Атрибут глагола команды.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class VerbAttribute : Attribute
    {
        /// <summary>
        /// Глагол команды.
        /// </summary>
        public string Verb { get; }

        /// <summary>
        /// Инициализирует атрибут глагола команды.
        /// </summary>
        /// <param name="verb">Глагол команды.</param>
        public VerbAttribute(string verb)
        {
            if (string.IsNullOrWhiteSpace(verb))
                throw new ArgumentNullException(nameof(verb));
            
            Verb = verb;
        }
    }
}