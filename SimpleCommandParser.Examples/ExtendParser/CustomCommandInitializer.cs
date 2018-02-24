using System;
using SimpleCommandParser.Core.Initializer;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;

namespace SimpleCommandParser.Examples.ExtendParser
{
    public class CustomCommandInitializer : ICommandInitializer
    {
        private readonly ICommandInitializer _sourceInitializer;

        public CustomCommandInitializer(Func<ICommandParserSettings> settingsProvider)
        {
            _sourceInitializer = new ParameterAttributeBasedCommandInitializer(settingsProvider);
        }

        public CommandInitializationStageResult Initialize(ICommandInitializationRequest request)
        {
            return _sourceInitializer.Initialize(request);
        }
    }
}