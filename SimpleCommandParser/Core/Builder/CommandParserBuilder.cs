using System;
using SimpleCommandParser.Core.Initializer;
using SimpleCommandParser.Core.ParseStrategy;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Core.TypeResolver;

namespace SimpleCommandParser.Core.Builder
{
    /// <summary>
    /// Билдер для парсера команд.
    /// </summary>
    public class CommandParserBuilder
    {
        private Func<MutableCommandParserSettings> _settingsProvider;
        private Func<ICommandTypeResolver> _typeResolverProvider;
        private Func<ICommandTokenizer> _tokenizerProvider;
        private Func<ICommandInitializer> _initializerProvider;
        
        /// <summary>
        /// Создает новый экземпляр <see cref="CommandParserBuilder"/> с компонентами по умолчанию.
        /// </summary>
        public CommandParserBuilder()
        {
            _settingsProvider = () => new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);
            _typeResolverProvider =() => new VerbAttributeBasedCommandTypeResolver(_settingsProvider);
            _tokenizerProvider = () => new DefaultCommandTokenizer(_settingsProvider);
            _initializerProvider =() => new ParameterAttributeBasedCommandInitializer(_settingsProvider);
        }
        
        /// <summary>
        /// Устанавливает компонент для определения типа команды.
        /// </summary>
        /// <param name="resolver">Компонент для определения типа команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="resolver"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTypeResolver(ICommandTypeResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            _typeResolverProvider = () => resolver;
            return this;
        }

        /// <summary>
        /// Устанавливает компонент для определения типа команды.
        /// </summary>
        /// <param name="resolverProvider">Провайдер компонента для определения типа команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="resolverProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTypeResolver(Func<ICommandTypeResolver> resolverProvider)
        {
            if (resolverProvider == null)
                throw new ArgumentNullException(nameof(resolverProvider));

            _typeResolverProvider = resolverProvider;
            return this;
        }
        
        /// <summary>
        /// Устанавливает компонент для определения типа команды.
        /// </summary>
        /// <param name="resolverProvider">Провайдер компонента для определения типа команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="resolverProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTypeResolver(Func<Func<ICommandParserSettings>, ICommandTypeResolver> resolverProvider)
        {
            if (resolverProvider == null)
                throw new ArgumentNullException(nameof(resolverProvider));

            _typeResolverProvider = () => resolverProvider(_settingsProvider);
            return this;
        }

        /// <summary>
        /// Устанавливает компонент для разбора команды на составляющие.
        /// </summary>
        /// <param name="tokenizer">Компонент для разбора команды на составляющие.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="tokenizer"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTokenizer(ICommandTokenizer tokenizer)
        {
            if (tokenizer == null)
                throw new ArgumentNullException(nameof(tokenizer));

            _tokenizerProvider = () => tokenizer;
            return this;
        }
        
        /// <summary>
        /// Устанавливает компонент для разбора команды на составляющие.
        /// </summary>
        /// <param name="tokenizerProvider">Провайдер компонента для разбора команды на составляющие.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="tokenizerProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTokenizer(Func<ICommandTokenizer> tokenizerProvider)
        {
            if (tokenizerProvider == null)
                throw new ArgumentNullException(nameof(tokenizerProvider));

            _tokenizerProvider = tokenizerProvider;
            return this;
        }

        /// <summary>
        /// Устанавливает компонент для разбора команды на составляющие.
        /// </summary>
        /// <param name="tokenizerProvider">Провайдер компонента для разбора команды на составляющие.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="tokenizerProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithTokenizer(Func<Func<ICommandParserSettings>, ICommandTokenizer> tokenizerProvider)
        {
            if (tokenizerProvider == null)
                throw new ArgumentNullException(nameof(tokenizerProvider));

            _tokenizerProvider = () => tokenizerProvider(_settingsProvider);
            return this;
        }
        
        /// <summary>
        /// Устанавливает компонент для инициализации экемпляра модели команды.
        /// </summary>
        /// <param name="initializer">Компонента для инициализации модели команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="initializer"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithInitializer(ICommandInitializer initializer)
        {
            if (initializer == null)
                throw new ArgumentNullException(nameof(initializer));

            _initializerProvider = () => initializer;
            return this;
        }

        /// <summary>
        /// Устанавливает компонент для инициализации модели команды.
        /// </summary>
        /// <param name="initializerProvider">Провайдер компонента инициализации модели команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="initializerProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithInitializer(Func<ICommandInitializer> initializerProvider)
        {
            if (initializerProvider == null)
                throw new ArgumentNullException(nameof(initializerProvider));

            _initializerProvider = initializerProvider;
            return this;
        }
        
        /// <summary>
        /// Устанавливает компонент для инициализации модели команды.
        /// </summary>
        /// <param name="initializerProvider">Провайдер компонента инициализации модели команды.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="initializerProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithInitializer(Func<Func<ICommandParserSettings>, ICommandInitializer> initializerProvider)
        {
            if (initializerProvider == null)
                throw new ArgumentNullException(nameof(initializerProvider));

            _initializerProvider = () => initializerProvider(_settingsProvider);
            return this;
        }
        
        /// <summary>
        /// Устанавливает конфигуратор настроек парсера. В качестве параметра конфигуратору передаются настройки по умолчанию.
        /// </summary>
        /// <param name="configurer">Конфигуратор настроек.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="configurer"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithSettings(Action<MutableCommandParserSettings> configurer)
        {
            if (configurer == null)
                throw new ArgumentNullException(nameof(configurer));

            _settingsProvider = () =>
            {
                var settings = new MutableCommandParserSettings(MutableCommandParserSettings.DefaultSettings);
                configurer(settings);
                return settings;
            };
            
            return this;
        }

        /// <summary>
        /// Устанавливает настройки парсера.
        /// </summary>
        /// <param name="settings">Настройки парсера.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="settings"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithSettings(MutableCommandParserSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _settingsProvider = () => settings;
            return this;
        }

        /// <summary>
        /// Устанавливает провайдер настроек парсера.
        /// </summary>
        /// <param name="settingsProvider">Провайдер настроек парсера.</param>
        /// <returns>Экземпляр билдера.</returns>
        /// <exception cref="ArgumentNullException">Если <paramref name="settingsProvider"/> равен <c>null</c>.</exception>
        public CommandParserBuilder WithSettings(Func<MutableCommandParserSettings> settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settingsProvider = settingsProvider;
            
            return this;
        }

        /// <summary>
        /// Создает экземпляр парсера с указанными шагами.
        /// </summary>
        /// <returns>Экземпляр парсера.</returns>
        public CommandParser Build()
        {
            var strategy = new DefaultCommandParseStrategy(_settingsProvider, _tokenizerProvider, _initializerProvider, _typeResolverProvider);
            return new CommandParser(_settingsProvider, strategy);
        }
    }
}