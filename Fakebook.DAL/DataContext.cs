using Fakebook.DAL.Configurations;
using FakeBook.Domain.Aggregates.PostAggregate;
using FakeBook.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.DAL
{
    public class DataContext: IdentityDbContext
    {
        public DataContext(DbContextOptions options) :base(options) { 
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new PostCommentConfig());
            builder.ApplyConfiguration(new PostInteractionConfig());
            builder.ApplyConfiguration(new UserProfileConfig());
            base.OnModelCreating(builder);
        }

        public DbSet<UserProfile> userProfiles { get; set; }
        public DbSet<Post> Posts{ get; set; }
        
    }
}
