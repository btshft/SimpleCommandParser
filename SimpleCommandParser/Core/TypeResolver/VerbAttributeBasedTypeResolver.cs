using System;
using System.Linq;
using SimpleCommandParser.Attributes;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Helpers;

namespace SimpleCommandParser.Core.TypeResolver
{
    /// <summary>
    /// Резолвер типа на основе атрибута <see cref="VerbAttribute"/>.
    /// </summary>
    public class VerbAttributeBasedCommandTypeResolver : ICommandTypeResolver
    {
        /// <summary>
        /// Настройки парсера.
        /// </summary>
        protected Func<ICommandParserSettings> SettingsProvider { get; set; }

        /// <summary>
        /// Настройки.
        /// </summary>
        protected ICommandParserSettings Settings => SettingsProvider();
        
        /// <summary>
        /// Иницализирует экземпляр <see cref="VerbAttributeBasedCommandTypeResolver"/>.
        /// </summary>
        /// <param name="settingsProvider">Провайдер настроек.</param>
        public VerbAttributeBasedCommandTypeResolver(Func<ICommandParserSettings> settingsProvider)
        {
            SettingsProvider = settingsProvider;
        }

        /// <inheritdoc />
        public CommandTypeResolutionResult Resolve(MultipleCommandParseRequest request, TokenizedCommand command)
        {
            if (request.ModelTypes == null || request.ModelTypes.Length == 0)
                throw new ArgumentException("Не заданы типы команд", nameof(request.ModelTypes));
            
            var typesWithoutVerb = AttributeHelper.AllTypesWithoutAttribute<VerbAttribute>(request.ModelTypes)
                .Select(t => t.Name)
                .ToArray();

            if (typesWithoutVerb.Length > 0)
            {
                return CommandTypeResolutionResult.Failed(
                    $"Для команд {string.Join(",", typesWithoutVerb)} не задан атрибут '{nameof(VerbAttribute)}'");
            }
            
            var types = AttributeHelper.AllTypesWithAttribute<VerbAttribute>(
                    request.ModelTypes, a => StringComparer.InvariantCulture.Equals(a.Verb, command.Verb))
                .ToArray();

            if (types.Length == 0)
            {
                return CommandTypeResolutionResult.Failed(
                    "Не найдена модель для сопоставления с командой. " +
                    $"Проверьте наличие атрибута {nameof(VerbAttribute)} соответствующего команде '{command.Verb}'");
                
            }

            if (types.Length > 1)
            {
                var possibleTypes = string.Join(",", types.Select(t => t.Name));
                
                return CommandTypeResolutionResult.Failed(
                    $"Для команды '{command.Verb}' найдено несколько возможных моделей сопоставления: {possibleTypes}");
            }

            return CommandTypeResolutionResult.Resolved(types[0]);
        }
    }
}