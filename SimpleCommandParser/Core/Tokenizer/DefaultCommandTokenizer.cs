using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Exceptions;

namespace SimpleCommandParser.Core.Tokenizer
{
    /// <summary>
    /// Реализация нетипизированного парсера команд.
    /// </summary>
    public class DefaultCommandTokenizer : ICommandTokenizer
    {      
        /// <summary>
        /// Регулярное выражение для нормализации строки.
        /// </summary>
        protected Regex StringNormalizationRegex { get; set; }
        
        /// <summary>
        /// Провайдер настроек парсера.
        /// </summary>
        protected ICommandParserSettings Settings { get; }

        protected internal DefaultCommandTokenizer(ICommandParserSettings settings)
        {           
            Settings = settings;
            StringNormalizationRegex = new Regex(@"\s+", RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public CommandTokenizeStageResult Tokenize(ICommandParseRequest request)
        {
            EnsureSettingsValid(Settings);
            
            if (string.IsNullOrWhiteSpace(request.Input))
                return CommandTokenizeStageResult.Failed("Команда не задана");

            var normalizedInput = StringNormalizationRegex
                .Replace(request.Input, " ")
                .Trim();

            if (!TryExtractCommandVerb(normalizedInput, out var verb))
            {
                return CommandTokenizeStageResult.Failed("Команда имеет некорректный формат");
            }

            if (!TryExtractCommandQuery(normalizedInput, verb, out var commandQuery))
            {
                return CommandTokenizeStageResult.Failed("Команда имеет некорректный формат");
            }
            
            var errors = new List<string>();
            var argumentTokens = new List<TokenizedCommand.ArgumentToken>();

            foreach (var queryArgument in ExtractQueryArguments(commandQuery))
            {
                if (!IsValidArgument(queryArgument, out var key, out var value))
                {
                    errors.Add($"Параметр '{queryArgument}' имеет некорректный формат. " +
                               "Ожидаемый формат параметра команды " +
                               $"'{Settings.CommandArgumentKeyPrefix}ключ{GetCommandArgumentKeyValueDelimeter()}значение'");
                        
                    continue;
                }
                    
                argumentTokens.Add(new TokenizedCommand.ArgumentToken(key, value));
            }
            
            if (errors.Count > 0)
                return CommandTokenizeStageResult.Failed(errors.ToArray());

            return CommandTokenizeStageResult.Tokenized(new TokenizedCommand(verb, argumentTokens));
        }

        /// <summary>
        /// Параметры запроса вида {ключ}:{значение}.
        /// </summary>
        /// <param name="commandQuery">Строка запроса.</param>
        /// <returns>Параметры запроса.</returns>
        protected virtual string[] ExtractQueryArguments(string commandQuery)
        {
            var commandQueryParts = Settings.CommandArgumentKeyPrefix.HasValue
                ? commandQuery
                    .Insert(0, " ")
                    .Split(new []{$" {Settings.CommandArgumentKeyPrefix.Value}"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(str => $"{Settings.CommandArgumentKeyPrefix.Value}{str}")
                    .ToArray()
                : commandQuery
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            commandQueryParts = commandQueryParts
                .Select(arg => arg.Trim())
                .ToArray();
            
            if (Settings.CommandArgumentKeyValueDelimeter == ' ' && !Settings.CommandArgumentKeyPrefix.HasValue)
            {
                return JoinQueryArgumentParts(commandQueryParts)
                    .Select(p => string.Join(GetCommandArgumentKeyValueDelimeter(), p))
                    .ToArray();
            }

            return commandQueryParts;
        }

        /// <summary>
        /// Извлекает глагол команды без префикса.
        /// </summary>
        /// <param name="normalizedInput">Текст команды.</param>
        /// <param name="verb">Глагол команды.</param>
        /// <returns>Признак успешности извлечения глагола.</returns>
        protected virtual bool TryExtractCommandVerb(string normalizedInput, out string verb)
        {    
            verb = normalizedInput
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
            
            if (!string.IsNullOrEmpty(Settings.CommandVerbPrefix))
            {
                if (!normalizedInput.StartsWith(Settings.CommandVerbPrefix, Settings.StringComparsion))
                {
                    return false;
                }

                verb = verb?.Substring(Settings.CommandVerbPrefix.Length);
            }
            
            return verb != null;
        }

        /// <summary>
        /// Извлекает строку запроса из команды.
        /// </summary>
        /// <param name="normalizedInput">Текст команды.</param>
        /// <param name="verb">Глагол команды.</param>
        /// <param name="query">Строка запроса команды.</param>
        /// <returns>Признак успешности извлечения строки запроса.</returns>
        protected virtual bool TryExtractCommandQuery(string normalizedInput, string verb, out string query)
        {
            query = null;
            
            var queryOffsetIndex = (string.IsNullOrEmpty(Settings.CommandVerbPrefix))
                ? verb.Length
                : verb.Length + Settings.CommandVerbPrefix.Length;

            if (normalizedInput.Length < queryOffsetIndex)
                return false;
            
            query = normalizedInput.Substring(queryOffsetIndex).Trim();
            return true;
        }
        
        /// <summary>
        /// Валидация того, что аргумент является валидным.
        /// </summary>
        /// <param name="argument">Аргумент в формате ключ:значение.</param>
        /// <param name="key">Ключ аргумента.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Признак корректности аргумента.</returns>
        protected virtual bool IsValidArgument(string argument, out string key, out string value)
        {
            key = value = null;
            
            if (string.IsNullOrWhiteSpace(argument))
            {
                return false;
            }

            var parts = argument.Split(GetCommandArgumentKeyValueDelimeter(),
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return false;
                
            
            key = parts[0];
            value = parts[1];

            if (Settings.CommandArgumentKeyPrefix.HasValue)
            {
                if (!key.StartsWith(Settings.CommandArgumentKeyPrefix.Value))
                    return false;

                key = key.Remove(0, 1);
            }
            
            return true;
        }

        /// <summary>
        /// Валидатция того, что настройки являются валидными.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        /// <exception cref="ArgumentNullException">В случае, если настройки <c>null</c>.</exception>
        /// <exception cref="CommandParserException">В случае, если настройки не валидны.</exception>
        protected virtual void EnsureSettingsValid(ICommandParserSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            
            if (!settings.CommandArgumentKeyValueDelimeter.HasValue &&
                !settings.CommandArgumentKeyPrefix.HasValue)
            {
                throw new CommandParserException(
                    "Должен быть обязательно задан один из параметров настроек " +
                    $"'{nameof(ICommandParserSettings.CommandArgumentKeyValueDelimeter)}' или " +
                    $"'{nameof(ICommandParserSettings.CommandArgumentKeyPrefix)}'.");
            }
        }

        private char GetCommandArgumentKeyValueDelimeter()
        {
            return Settings.CommandArgumentKeyValueDelimeter ?? ' ';
        }
        
        private static IEnumerable<string[]> JoinQueryArgumentParts(IReadOnlyCollection<string> parts)
        {
            const int size = 2;
            for (var i = 0; i < (float)parts.Count / size; i++)
            {
                yield return parts.Skip(i * size).Take(size).ToArray();
            }
        }
    }
}