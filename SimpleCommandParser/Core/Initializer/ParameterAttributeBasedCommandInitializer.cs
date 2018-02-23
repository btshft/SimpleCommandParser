using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleCommandParser.Attributes;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Core.Tokenizer;
using SimpleCommandParser.Exceptions;
using SimpleCommandParser.Helpers;

namespace SimpleCommandParser.Core.Initializer
{
    /// <summary>
    /// Выполняет инициализацию свойств на атрибута <see cref="ParameterAttribute"/>.
    /// </summary>
    public class ParameterAttributeBasedCommandInitializer : ICommandInitializer
    {
        /// <summary>
        /// Настройки парсера.
        /// </summary>
        protected ICommandParserSettings Settings { get; }

        /// <summary>
        /// Иницализирует экземпляр <see cref="ParameterAttributeBasedCommandInitializer"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        protected internal ParameterAttributeBasedCommandInitializer(ICommandParserSettings settings)
        {
            Settings = settings;
        }

        /// <inheritdoc />
        public CommandInitializationStageResult Initialize(ICommandInitializationRequest request)
        {
            var tokenizedCommand = request.TokenizedCommand;
            var parameters = AttributeHelper.PropertiesWithAttribute<ParameterAttribute>(request.CommandType);
            var options = AttributeHelper.PropertiesWithAttribute<OptionAttribute>(request.CommandType);

            var invalidOptions = options
                .Where(o => !o.PropertyInfo.PropertyType.IsAssignableFrom(typeof(bool)))
                .Select(o => o.PropertyInfo)
                .ToArray();

            if (invalidOptions.Length > 0)
            {
                var invalidOptionNames = string.Join(",", invalidOptions.Select(p => p.Name));
 
                return CommandInitializationStageResult.Failed(
                    $"Использование атрибута [{nameof(OptionAttribute)}] " +
                    $"допускается только на свойствах, которые могут быть преобразованы из типа {typeof(bool)}. " +
                    $"Некорректно заданы атрибута на свойствах {invalidOptionNames} класса '{request.CommandType.Name}'");
            }

            var errors = new Dictionary<string, string>();
            var exceptions = new List<Exception>();

            if (!tokenizedCommand.HasOnlyArgumentValues)
            {
                MapKeyedOptions(request, options, tokenizedCommand, errors, exceptions);
                
                foreach (var parameter in parameters)
                {
                    var actualArgument = tokenizedCommand.Arguments
                        .FirstOrDefault(a => parameter.Attribute.NameEquals(a.Key, Settings.StringComparsion));

                    MapParameter(request, parameter, actualArgument, errors, exceptions);
                }
            }
            else
            {
                if (Settings.RequireArgumentKeyPrefix)
                    return CommandInitializationStageResult.Failed(
                        "Указание наименований параметров ялвяется обязательным. " +
                        "Для использования мапинга параметров на поля модели в соответствии с порядком атрибутом " +
                        $"необходимо отключить настройку '{nameof(ICommandParserSettings.RequireArgumentKeyPrefix)}'");
                
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var actualArgument = tokenizedCommand.Arguments.ElementAtOrDefault(i);

                    MapParameter(request, parameter, actualArgument, errors, exceptions);
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);

            return errors.Count > 0
                ? CommandInitializationStageResult.Failed(FlattenErrors(errors))
                : CommandInitializationStageResult.Initialized(request.CommandInstance);
        }

        private void MapKeyedOptions(
            ICommandInitializationRequest request, 
            IEnumerable<AttributeHelper.AttributeExtendedPropertyInfo<OptionAttribute>> options,
            TokenizedCommand tokenizedCommand, 
            IDictionary<string, string> errors, 
            ICollection<Exception> exceptions)
        {
            foreach (var option in options)
            {
                if (!tokenizedCommand.Arguments.Any(a => option.Attribute.NameEquals(a.Key, Settings.StringComparsion)))
                    continue;

                if (!option.PropertyInfo.CanWrite)
                {
                    errors.Add(option.Attribute.LongName,
                        $"Невозможно установить свойство '{option.PropertyInfo.Name}' " +
                        $"модели '{request.CommandType.Name}'");

                    continue;
                }

                try
                {
                    option.PropertyInfo.SetValue(request.CommandInstance, true);
                }
                catch (Exception e)
                {
                    exceptions.Add(
                        new CommandParserException($"Исключение при установке свойства '{option.PropertyInfo.Name}'", e));
                }
            }
        }
        
        private static void MapParameter(
            ICommandInitializationRequest request, 
            AttributeHelper.AttributeExtendedPropertyInfo<ParameterAttribute> parameter,
            TokenizedCommand.ArgumentToken actualArgument, 
            IDictionary<string, string> errors, 
            ICollection<Exception> exceptions)
        {
            if (parameter.Attribute.Required && string.IsNullOrEmpty(actualArgument?.Value))
            {
                errors.Add(parameter.Attribute.LongName, "Параметр не задан");
                return;
            }

            if (actualArgument == null)
            {
                return;
            }

            var converter = TypeDescriptor.GetConverter(parameter.PropertyInfo.PropertyType);
            if (!converter.CanConvertFrom(typeof(string)))
            {
                errors.Add(parameter.Attribute.LongName,
                    $"Невозможно привести значение параметра к типу '{parameter.PropertyInfo.PropertyType.Name}' " +
                    $"свойства '{parameter.PropertyInfo.Name}. Значение: {actualArgument.Value}'");

                return;
            }

            if (!parameter.PropertyInfo.CanWrite)
            {
                errors.Add(parameter.Attribute.LongName,
                    $"Невозможно установить свойство '{parameter.PropertyInfo.Name}' " +
                    $"модели '{request.CommandType.Name}'");

                return;
            }

            try
            {
                var convertedValue = converter.ConvertFromInvariantString(actualArgument.Value);
                parameter.PropertyInfo.SetValue(request.CommandInstance, convertedValue);
            }
            catch (Exception e)
            {
                exceptions.Add(new CommandParserException(
                    $"Исключение при установке свойства '{parameter.PropertyInfo.Name}' " +
                    $"модели '{request.CommandType.Name} значением '{actualArgument.Value}'", e));
            }
        }

        private string[] FlattenErrors(IDictionary<string, string> errors)
        {
            return errors.Select(error => $"Ошибка обработки параметра команды '{error.Key}': {error.Value}").ToArray();
        }
    }
}