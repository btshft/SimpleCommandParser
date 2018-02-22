using System;

namespace SimpleCommandParser.Core.Settings
{
    /// <summary>
    /// Модель настроек парсера.
    /// </summary>
    public class MutableCommandParserSettings : ICommandParserSettings
    {
        /// <inheritdoc />
        public StringComparison StringComparsion { get; set; }

        /// <inheritdoc />
        public string CommandVerbPrefix { get; set; }

        /// <inheritdoc />
        public char? CommandArgumentKeyValueDelimeter { get; set; }

        /// <inheritdoc />
        public char? CommandArgumentKeyPrefix { get; set; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="MutableCommandParserSettings"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        internal MutableCommandParserSettings(ICommandParserSettings settings)
        {
            StringComparsion = settings.StringComparsion;
            CommandVerbPrefix = settings.CommandVerbPrefix;
            CommandArgumentKeyValueDelimeter = settings.CommandArgumentKeyValueDelimeter;
            CommandArgumentKeyPrefix = settings.CommandArgumentKeyPrefix;
        }
        
        /// <summary>
        /// Инициализирует настройки по умолчанию.
        /// </summary>
        public MutableCommandParserSettings()  { }
        
        /// <summary>
        /// Настройки по умолчанию.
        /// </summary>
        internal static MutableCommandParserSettings DefaultSettings { get; }
            = new MutableCommandParserSettings
            {
                StringComparsion = StringComparison.InvariantCultureIgnoreCase,
                CommandArgumentKeyPrefix = '-'
            };
    }
}