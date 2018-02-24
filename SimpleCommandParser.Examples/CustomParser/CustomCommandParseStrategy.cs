using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Initializer;
using SimpleCommandParser.Core.ParseStrategy;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Core.TypeResolver;

namespace SimpleCommandParser.Examples.CustomParser
{
    public class CustomCommandParseStrategy : DefaultCommandParseStrategy
    {
        protected internal CustomCommandParseStrategy(
            Func<ICommandParserSettings> settingsProvider, 
            Func<ICommandTokenizer> tokenizerProvider, 
            Func<ICommandInitializer> initializerProvider, 
            Func<ICommandTypeResolver> typeResolverProvider) 
            : base(settingsProvider, tokenizerProvider, initializerProvider, typeResolverProvider)
        {
        }

        protected internal CustomCommandParseStrategy(Func<ICommandParserSettings> settingsProvider) 
            : base(settingsProvider)
        {
        }

        public override ICommandParseResult<TModel> ParseCommand<TModel>(SingleCommandParseRequest<TModel> request)
        {
            return base.ParseCommand(request);
        }

        public override ICommandParseResult<object> ParseCommands(MultipleCommandParseRequest request)
        {
            return base.ParseCommands(request);
        }
    }
}