using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Test
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "Platform xUnit",
                CommandLine = "dotnet test"
            };
        }

        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {

            //Given //Arrange
            // DON’T REPEAT YOURSELF, Its already initialize at top.

            //When //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Then //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            //Given //Arrange
            // DON’T REPEAT YOURSELF, Its already initialize at top.

            //When //Act
            testCommand.Platform = "Platform xUnit";

            //Then //Assert
            Assert.Equal("Platform xUnit", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            //Given //Arrange
            // DON’T REPEAT YOURSELF, Its already initialize at top.

            //When //Act
            testCommand.CommandLine = "dotnet test";

            //Then //Assert
            Assert.Equal("dotnet test", testCommand.CommandLine);
        }
    }
}