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
        public string VerbPrefix { get; set; }

        /// <inheritdoc />
        public string ArgumentPrefix { get; set; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="MutableCommandParserSettings"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        internal MutableCommandParserSettings(ICommandParserSettings settings)
        {
            StringComparsion = settings.StringComparsion;
            VerbPrefix = settings.VerbPrefix;
            ArgumentPrefix = settings.ArgumentPrefix;
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
                ArgumentPrefix = ":"
            };
    }
}