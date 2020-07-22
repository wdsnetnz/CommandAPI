using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CommandAPI.Controllers;
using CommandAPI.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandAPI.Test
{
    public class CommandsControllerTests : IDisposable
    {
        DbContextOptionsBuilder<CommandContext> optionsBuilder;
        CommandContext dbContext;
        CommandsController controller;

        public CommandsControllerTests()
        {
            //Given // ARRANGE
            //DBContext
            optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("UnitTestInMemBD");
            dbContext = new CommandContext(optionsBuilder.Options);

            //Controller
            controller = new CommandsController(dbContext);
        }

        public void Dispose()
        {
            optionsBuilder = null;
            foreach (var cmd in dbContext.CommandItems)
            {
                dbContext.CommandItems.Remove(cmd);
            }
            dbContext.SaveChanges();
            dbContext.Dispose();
            controller = null;
        }

        //ACTION 1 Tests: GET /api/commands
        //TEST 1.1 REQUEST OBJECTS WHEN NONE EXIST – RETURN "NOTHING"
        [Fact]
        // <method name>_<expected result>_<condition>
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //When //ACT
            var result = controller.GetCommandItems();

            //Then //ASSERT
            Assert.Empty(result.Value);
        }

        //TEST 1.2: RETURNING A COUNT OF 1 FOR A SINGLE COMMAND OBJECT
        [Fact]
        public void GetCommandItems_ReturnsOneItem_WhenDBHasOneObject()
        {
            //Given //ARRANGE
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            //When //ACT
            var result = controller.GetCommandItems();

            //Then //ASSERT
            Assert.Single(result.Value);
        }

        //TEST 3: RETURNING A COUNT OF N FOR N COMMAND OBJECTS
        [Fact]
        public void GetCommandItems_ReturnNItems_WhenDBHasNObjects()
        {
            //ARRANGE
            var command = new Command
            {
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            var command2 = new Command
            {
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            
            dbContext.CommandItems.Add(command);
            dbContext.CommandItems.Add(command2);
            dbContext.SaveChanges();
            
            //ACT
            var result = controller.GetCommandItems();
            
            //ASSERT
            Assert.Equal(2, result.Value.Count());
        }

        //TEST 4: RETURNS THE EXPECTED TYPE
        [Fact]
        public void GetCommandItems_ReturnsTheCorrect_Type()
        {
            //Given //ARRANGE
            
            //When //ACT
            var result = controller.GetCommandItems();
            
            //Then //ASSERT
            Assert.IsType<ActionResult<IEnumerable<Command>>>(result);
        }

        // TEST 2.1 INVALID RESOURCE ID – NULL OBJECT VALUE RESULT
        [Fact]
        public void TestName()
        {
            //Given
            //DB should be empty, any ID will be invalid

            //When
            var result = controller.GetCommandItem(0);

            //Then
            Assert.Null(result.Value);
        }

        // TEST 2.2 INVALID RESOURCE ID – 404 NOT FOUND RETURN CODE
        [Fact]
        public void GetCommandItem_Returns404NotFound_WhenInvalidID()
        {
            //Given
            //DB should be empty, any ID will be invalid

            //When
            var result = controller.GetCommandItem(0);

            //Then
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // TEST 2.3 VALID RESOURCE ID – CHECK CORRECT RETURN TYPE
        [Fact]
        public void GetCommandItem_ReturnsTheCorrect_Type()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;

            //When
            var result = controller.GetCommandItem(cmdId);

            //Then
            Assert.IsType<ActionResult<Command>>(result);
        }

        // TEST 2.4 VALID RESOURCE ID – CORRECT RESOURCE RETURNED
        [Fact]
        public void GetCommandItem_ReturnsTheCorrect_Resouce()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            
            //When
            var result = controller.GetCommandItem(cmdId);

            //Then
            Assert.Equal(cmdId, result.Value.Id);
        }

        // TEST 3.1 VALID OBJECT SUBMITTED – OBJECT COUNT INCREMENTS BY 1
        [Fact]
        public void PostCommandItem_ObjectCountIncrement_WhenValidObject()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            var oldCount = dbContext.CommandItems.Count();

            //When
            var result = controller.PostCommandItem(command);

            //Then
            Assert.Equal(oldCount + 1, dbContext.CommandItems.Count());
        }

        // TEST 3.2 VALID OBJECT SUBMITTED – 201 CREATED RETURN CODE
        [Fact]
        public void PostCommandItem_Returns201Created_WhenValidObject()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            //When
            var result = controller.PostCommandItem(command);

            //Then
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        // TEST 4.1 VALID OBJECT SUBMITTED – ATTRIBUTE IS UPDATED
        [Fact]
        public void PutCommandItem_AttributeUpdated_WhenValidObject()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            command.HowTo = "UPDATED";

            //When
            controller.PutCommandItem(cmdId, command);
            var result = dbContext.CommandItems.Find(cmdId);

            //Then
            Assert.Equal(command.HowTo, result.HowTo);
        }

        // TEST 4.2 VALID OBJECT SUBMITTED – 204 RETURN CODE
        [Fact]
        public void  PutCommandItem_Returns204_WhenValidObject()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            command.HowTo = "UPDATED";

            //When
            var result = controller.PutCommandItem(cmdId, command);

            //Then
            Assert.IsType<NoContentResult>(result);
        }

        // TEST 4.3 INVALID OBJECT SUBMITTED – 400 RETURN CODE
        [Fact]
        public void PutCommandItem_Returns400_WhenInvalidObject()
        {
            //Given
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id + 1;
            command.HowTo = "UPDATED";

            //When
            var result = controller.PutCommandItem(cmdId, command);

            //Then
            Assert.IsType<BadRequestResult>(result);
        }

        // TEST 4.4 INVALID OBJECT SUBMITTED – OBJECT REMAINS UNCHANGED
        [Fact]
        public void PutCommandItem_AttributeUnchanged_WhenInvalidObject()
        {
            //Given
            var commandRecordOne = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            
            dbContext.CommandItems.Add(commandRecordOne);
            dbContext.SaveChanges();
            
            var commandRecordTwo = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            //When
            controller.PutCommandItem(commandRecordOne.Id + 1, commandRecordTwo);
            var result = dbContext.CommandItems.Find(commandRecordOne.Id); 

            //Then
            Assert.Equal(commandRecordOne.HowTo, result.HowTo);
        }
    }
}