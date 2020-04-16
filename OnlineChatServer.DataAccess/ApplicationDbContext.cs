using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineChatServer.Domain;

namespace OnlineChatServer.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatMessage> Messages { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}