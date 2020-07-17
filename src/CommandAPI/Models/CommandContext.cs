using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Models
{
    public class CommandContext : DbContext
    {
        public CommandContext (DbContextOptions<CommandContext> options) : base(options)
        {

        }

        // Connecting to Model class to Database Table
        public DbSet<Command> CommandItems {get; set;}
        
    }
}