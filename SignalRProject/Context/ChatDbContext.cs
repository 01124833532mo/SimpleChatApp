using Microsoft.EntityFrameworkCore;
using SignalRProject.Models;

namespace SignalRProject.Context
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {

        }


        public DbSet<Message> Messages { get; set; }

    }
}
