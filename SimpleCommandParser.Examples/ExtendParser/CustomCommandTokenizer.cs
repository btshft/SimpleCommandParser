using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Core.Tokenizer;

namespace SimpleCommandParser.Examples.ExtendParser
{
    public class CustomCommandTokenizer : ICommandTokenizer
    {
        private readonly ICommandTokenizer _sourceTokenizer;
        
        public CustomCommandTokenizer(Func<ICommandParserSettings> settingsProvider)
        {
            _sourceTokenizer = new DefaultCommandTokenizer(settingsProvider);
        }

        public CommandTokenizeStageResult Tokenize(ICommandParseRequest request)
        {
            return _sourceTokenizer.Tokenize(request);
        }
    }
}