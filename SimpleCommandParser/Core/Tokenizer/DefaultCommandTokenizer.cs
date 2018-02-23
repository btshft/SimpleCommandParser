using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.StageResults;
using SimpleCommandParser.Exceptions;
using SimpleCommandParser.Extensions;

namespace SimpleCommandParser.Core.Tokenizer
{
    /// <summary>
    /// Реализация нетипизированного парсера команд.
    /// </summary>
    public class DefaultCommandTokenizer : ICommandTokenizer
    {
        /// <summary>
        /// Массив кавычек.
        /// </summary>
        protected readonly char[] Quotes = new[] {'\'', '\"' };
              
        /// <summary>
        /// Регулярное выражение для нормализации строки.
        /// </summary>
        protected Regex StringNormalizationRegex { get; set; }
        
        /// <summary>
        /// Провайдер настроек парсера.
        /// </summary>
        protected ICommandParserSettings Settings { get; }

        /// <summary>
        /// Инициалиризует новый экземпляр <see cref="DefaultCommandTokenizer"/>.
        /// </summary>
        /// <param name="settings">Настройки.</param>
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
                if (!TrySeparateArgumentKeyValue(queryArgument, out var key, out var value))
                {
                    errors.Add($"Параметр '{queryArgument}' имеет некорректный формат. " +
                               "Ожидаемый формат параметра команды " +
                               $"'{Settings.ArgumentKeyPrefix}ключ значение'");
                        
                    continue;
                }
                    
                argumentTokens.Add(new TokenizedCommand.ArgumentToken(key, value));
            }
            
            if (errors.Count > 0)
                return CommandTokenizeStageResult.Failed(errors.ToArray());

            return CommandTokenizeStageResult.Tokenized(new TokenizedCommand(verb, argumentTokens));
        }

        /// <summary>
        /// Параметры запроса вида {префикс}{ключ} {значение}.
        /// </summary>
        /// <param name="commandQuery">Строка запроса.</param>
        /// <returns>Параметры запроса.</returns>
        protected virtual string[] ExtractQueryArguments(string commandQuery)
        {
            var commandQueryParts = commandQuery
                .PreserveSplit(new[] { Settings.ArgumentKeyPrefix })
                .TrimAll()
                .ToArray();

            // Если в запросе нет ключей, то значит там только параметры
            if (commandQueryParts.Length < 2 && !commandQueryParts[0].StartsWith(
                    Settings.ArgumentKeyPrefix, Settings.StringComparsion))
            {
                var parts = new List<string>();

                while (!string.IsNullOrEmpty(commandQuery))
                {
                    var startQuote = Quotes.FirstOrDefault(q => commandQuery.StartsWith(q));

                    var part = startQuote == default(char)
                        ? string.Concat(commandQuery.TakeWhile(c => c != ' '))
                        : string.Concat(commandQuery.Skip(1).TakeWhile(c => c != startQuote))
                            .Escape(startQuote);
                    
                    parts.Add(part);
                    commandQuery = commandQuery.Remove(0, part.Length).Trim();
                }

                commandQueryParts = parts.ToArray();
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
                .SplitByWhiteSpace()
                .FirstOrDefault();
            
            if (!string.IsNullOrEmpty(Settings.VerbPrefix))
            {
                if (!normalizedInput.StartsWith(Settings.VerbPrefix, Settings.StringComparsion))
                {
                    return false;
                }

                verb = verb?.Substring(Settings.VerbPrefix.Length);
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
            
            var queryOffsetIndex = (string.IsNullOrEmpty(Settings.VerbPrefix))
                ? verb.Length
                : verb.Length + Settings.VerbPrefix.Length;

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
        protected virtual bool TrySeparateArgumentKeyValue(string argument, out string key, out string value)
        {
            key = value = string.Empty;
            
            if (string.IsNullOrWhiteSpace(argument))
                return false;

            if (argument.StartsWith(Settings.ArgumentKeyPrefix, Settings.StringComparsion))
            {
                key = string
                    .Concat(argument
                        .Skip(Settings.ArgumentKeyPrefix.Length)
                        .TakeWhile(c => c != ' '));

                argument = argument.Remove(0, key.Length + Settings.ArgumentKeyPrefix.Length);
            }

            key = key.Trim(Quotes).Trim(' ');      
            value = argument.Trim(' ');

            if (value.StartsWith('\'') && value.EndsWith('\'') ||
                value.StartsWith('\"') && value.EndsWith('\"'))
            {
                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);
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
            
            if (string.IsNullOrWhiteSpace(settings.ArgumentKeyPrefix))
            {
                throw new CommandParserException(
                    "Префикс команды должен быть задан и отличаться от пробела " +
                    $"{nameof(ICommandParserSettings.ArgumentKeyPrefix)}");
            }
        }
    }
}