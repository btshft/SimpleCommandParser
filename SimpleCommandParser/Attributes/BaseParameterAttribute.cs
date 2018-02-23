using System;

namespace SimpleCommandParser.Attributes
{
    /// <summary>
    /// Базовый атрибут параметра.
    /// </summary>
    public abstract class BaseParameterAttribute : Attribute
    {
        /// <summary>
        /// Наименование параметра.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Длинное наименование параметра.
        /// </summary>
        public string LongName { get; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="BaseParameterAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        protected BaseParameterAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = LongName = name;
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="BaseParameterAttribute"/>.
        /// </summary>
        /// <param name="name">Наименование параметра.</param>
        /// <param name="longName">Длинное наименование параметра.</param>
        protected BaseParameterAttribute(string name, string longName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            
            if (string.IsNullOrWhiteSpace(longName))
                throw new ArgumentNullException(nameof(longName));
            
            Name = name;
            LongName = longName;
        }

        /// <summary>
        /// Выполняет проверку эквивалентности имени параметра строке <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Имя для сравнения.</param>
        /// <param name="comparison">Культура сравнения.</param>
        /// <returns>Признак эквивалентности имени или или длинного имени строке <paramref name="name"/>.</returns>
        internal bool NameEquals(string name, StringComparison comparison)
        {
            return string.Equals(name, Name, comparison) || string.Equals(name, LongName, comparison);
        }
    }
}