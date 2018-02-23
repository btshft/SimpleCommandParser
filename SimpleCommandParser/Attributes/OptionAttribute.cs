using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Параметр флага.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : BaseParameterAttribute
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        /// <param name="longName">Длинное наименование параметра.</param>
        public OptionAttribute(string name, string longName) 
            : base(name, longName)
        {
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="OptionAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        public OptionAttribute(string name) 
            : base(name)
        {
        }
    }
}