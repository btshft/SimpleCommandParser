using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.ParseStrategy;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Exceptions;

namespace SimpleCommandParser
{
    /// <summary>
    /// Парсер команд.
    /// </summary>
    public class CommandParser : ICommandParser
    {
        /// <summary>
        /// Парсер с настройками по умолчанию.
        /// </summary>
        public static CommandParser Default { get; } = new CommandParser(MutableCommandParserSettings.DefaultSettings);
        
        /// <summary>
        /// Признак того, что парсер был инициализирован.
        /// </summary>
        public bool IsConfigured { get; protected set; }
        
        /// <summary>
        /// Настройки
        /// </summary>
        public ICommandParserSettings Settings => SettingsProvider();

        /// <summary>
        /// Стратегия разбора пакета.
        /// </summary>
        protected ICommandParseStrategy ParseStrategy { get; set; }
        
        /// <summary>
        /// Провайдер настроек.
        /// </summary>
        protected Func<ICommandParserSettings> SettingsProvider { get; set; }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParser"/>.
        /// </summary>
        /// <param name="configuration">Конфигуратор парсера.</param>
        public CommandParser(Action<MutableCommandParserSettings> configuration)
        {
            var settings = new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);     
            configuration(settings);

            SettingsProvider = () => settings;
            ParseStrategy = new DefaultCommandParseStrategy(SettingsProvider);
            IsConfigured = true;
        }

        /// <summary>
        /// Инициализирует не инициализированный экземпляр <see cref="CommandParser"/> с настройками по умолчанию.
        /// </summary>
        public CommandParser()
        {     
            var settings = new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);

            SettingsProvider = () => settings;
            ParseStrategy = new DefaultCommandParseStrategy(SettingsProvider);
            IsConfigured = false;
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParser"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        internal CommandParser(MutableCommandParserSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            
            SettingsProvider = () => settings;
            ParseStrategy = new DefaultCommandParseStrategy(SettingsProvider);
            IsConfigured = false;
        }

        /// <summary>
        /// Инициализирует новый экезмпляр <see cref="CommandParser"/>.
        /// Конструктор для билдера.
        /// </summary>
        /// <param name="settingsProvider">Провайдер настроек.</param>
        /// <param name="parseStrategy"></param>
        internal CommandParser(Func<MutableCommandParserSettings> settingsProvider, ICommandParseStrategy parseStrategy)
        {
            SettingsProvider = settingsProvider;
            ParseStrategy = parseStrategy;
            IsConfigured = true; // Инициализация через билдер
        }
        
        /// <inheritdoc />
        public ICommandParseResult<TModel> ParseCommand<TModel>(string input) where TModel : class, new()
        {
            try
            {
                var request = new SingleCommandParseRequest<TModel>(input);         
                return ParseStrategy.ParseCommand(request);
            }
            catch (Exception e)
            {
                throw new CommandParserException($"Ошибка при разборе команды '{input}' типа '{typeof(TModel)}'", e);
            }
        }

        /// <inheritdoc />
        public ICommandParseResult<object> ParseCommands(string input, Type[] supportedTypes)
        {
            try
            {
                var request = new MultipleCommandParseRequest(input, supportedTypes);
                return ParseStrategy.ParseCommands(request);
            }
            catch (Exception e)
            {
                throw new CommandParserException($"Ошибка при разборе команды '{input}''", e);
            }         
        }

        /// <summary>
        /// Выполняет настройку парсера.
        /// </summary>
        /// <param name="settings">Обработчик настроек.</param>
        public CommandParser Configure(Action<MutableCommandParserSettings> settings)
        {
            if (IsConfigured)
                throw new CommandParserException("Компонент уже был инициализирован ранее");
            
            settings((MutableCommandParserSettings)Settings);
            return this;
        }
    }
}