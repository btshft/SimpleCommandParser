namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Команда, которая была обработана.
    /// </summary>
    public class ConsumedParsedCommand<TModel> : CommandParseResult<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Оригинальная команда.
        /// </summary>
        public ICommandParseResult<TModel> Original { get; }
        
        /// <summary>
        /// Инициализиуер экземпляр <see cref="ConsumedParsedCommand{Model}"/>.
        /// </summary>
        /// <param name="original">Оригинальная команда.</param>
        protected internal ConsumedParsedCommand(ICommandParseResult<TModel> original)
            :base(CommandParseState.Consumed)
        {
            Original = original;
        }
    }
}