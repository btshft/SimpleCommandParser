using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Core.TypeResolver;

namespace SimpleCommandParser.Examples.ExtendParser
{
    public class CustomCommandTypeResolver : ICommandTypeResolver
    {
        private readonly ICommandTypeResolver _sourceTypeResolver;

        public CustomCommandTypeResolver(Func<ICommandParserSettings> settingsProvider)
        {
            _sourceTypeResolver = new VerbAttributeBasedCommandTypeResolver(settingsProvider);
        }
        
        public CommandTypeResolutionResult Resolve(MultipleCommandParseRequest request, TokenizedCommand command)
        {
            return _sourceTypeResolver.Resolve(request, command);
        }
    }
}