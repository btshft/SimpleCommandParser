using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Initializer;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Core.TypeResolver;
using SimpleCommandParser.Exceptions;

namespace SimpleCommandParser.Core.ParseStrategy
{
    /// <summary>
    /// Стратегия разбора команды по умолчанию.
    /// </summary>
    public class DefaultCommandParseStrategy : ICommandParseStrategy
    {
        internal class Defaults
        {
            /// <summary>
            /// Провайдер токенизатора команды.
            /// </summary>
            public Lazy<ICommandTokenizer> TokenizerContainer { get; }
        
            /// <summary>
            /// Провайдер инициализатора команды.
            /// </summary>
            public Lazy<ICommandInitializer> InitializerContainer { get; }
        
            /// <summary>
            /// Провайдер определителя типа команды.
            /// </summary>
            public Lazy<ICommandTypeResolver> TypeResolverContainer { get;  }

            public Defaults(Func<ICommandParserSettings> settingsProvider)
            {
                TokenizerContainer = new Lazy<ICommandTokenizer>(() => new DefaultCommandTokenizer(settingsProvider));
                InitializerContainer = new Lazy<ICommandInitializer>(() => new ParameterAttributeBasedCommandInitializer(settingsProvider));
                TypeResolverContainer = new Lazy<ICommandTypeResolver>(() => new VerbAttributeBasedCommandTypeResolver(settingsProvider));
            }
        }
        
        /// <summary>
        /// Провайдер настроек.
        /// </summary>
        protected Func<ICommandParserSettings> SettingsProvider { get; set; }
        
        /// <summary>
        /// Провайдер токенизатора команды.
        /// </summary>
        protected Func<ICommandTokenizer> TokenizerProvider { get; set; }
        
        /// <summary>
        /// Провайдер инициализатора команды.
        /// </summary>
        protected Func<ICommandInitializer> InitializerProvider { get; set; }
        
        /// <summary>
        /// Провайдер определителя типа команды.
        /// </summary>
        protected Func<ICommandTypeResolver> TypeResolverProvider { get; set; }
            
        /// <summary>
        /// Инициализирует экземпляр <see cref="DefaultCommandParseStrategy"/>.
        /// </summary>
        /// <param name="settingsProvider">Провайдер настроек.</param>
        /// <param name="tokenizerProvider">Провайдер токенизатора.</param>
        /// <param name="initializerProvider">Провайдер инициализатора.</param>
        /// <param name="typeResolverProvider">Резолвер типа команды.</param>
        protected internal DefaultCommandParseStrategy(
            Func<ICommandParserSettings> settingsProvider,
            Func<ICommandTokenizer> tokenizerProvider,
            Func<ICommandInitializer> initializerProvider,
            Func<ICommandTypeResolver> typeResolverProvider)
        {
            SettingsProvider = settingsProvider;
            TokenizerProvider = tokenizerProvider;
            InitializerProvider = initializerProvider;
            TypeResolverProvider = typeResolverProvider;
        }

        /// <summary>
        /// Конструктор используемый при инициализации парсера по умолчанию.
        /// </summary>
        /// <param name="settingsProvider">Провайдер настроек.</param>
        protected internal DefaultCommandParseStrategy(Func<ICommandParserSettings> settingsProvider)
        {
            var defaults = new Defaults(settingsProvider);
            
            SettingsProvider = settingsProvider;
            TokenizerProvider = () => defaults.TokenizerContainer.Value;
            InitializerProvider = () => defaults.InitializerContainer.Value;
            TypeResolverProvider = () => defaults.TypeResolverContainer.Value;
        }
        
        /// <inheritdoc />
        public virtual ICommandParseResult<TModel> ParseCommand<TModel>(SingleCommandParseRequest<TModel> request)
            where TModel : class, new()
        {
            try
            {
                var untypedCommandCreator = TokenizerProvider();
                if (untypedCommandCreator == null)
                    throw new CommandParserException($"Не удалось получить парсер команд для запроса {request}");
                
                var tokenizeResult = untypedCommandCreator.Tokenize(request);
                if (!tokenizeResult.IsSucceed)
                {
                    return new UnparsedCommand<TModel>(
                        new CommandParseError(tokenizeResult.ErrorsText, tokenizeResult.ErrorCode));
                }

                var initializationRequest = new CommandInitializationRequest(
                    typeof(TModel), new TModel(), tokenizeResult.Command, request);

                var initializer = InitializerProvider();
                var initializationResult = initializer.Initialize(initializationRequest);
                if (!initializationResult.IsSucceed)
                {
                    return new UnparsedCommand<TModel>(
                        new CommandParseError(initializationResult.ErrorsText, initializationResult.ErrorCode));
                }
 
                return new ParsedCommand<TModel>((TModel)initializationResult.InitializedCommand);
            }
            catch (CommandParserException _)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CommandParserException($"Не удалось выполнить разбор команды для запроса {request}", e);
            }
        }

        /// <inheritdoc />
        public virtual ICommandParseResult<object> ParseCommands(MultipleCommandParseRequest request)
        {
            try
            {
                var untypedCommandCreator = TokenizerProvider();
                if (untypedCommandCreator == null)
                    throw new CommandParserException($"Не удалось получить парсер команд для запроса {request}");
                
                var tokenizeResult = untypedCommandCreator.Tokenize(request);
                if (!tokenizeResult.IsSucceed)
                {
                    return new UnparsedCommand<object>(
                        new CommandParseError(tokenizeResult.ErrorsText, tokenizeResult.ErrorCode));
                }

                var typeResolver = TypeResolverProvider();
                if (typeResolver == null)
                    throw new CommandParserException($"Не удалось получить резолвер типа модели для запроса {request}");


                var typeResolution = typeResolver.Resolve(request, tokenizeResult.Command);
                if (!typeResolution.IsSucceed)
                {
                    return new UnparsedCommand<object>(
                        new CommandParseError(typeResolution.ErrorsText, typeResolution.ErrorCode));
                }

                var instance = Activator.CreateInstance(typeResolution.CommandType);
                
                var initializationRequest = new CommandInitializationRequest(
                    typeResolution.CommandType, instance, tokenizeResult.Command, request);

                var initializer = InitializerProvider();
                var initializationResult = initializer.Initialize(initializationRequest);
                if (!initializationResult.IsSucceed)
                {
                    return new UnparsedCommand<object>(
                        new CommandParseError(initializationResult.ErrorsText, initializationResult.ErrorCode));
                }

                return new ParsedCommand<object>(initializationResult.InitializedCommand);
                
            }
            catch (CommandParserException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CommandParserException($"Не удалось выполнить разбор команды для запроса {request}", e);
            }
        }
    }
}