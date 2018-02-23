namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Команда, которая была обработана.
    /// </summary>
    public class ConsumedCommand<TModel> : CommandParseResult<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Оригинальная команда.
        /// </summary>
        public ICommandParseResult<TModel> Original { get; }
        
         
        /// <summary>
        /// Инициализиуер экземпляр <see cref="ConsumedCommand{TModel}"/>.
        /// </summary>
        /// <param name="original">Оригинальная команда.</param>
        protected internal ConsumedCommand(ICommandParseResult<TModel> original)
            :base(CommandParseState.Consumed)
        {
            Original = original;
        }
    }
}