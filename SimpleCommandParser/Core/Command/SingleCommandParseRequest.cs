using System;

namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Модель запроса разбора команды.
    /// </summary>
    public class SingleCommandParseRequest<TModel> : ICommandParseRequest
         where TModel : class, new()
    {
        /// <inheritdoc />
        public string Input { get; }

        /// <summary>
        /// Тип модели.
        /// </summary>
        public Type ModelType { get; }

        /// <summary>
        /// Инициализирует экземпляр <see cref="SingleCommandParseRequest{TModel}"/>.
        /// </summary>
        /// <param name="input">Входная строка.</param>
        internal SingleCommandParseRequest(string input)
        {
            Input = input;
            ModelType = typeof(TModel);
        }
    }
}