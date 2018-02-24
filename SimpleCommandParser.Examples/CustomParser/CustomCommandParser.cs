using System;
using SimpleCommandParser.Core.Settings;

namespace SimpleCommandParser.Examples.CustomParser
{
    public class CustomCommandParser : CommandParser
    {
        public CustomCommandParser(Action<MutableCommandParserSettings> configuration)
            : base(configuration)
        {
            
        }

        public CustomCommandParser()
        {
            ParseStrategy = new CustomCommandParseStrategy(SettingsProvider);
        }
    }
}