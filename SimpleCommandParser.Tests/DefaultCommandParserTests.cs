using System;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Extensions;
using SimpleCommandParser.Tests.Models;
using Xunit;

namespace SimpleCommandParser.Tests
{
    /// <summary>
    /// Набор тестов для парсера команд по умолчанию.
    /// </summary>
    public class DefaultCommandParserTests
    {
        #region ParseCommand
        
        [Fact]
        public void ParseCommand_ValidInput_Command_Returns_ParsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
        }

        [Fact]
        public void ParseCommand_ValidInput_RequiredProperies_Initialized()
        {       
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            
            Assert.Equal("abc", parsedCommand.Value.RequiredArg1);
            Assert.Equal("cde", parsedCommand.Value.RequiredArg2);
        }

        [Fact]
        public void ParseCommand_ValidInput_OptionalProperties_Initialized()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde :arg3 kkk";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            
            Assert.Equal("kkk", parsedCommand.Value.OptionalArg3);
        }

        [Fact]
        public void ParseCommand_ValidInput_OptionalProperties_SetToDefault()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            
            Assert.Equal(default(string), parsedCommand.Value.OptionalArg3);
        }
        
        [Fact]
        public void ParseCommand_ValidInput_TypeConversionSucceed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde :arg4 199";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            
            Assert.Equal(199, parsedCommand.Value.OptionalArg4);
        }
        
        [Fact]
        public void ParseCommand_ValidInput_OptionSetted()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 cde :arg7";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);          
            Assert.Equal(true, parsedCommand.Value.Option7);
        }
        
        [Fact]
        public void ParseCommand_ValidInput_LongArgNames()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :argument1 abc :argument2 cde :argument7";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var parsedCommand = result as ParsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(parsedCommand);          
            Assert.Equal(true, parsedCommand.Value.Option7);
        }
        
        [Fact]
        public void ParseCommand_InvalidInput_EmptyInput_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }

        [Fact]
        public void ParseCommand_InvalidInput_BrokenVerb_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = new CommandParser(s =>
            {
                s.StringComparsion = StringComparison.InvariantCultureIgnoreCase;
                s.ArgumentKeyPrefix = ":";
                s.VerbPrefix = "/";
            });
            
            var input = "signal :arg1 abc :arg2 cde";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }
        

        [Fact]
        public void ParseCommand_InvalidArgument_MissingRequiredArgument_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 kd :arg2";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }
        
        [Fact]
        public void ParseCommand_InvalidModel_NoPropertySetter_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 a :arg5 cde";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }
        
        [Fact]
        public void ParseCommand_InvalidModel_TypeConverterForPropertyMissing_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            
            var input = "signal :arg1 abc :arg2 ddd :arg6 hhh";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }
        
        [Fact]
        public void ParseCommand_InvalidModel_TypeConversionFailed_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "signal :arg1 abc :arg2 ddd :arg5 72.2";
            
            // Act
            var result = parser.ParseCommand<SingleCommandModel>(input);
            var unparsedCommand = result as UnparsedCommand<SingleCommandModel>;
            
            // Assert         
            Assert.NotNull(unparsedCommand);
        }
        
        #endregion

        #region ParseCommands

        [Fact]
        public void ParseCommands_SingleCommand_Valid_Input_Returns_ParsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new [] { typeof(FirstCommand) });
            var parsedCommand = result as ParsedCommand<object>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            Assert.Equal(typeof(FirstCommand), parsedCommand.Value.GetType());
        }

        [Fact]
        public void ParseCommand_TwoCommands_FirstCommandInput_Returns_FirstModel()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new[] { typeof(FirstCommand), typeof(SecondCommand) });
            var parsedCommand = result as ParsedCommand<object>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            Assert.Equal(typeof(FirstCommand), parsedCommand.Value.GetType());
        }

        [Fact]
        public void ParseCommand_TwoCommands_SecondCommandInput_Returns_SecondModel()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "second :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new [] { typeof(FirstCommand), typeof(SecondCommand) });
            var parsedCommand = result as ParsedCommand<object>;
            
            // Assert         
            Assert.NotNull(parsedCommand);
            Assert.Equal(typeof(SecondCommand), parsedCommand.Value.GetType());
        }
        
        [Fact]

        public void ParseCommand_TwoCommands_NotMatchedInput_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new [] { typeof(FirstCommand), typeof(SecondCommand) });
            var command = result as UnparsedCommand<object>;
            
            // Assert         
            Assert.NotNull(command);
        }

        [Fact]
        public void ParseCommand_ThreeModels_DuplicateVerbs_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new [] { typeof(FirstCommand), typeof(SecondCommand), typeof(SecondCommandDuplicate) });
            var command = result as UnparsedCommand<object>;
            
            // Assert         
            Assert.NotNull(command);
        }

        [Fact]
        public void ParseCommand_TwoModels_MissingVerb_Returns_UnparsedCommand()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            // Act
            var result = parser.ParseCommands(input, new [] { typeof(FirstCommand), typeof(VerbMissingCommand) });
            var command = result as UnparsedCommand<object>;
            
            // Assert         
            Assert.NotNull(command);
        }
        
        #endregion

        #region ICommandParseResultExtensions

        [Fact]
        public void WhenParsed_SingleCommand_SingleHandlerCalled()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommand<FirstCommand>(input)
                .WhenParsed(_ => isFirstCommandHandlerCalled = true);
            
            // Assert         
            Assert.True(isFirstCommandHandlerCalled);
        }
        
        [Fact]
        public void WhenParsed_SingleHandlerCalled_If_CommandParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isFirstCommandHandlerCalledTwice = false;
            var isSecondCommandHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true)
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalledTwice = true)
                .WhenParsed<SecondCommand>(_ => isSecondCommandHandlerCalled = true);
            
            // Assert         
            Assert.True(isFirstCommandHandlerCalled);
            Assert.False(isFirstCommandHandlerCalledTwice);
            Assert.False(isSecondCommandHandlerCalled);
        }

        [Fact]
        public void WhenParsed_MultipleHandlersCalled_IfCommandParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isFirstCommandHandlerCalledTwice = false;
            var isSecondCommandHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true, finalConsumer: false)
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalledTwice = true)
                .WhenParsed<SecondCommand>(_ => isSecondCommandHandlerCalled = true);
            
            // Assert         
            Assert.True(isFirstCommandHandlerCalled);
            Assert.True(isFirstCommandHandlerCalledTwice);
            Assert.False(isSecondCommandHandlerCalled);
        }

        [Fact]
        public void WhenParsed_ChangesCommand_To_Consumed_If_CommandParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => { })
                .WhenParsed<SecondCommand>(_ => { });
            
            
            // Assert
            Assert.Equal(typeof(ConsumedCommand<object>), result.GetType());
        }

        [Fact]
        public void WhenParsed_NotChangesCommand_To_Consumed_If_CommandParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => { }, finalConsumer: false)
                .WhenParsed<SecondCommand>(_ => { });
            
            
            // Assert
            Assert.NotEqual(typeof(ConsumedCommand<object>), result.GetType());
            Assert.Equal(typeof(ParsedCommand<object>), result.GetType());
        }

        [Fact]
        public void WhenParsed_HandlerNotCalled_If_CommandNotParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isSecondCommandHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true)
                .WhenParsed<SecondCommand>(_ => isSecondCommandHandlerCalled = true);
            
            // Assert         
            Assert.False(isFirstCommandHandlerCalled);
            Assert.False(isSecondCommandHandlerCalled);
        }

        [Fact]
        public void WhenNotParsed_SingleHandlerCalled_If_CommandNotParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isSecondCommandHandlerCalled = false;
            var isErrorHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true)
                .WhenParsed<SecondCommand>(_ => isSecondCommandHandlerCalled = true)
                .WhenNotParsed(_ => isErrorHandlerCalled = true);
            
            // Assert         
            Assert.True(isErrorHandlerCalled);
            Assert.False(isFirstCommandHandlerCalled);
            Assert.False(isSecondCommandHandlerCalled);
        }

        [Fact]
        public void WhenNotParsed_MultipleHandlersCalled_If_CommandNotParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "unknown :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isErrorHandlerCalled = false;
            var isErrorHandlerCalledTwice = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true)
                .WhenNotParsed(_ => isErrorHandlerCalled = true, finalConsumer: false)
                .WhenNotParsed(_ => isErrorHandlerCalledTwice = true);
            
            // Assert         
            Assert.True(isErrorHandlerCalled);
            Assert.True(isErrorHandlerCalledTwice);
            Assert.False(isFirstCommandHandlerCalled);
        }

        [Fact]
        public void WhenNotParsed_HandlerNotCalled_If_CommandParsed()
        {
            // Arrange
            var parser = CommandParser.Default;
            var input = "first :arg1 abc :arg2 10";
            
            var isFirstCommandHandlerCalled = false;
            var isErrorHandlerCalled = false;
            
            // Act
            var result = parser
                .ParseCommands(input, new[] {typeof(FirstCommand), typeof(SecondCommand)})
                .WhenParsed<FirstCommand>(_ => isFirstCommandHandlerCalled = true)
                .WhenNotParsed(_ => isErrorHandlerCalled = true);
            
            // Assert         
            Assert.False(isErrorHandlerCalled);
            Assert.True(isFirstCommandHandlerCalled);
        }

        #endregion
    }
}