using SimpleCommandParser.Core.Builder;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Examples.CustomParser;
using SimpleCommandParser.Examples.ExtendParser;
using SimpleCommandParser.Examples.Models;
using Xunit;

namespace SimpleCommandParser.Examples
{
    public class Examples
    {
        [Fact]
        public void ExtendedCommandParser_Works_Correct()
        {
            // Arrange
            var parser = new CommandParserBuilder()
                .WithTypeResolver(s => new CustomCommandTypeResolver(s))
                .WithInitializer(s => new CustomCommandInitializer(s))
                .WithTokenizer(s => new CustomCommandTokenizer(s))
                .WithSettings(s =>
                {
                    s.VerbPrefix = "/";
                    s.ArgumentKeyPrefix = "-";
                })
                .Build();

            // Act
            var result = parser.ParseCommand<SampleCommand>("/sample -f value") as ParsedCommand<SampleCommand>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("value", result.Value.Foo);
        }

        [Fact]
        public void CustomCommandParser_Works_Correct()
        {
            // Arrange
            var parser = new CustomCommandParser(s =>
            {
                s.VerbPrefix = "/";
                s.ArgumentKeyPrefix = "-";
                
            });
            
            // Act
            var result = parser.ParseCommand<SampleCommand>("/sample -f value") as ParsedCommand<SampleCommand>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("value", result.Value.Foo);
        }
    }
}