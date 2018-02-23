using System;
using System.Linq;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Core.Settings;
using SimpleCommandParser.Core.Tokenizer;
using Xunit;

namespace SimpleCommandParser.Tests
{
    public class UnchangedTokenizer : DefaultCommandTokenizer
    {
        public UnchangedTokenizer(ICommandParserSettings settings) : base(settings)
        {
        }
    }

    public class ParseRequest : ICommandParseRequest
    {
        public ParseRequest(string input)
        {
            Input = input;
        }

        public string Input { get; }
    }

    /// <summary>
    /// Набор тестов для токенизатора.
    /// </summary>
    public class DefaultCommandTokenizerTests
    {
        [Theory]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", "'", null)]
        [InlineData("/", ":", "\"", null)]
        [InlineData("/", ":", null, "\'")]
        [InlineData("/", ":", null, "\"")]
        [InlineData("/", ":", "\'", "\'")]
        [InlineData("/", ":", "\"", "\"")]     
        [InlineData("/", "/", null, null)]     
        public void Tokenize_ValidInput_SingleArgument_Returns_ValidCommand(
            string verbPrefix, 
            string argumentPrefix, 
            string keyEscape, 
            string valueEscape)
        {
            // Arrange
            var settings = CreateSettings(verbPrefix, argumentPrefix);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPrefix}command {argumentPrefix}{keyEscape}arg1{keyEscape} {valueEscape}value{valueEscape}";

            // Act
            var result = tokenizer.Tokenize(new ParseRequest(input));

            // Assert

            Assert.True(result.IsSucceed);
            Assert.Equal("command", result.Result.Verb);
            Assert.Equal(1, result.Result.Arguments.Count);
            Assert.Equal("arg1", result.Result.Arguments.First().Key);
            Assert.Equal("value", result.Result.Arguments.First().Value);
        }

        [Theory]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", "'", null)]
        [InlineData("/", ":", "\"", null)]
        [InlineData("/", ":", null, "\'")]
        [InlineData("/", ":", null, "\"")]
        [InlineData("/", ":", "\'", "\'")]
        [InlineData("/", ":", "\"", "\"")]     
        [InlineData("/", "/", null, null)] 
        public void Tokenize_ValidInputWithQuotes_SingleArgument_Returns_ValidCommand(
            string verbPrefix,
            string argumentPrefix,
            string keyEscape,
            string valueEscape)
        {
            // Arrange
            var settings = CreateSettings(verbPrefix, argumentPrefix);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPrefix}command {argumentPrefix}{keyEscape}arg1{keyEscape} {valueEscape}some 'quoted'{valueEscape}";

            // Act
            var result = tokenizer.Tokenize(new ParseRequest(input));

            // Assert

            Assert.True(result.IsSucceed);
            Assert.Equal("command", result.Result.Verb);
            Assert.Equal(1, result.Result.Arguments.Count);
            Assert.Equal("arg1", result.Result.Arguments.First().Key);
            Assert.Equal("some 'quoted\'", result.Result.Arguments.First().Value);
        }
        

        [Theory]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", null, null)]
        [InlineData("/", ":", "'", null)]
        [InlineData("/", ":", "\"", null)]
        [InlineData("/", ":", null, "\'")]
        [InlineData("/", ":", null, "\"")]
        [InlineData("/", ":", "\'", "\'")]
        [InlineData("/", ":", "\"", "\"")]     
        [InlineData("/", "/", null, null)]  
        public void Tokenize_ValidInput_MultipleArg_Returns_ValidCommand(
            string verbPrefix,
            string argumentPrefix,
            string keyEscape,
            string valueEscape)
        {
            // Arrange
            var settings = CreateSettings(verbPrefix, argumentPrefix);
            var tokenizer = new UnchangedTokenizer(settings);
            
            var input = $"{verbPrefix}command " +
                        $"{argumentPrefix}{keyEscape}oneArg{keyEscape} {valueEscape}one{valueEscape} " +
                        $"{argumentPrefix}{keyEscape}twoArg{keyEscape} {valueEscape}two{valueEscape}";

            // Act
            var result = tokenizer.Tokenize(new ParseRequest(input));

            // Assert

            Assert.True(result.IsSucceed);
            Assert.Equal("command", result.Result.Verb);
            Assert.Equal(2, result.Result.Arguments.Count);

            Assert.Equal("oneArg", result.Result.Arguments.First().Key);
            Assert.Equal("one", result.Result.Arguments.First().Value);

            Assert.Equal("twoArg", result.Result.Arguments.ElementAt(1).Key);
            Assert.Equal("two", result.Result.Arguments.ElementAt(1).Value);
        }

        [Theory]
        [InlineData("/", ":")]
        [InlineData("/", "/")]
        
        public void Tokenize_ValidInput_OnlyValues_Returns_ValidCommand(string verbPrefix, string argumentPrefix)
        {
            // Arrange
            var settings = CreateSettings(verbPrefix, argumentPrefix);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPrefix}command one two";

            // Act
            var result = tokenizer.Tokenize(new ParseRequest(input));

            // Assert

            Assert.True(result.IsSucceed);
            Assert.Equal("command", result.Result.Verb);
            Assert.Equal(2, result.Result.Arguments.Count);

            Assert.Equal(string.Empty, result.Result.Arguments.First().Key);
            Assert.Equal("one", result.Result.Arguments.First().Value);

            Assert.Equal(string.Empty, result.Result.Arguments.ElementAt(1).Key);
            Assert.Equal("two", result.Result.Arguments.ElementAt(1).Value);
        }
        
        [Theory]
        [InlineData("/", ":")]
        [InlineData("/", "/")]
        public void Tokenize_ValidInput_OnlyValuesWithSpaced_Returns_ValidCommand(string verbPrefix, string argumentPrefix)
        {
            // Arrange
            var settings = CreateSettings(verbPrefix, argumentPrefix);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPrefix}command    ' one \"one\" one '     two   ' three '";

            // Act
            var result = tokenizer.Tokenize(new ParseRequest(input));

            // Assert

            Assert.True(result.IsSucceed);
            Assert.Equal("command", result.Result.Verb);
            Assert.Equal(3, result.Result.Arguments.Count);

            Assert.Equal(string.Empty, result.Result.Arguments.First().Key);
            Assert.Equal(" one \"one\" one ", result.Result.Arguments.First().Value);

            Assert.Equal(string.Empty, result.Result.Arguments.ElementAt(1).Key);
            Assert.Equal("two", result.Result.Arguments.ElementAt(1).Value);       
            
            Assert.Equal(string.Empty, result.Result.Arguments.ElementAt(2).Key);
            Assert.Equal(" three ", result.Result.Arguments.ElementAt(2).Value);
        }


        private MutableCommandParserSettings CreateSettings(string verbPref, string argumentPrefix)
        {
            return new MutableCommandParserSettings
            {
                VerbPrefix = verbPref,
                ArgumentPrefix = argumentPrefix
            };
        }
    }
}