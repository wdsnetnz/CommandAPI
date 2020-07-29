using System;
using System.Collections.Generic;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _context;
        public CommandsController(CommandContext context) => _context = context;

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommandItems()
        {
            // return new string[] {"this", "is", "hard", "coded", "Array", "Azure"};
            return _context.CommandItems;
        }

        [HttpGet("{id}/like/{likeId}")]
        public ActionResult<IEnumerable<Command>> GetAllCommandItems(int id, int likeId)
        {
            var command = new Command
            {
                Id = 1000000000,
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            List<Command> commandLst = new List<Command>();
            commandLst.Add(command);

            return commandLst;
        }

        // http://www.binaryintellect.net/articles/9db02aa1-c193-421e-94d0-926e440ed297.aspx
        // The action name is GetCommandByHowTo, To invoke this action you need to explicitly specify the action name in the URL.
        // http://localhost:5000/api/commands/GetCommandByHowTo/dosomething

        [Route("[action]/{howto}")]
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommandByHowTo(string howto)
        {
            var command = new Command
            {
                Id = 1000000000,
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            List<Command> commandLst = new List<Command>();
            commandLst.Add(command);

            return commandLst;
        }

        [Route("[action]/{howto}")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetCommandsInString(string howto)
        {
            return new string[] {"this", "is", "hard", "coded", "Array", "Azure"};            
        }

        //GET:  api/commands/{Id}
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);

            if(commandItem == null)
            {
                return NotFound();
            }

            return commandItem;
        }

        //POST:     api/commands
        [HttpPost]
        public ActionResult<Command> PostCommandItem(Command command)
        {
            _context.CommandItems.Add(command);

            try
            {
                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return CreatedAtAction("GetCommandItem", new Command { Id = command.Id }, command);
        }

        //PUT:      api/commands/{Id}
        [HttpPut("{Id}")]
        public ActionResult PutCommandItem(int id, Command command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            _context.Entry(command).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        //DELETE:       api/commands/{Id}
        [HttpDelete("{id}")]
        public ActionResult<Command> DeleteCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);

            if(commandItem == null)
            {
                return NotFound();
            }

            _context.CommandItems.Remove(commandItem);
            _context.SaveChanges();

            return commandItem;
        }
    }
}