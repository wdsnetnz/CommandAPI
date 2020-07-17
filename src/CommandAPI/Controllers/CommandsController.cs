using System.Collections.Generic;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;


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
            // return new string[] {"this", "is", "hard", "coded"};
            return _context.CommandItems;
        }
    }
}