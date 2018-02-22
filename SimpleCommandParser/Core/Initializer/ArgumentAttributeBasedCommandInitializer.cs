using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleCommandParser.Attributes;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Exceptions;
using SimpleCommandParser.Helpers;

namespace SimpleCommandParser.Core.Initializer
{
    /// <summary>
    /// Выполняет инициализацию свойств на атрибута <see cref="ArgumentAttribute"/>.
    /// </summary>
    public class ArgumentAttributeBasedCommandInitializer : ICommandInitializer
    {
        /// <summary>
        /// Настройки парсера.
        /// </summary>
        protected ICommandParserSettings Settings { get; }

        /// <summary>
        /// Иницализирует экземпляр <see cref="ArgumentAttributeBasedCommandInitializer"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        protected internal ArgumentAttributeBasedCommandInitializer(ICommandParserSettings settings)
        {
            Settings = settings;
        }

        /// <inheritdoc />
        public CommandInitializationStageResult Initialize(ICommandInitializationRequest request)
        {
            var expectedArgs = AttributeHelper.PropertiesWithAttribute<ArgumentAttribute>(request.CommandType);
            if (expectedArgs.Length <= 0) 
                return CommandInitializationStageResult.Initialized(request.CommandInstance);
            
            var errors = new Dictionary<string, string>();
            var exceptions = new List<Exception>();

            foreach (var requiredExpectedArgument in expectedArgs)
            {
                var mathedFactualArgument = request.TokenizedCommand.Arguments
                    .FirstOrDefault(e => string.Equals(e.Key, requiredExpectedArgument.Attribute.Key,
                        Settings.StringComparsion));

                var requiredExpectedArgumentProperty = requiredExpectedArgument.PropertyInfo;

                if (mathedFactualArgument == null && requiredExpectedArgument.Attribute.Required)
                {
                    errors.Add(requiredExpectedArgument.Attribute.Key, "Параметр не задан");
                }
                else if (mathedFactualArgument != null)
                {
                    var converter = TypeDescriptor.GetConverter(requiredExpectedArgumentProperty.PropertyType);
                    if (!converter.CanConvertFrom(typeof(string)))
                    {
                        errors.Add(requiredExpectedArgument.Attribute.Key,
                            $"Невозможно привести к типу '{requiredExpectedArgumentProperty.PropertyType.Name}' " +
                            $"свойства '{requiredExpectedArgumentProperty.Name}'");

                        continue;
                    }

                    if (!requiredExpectedArgumentProperty.CanWrite)
                    {
                        errors.Add(requiredExpectedArgument.Attribute.Key,
                            $"Невозможно установить свойство '{requiredExpectedArgumentProperty.Name}' " +
                            $"модели '{request.CommandType.Name}'");

                        continue;
                    }

                    try
                    {
                        var convertedValue = converter.ConvertFromInvariantString(mathedFactualArgument.Value);
                        requiredExpectedArgument.PropertyInfo.SetValue(request.CommandInstance, convertedValue);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(new CommandParserException(
                            $"Исключение при установке свойства '{requiredExpectedArgumentProperty.Name}' " +
                            $"модели '{request.CommandType.Name} значением '{mathedFactualArgument.Value}'", e));
                    }
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);

            return errors.Count > 0
                ? CommandInitializationStageResult.Failed(FlattenErrors(errors))
                : CommandInitializationStageResult.Initialized(request.CommandInstance);
        }

        private string[] FlattenErrors(IDictionary<string, string> errors)
        {
            return errors.Select(error => $"Ошибка обработки параметра команды '{error.Key}': {error.Value}").ToArray();
        }
    }
}