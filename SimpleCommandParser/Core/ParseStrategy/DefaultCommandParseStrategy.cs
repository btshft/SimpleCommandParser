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
        private readonly Lazy<ICommandTokenizer> _untypedCommandCreatorProvider;
        private readonly Lazy<ICommandModelTypeResolver> _typeResolverProvider;
        private readonly Lazy<ICommandInitializer> _modelInitializerProvider;
        
        /// <summary>
        /// Настройки парсера.
        /// </summary>
        protected ICommandParserSettings Settings { get; }
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="DefaultCommandParseStrategy"/>.
        /// </summary>
        /// <param name="settings">Провайдер настроек.</param>
        protected internal DefaultCommandParseStrategy(ICommandParserSettings settings)
        {
            Settings = settings;
            _untypedCommandCreatorProvider = new Lazy<ICommandTokenizer>(() => new DefaultCommandTokenizer(settings));
            _typeResolverProvider = new Lazy<ICommandModelTypeResolver>(() => new VerbAttributeBasedCommandModelTypeResolver(settings));
            _modelInitializerProvider = new Lazy<ICommandInitializer>(() => new ArgumentAttributeBasedCommandInitializer(settings));
        }

        /// <inheritdoc />
        public virtual ICommandParseResult<TModel> ParseCommand<TModel>(SingleCommandParseRequest<TModel> request)
            where TModel : class, new()
        {
            try
            {
                var untypedCommandCreator = CreateTokenizer(request);
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

                var initializer = CreateInitializer(initializationRequest);
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
        public ICommandParseResult<object> ParseCommands(MultipleCommandParseRequest request)
        {
            try
            {
                var untypedCommandCreator = CreateTokenizer(request);
                if (untypedCommandCreator == null)
                    throw new CommandParserException($"Не удалось получить парсер команд для запроса {request}");
                
                var tokenizeResult = untypedCommandCreator.Tokenize(request);
                if (!tokenizeResult.IsSucceed)
                {
                    return new UnparsedCommand<object>(
                        new CommandParseError(tokenizeResult.ErrorsText, tokenizeResult.ErrorCode));
                }

                var typeResolver = CreateTypeResolver(request, tokenizeResult.Command);
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

                var initializer = CreateInitializer(initializationRequest);
                var initializationResult = initializer.Initialize(initializationRequest);
                if (!initializationResult.IsSucceed)
                {
                    return new UnparsedCommand<object>(
                        new CommandParseError(initializationResult.ErrorsText, initializationResult.ErrorCode));
                }

                return new ParsedCommand<object>(initializationResult.InitializedCommand);
                
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
        
        /// <summary>
        /// Создает экземпляр компонента для выделения токенов из команды.
        /// </summary>
        /// <returns>Экземпляр компонента.</returns>
        protected virtual ICommandTokenizer CreateTokenizer(ICommandParseRequest request)
        {
            return _untypedCommandCreatorProvider.Value;
        }

        /// <summary>
        /// Создает экземпляр инициализатора модели.
        /// </summary>
        /// <param name="request">Запрос на выполнение инициализации модели команды.</param>
        /// <returns>Экземпляр инициалиатора.</returns>
        protected virtual ICommandInitializer CreateInitializer(ICommandInitializationRequest request)
        {
            return _modelInitializerProvider.Value;
        }

        /// <summary>
        /// Создает резолвер для типа модели команды.
        /// </summary>
        /// <param name="request">Запрос разбора команды.</param>
        /// <param name="command">Команда.</param>
        /// <returns>Резолвер типа команды.</returns>
        protected virtual ICommandModelTypeResolver CreateTypeResolver(
            MultipleCommandParseRequest request, TokenizedCommand command)
        {
            return _typeResolverProvider.Value;
        }
    }
}