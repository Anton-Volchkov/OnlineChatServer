using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineChatServer.Domain;
using OnlineChatServer.Domain.Entities;

namespace OnlineChatServer.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<UnreadDialog> UnreadDialogs { get; set; }

        public DbSet<BlackList> BlackList { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Нужно для Identity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}