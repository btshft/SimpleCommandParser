using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Атрибут параметра команды.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ParameterAttribute : BaseParameterAttribute
    {
        /// <summary>
        /// Признак обязательности параметра.
        /// </summary>
        public bool Required { get; set; }
        
        /// <summary>
        /// Позиция значения параметра при вызове команды.
        /// Используется при сокращенном вызове команды без указания наименований параметров.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="ParameterAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        /// <param name="longName">Длинное наименование параметра.</param>
        public ParameterAttribute(string name, string longName) 
            : base(name, longName)
        {
            Order = null;
            Required = true;
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="ParameterAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        public ParameterAttribute(string name) 
            : base(name)
        {
            Order = null;
            Required = true;
        }
    }
}