using JobsityChatroom.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobsityChatroom.Data
{
    public class JobsityChatroomContext : IdentityDbContext<JobsityChatroomUser>
    {
        public DbSet<MessageEntity> Messages { get; set; }
        public JobsityChatroomContext(DbContextOptions<JobsityChatroomContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //TODO: Relocate context
            builder.Entity<MessageEntity>()
                .HasKey(m => new { m.Timestamp, m.MessageText });
            builder.Entity<MessageEntity>()
                .Property(m => m.Timestamp)
                .HasDefaultValueSql("getdate()");
        }
    }
}
