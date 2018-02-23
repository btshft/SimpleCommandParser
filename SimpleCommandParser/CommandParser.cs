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
        /// Признак того, что парсер был инициализирован.
        /// </summary>
        public bool IsConfigured { get; }
        
        /// <summary>
        /// Парсер с настройками по умолчанию.
        /// </summary>
        public static CommandParser Default { get; } = new CommandParser(MutableCommandParserSettings.DefaultSettings);

        /// <summary>
        /// Настройки
        /// </summary>
        public ICommandParserSettings Settings { get; }

        /// <summary>
        /// Стратегия разбора пакета.
        /// </summary>
        public ICommandParseStrategy ParseStrategy { get; }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParser"/>.
        /// </summary>
        /// <param name="configuration">Конфигуратор парсера.</param>
        public CommandParser(Action<MutableCommandParserSettings> configuration)
        {
            var settings = new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);     
            configuration(settings);

            Settings = settings;
            ParseStrategy = new DefaultCommandParseStrategy(Settings);
            IsConfigured = true;
        }

        /// <summary>
        /// Инициализирует не инициализированный экземпляр <see cref="CommandParser"/> с настройками по умолчанию.
        /// </summary>
        public CommandParser()
        {
            IsConfigured = false;      
            Settings = new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);
            ParseStrategy = new DefaultCommandParseStrategy(Settings);
        }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParser"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        internal CommandParser(MutableCommandParserSettings settings)
        {
            IsConfigured = false;
            Settings = settings;
            ParseStrategy = new DefaultCommandParseStrategy(Settings);
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