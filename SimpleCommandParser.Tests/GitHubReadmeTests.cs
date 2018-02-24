using System;
using Newtonsoft.Json;
using SimpleCommandParser.Attributes;
using SimpleCommandParser.Core.Command;
using SimpleCommandParser.Extensions;
using Xunit;

namespace SimpleCommandParser.Tests
{
    public class GitHubReadmeTests
    {
        [Fact]
        public void GitHub_Readme_ParseCommand_ShortName_Is_Valid()
        {
            // Arrange
            var input = "create :n 'package name' :t cool_package :h";
            var isHandlerCalled = false;

            // Act
            var consumed = CommandParser.Default.ParseCommand<CreatePackageCommand>(input)
                .WhenParsed(c => isHandlerCalled = true) as ConsumedCommand<CreatePackageCommand>;
  
            // Assert
            Assert.True(isHandlerCalled);
            Assert.NotNull(consumed);
            
            Assert.Equal("package name", consumed.Value.Name);
            Assert.Equal("cool_package", consumed.Value.Tag);
            Assert.True(consumed.Value.IsHidden);
        }
        
        [Fact]
        public void GitHub_Readme_ParseCommand_LongName_Is_Valid()
        {
            // Arrange
            var input = "create :name 'package name' :tag cool_package :hidden";
            var isHandlerCalled = false;

            // Act
            var consumed = CommandParser.Default.ParseCommand<CreatePackageCommand>(input)
                .WhenParsed(c => isHandlerCalled = true) as ConsumedCommand<CreatePackageCommand>;

            // Assert
            Assert.True(isHandlerCalled);
            Assert.NotNull(consumed);
            
            Assert.Equal("package name", consumed.Value.Name);
            Assert.Equal("cool_package", consumed.Value.Tag);
            Assert.True(consumed.Value.IsHidden);
        }
        
        internal class CreatePackageCommand
        { 
            [Parameter("n", "name")]
            public string Name { get; set; }
      
            [Parameter("t", "tag", Required = false)]
            public string Tag { get; set; }    
                                  
            [Option("h", "hidden")]
            public bool IsHidden { get; set; }
        }
    }
}