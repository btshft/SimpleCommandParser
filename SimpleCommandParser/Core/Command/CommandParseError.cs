namespace SimpleCommandParser.Core.Command
{
    /// <summary>
    /// Перечисление кодов ошибки.
    /// </summary>
    public enum CommandParseErrorCode
    {
        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        Undefined = 0,
        
        /// <summary>
        /// Некорректный ввод.
        /// </summary>
        BrokenInput = 100,
        
        /// <summary>
        /// Ошибка при инициализации модели.
        /// </summary>
        CommandInitializationFailed = 200,
        
        /// <summary>
        /// Невозможно определить тип модели для разбора команды.
        /// </summary>
        UnableResolveCommandType = 300,
    }
    
    /// <summary>
    /// Ошибка разбора команды.
    /// </summary>
    public class CommandParseError
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseError"/>.
        /// </summary>
        /// <param name="text">Текст ошибки.</param>
        /// <param name="code">Код ошибки.</param>
        internal CommandParseError(string text, CommandParseErrorCode code)
        {
            Text = text;
            Code = code;
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="CommandParseError"/>.
        /// </summary>
        /// <param name="text">Текст ошибки.</param>
        /// <param name="code">Код ошибки.</param>
        internal CommandParseError(string text, CommandParseErrorCode? code)
        {
            Text = text;
            Code = code ?? CommandParseErrorCode.Undefined;
        }
        
        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string Text { get; }
        
        /// <summary>
        /// Код ошибки.
        /// </summary>
        public CommandParseErrorCode Code { get; }
    }
}