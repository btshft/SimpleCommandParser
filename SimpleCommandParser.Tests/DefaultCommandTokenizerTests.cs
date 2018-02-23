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
        [InlineData("/", ':', '-')] // /command :arg-key
        [InlineData("/", ':', null)] // /command :arg key
        [InlineData("/", '/', null)] // /command /arg key
        [InlineData("/", null, '-')] // /command arg-key
        [InlineData("/", null, '/')] // /command arg/key
        [InlineData("", ':', '-')] // command :arg-key
        [InlineData("", ':', null)] // command :arg key
        [InlineData("", null, '-')] // command arg-key
        [InlineData("--", ':', '/')] // --command :arg/key
        [InlineData("--", ':', null)] // --command :arg key
        [InlineData("--", '/', null)] // --command /arg key
        [InlineData("--", null, '/')] // --command arg/key
        [InlineData("--", '-', '/')] // --command -arg/key
        [InlineData("--", '/', '-')] // --command /arg-key
        [InlineData("--", ' ', '-')] // --command arg-key
        [InlineData("--", '-', ' ')] // --command -arg key
        [InlineData("--", null, '-')] // --command arg-key
        [InlineData("--", '-', null)] // --command -arg key
        public void Tokenize_ValidInput_SingleArgument_Returns_ValidCommand(string verbPref, char? argPref,
            char? keyValueDelimeter)
        {
            // Arrange
            var delimeter = keyValueDelimeter != null ? keyValueDelimeter : ' ';
            var settings = CreateSettings(verbPref, keyValueDelimeter, argPref);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPref}command {argPref}arg1{delimeter}value";

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
        [InlineData("/", ':', '-')] // /command :arg-key
        [InlineData("/", ':', null)] // /command :arg key
        [InlineData("/", '/', null)] // /command /arg key
        [InlineData("/", null, '-')] // /command arg-key
        [InlineData("/", null, '/')] // /command arg/key
        [InlineData("", ':', '-')] // command :arg-key
        [InlineData("", ':', null)] // command :arg key
        [InlineData("", null, '-')] // command arg-key
        [InlineData("--", ':', '/')] // --command :arg/key
        [InlineData("--", ':', null)] // --command :arg key
        [InlineData("--", '/', null)] // --command /arg key
        [InlineData("--", null, '/')] // --command arg/key
        [InlineData("--", '-', '/')] // --command -arg/key
        [InlineData("--", '/', '-')] // --command /arg-key
        [InlineData("--", ' ', '-')] // --command arg-key
        [InlineData("--", '-', ' ')] // --command -arg key
        [InlineData("--", null, '-')] // --command arg-key
        [InlineData("--", '-', null)] // --command -arg key
        public void Tokenize_ValidInput_MultipleArg_Returns_ValidCommand(string verbPref, char? keyValueDelimeter,
            char? argPref)
        {
            // Arrange
            var delimeter = keyValueDelimeter != null ? keyValueDelimeter : ' ';
            var settings = CreateSettings(verbPref, keyValueDelimeter, argPref);
            var tokenizer = new UnchangedTokenizer(settings);
            var input = $"{verbPref}command {argPref}oneArg{delimeter}one {argPref}twoArg{delimeter}two";

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

        private MutableCommandParserSettings CreateSettings(string verbPref, char? keyValueDelimeter, char? argPref)
        {
            return new MutableCommandParserSettings
            {
                CommandVerbPrefix = verbPref,
                CommandArgumentKeyPrefix = argPref,
                CommandArgumentKeyValueDelimeter = keyValueDelimeter
            };
        }
    }
}