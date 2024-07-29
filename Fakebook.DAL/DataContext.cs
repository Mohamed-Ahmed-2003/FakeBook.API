using Fakebook.DAL.Configurations;
using FakeBook.Domain.Aggregates.ChatRoomAggregate;
using FakeBook.Domain.Aggregates.FriendshipAggregate;
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

            builder.ApplyConfiguration(new UserProfileConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new PostCommentConfig());
            builder.ApplyConfiguration(new PostInteractionConfig());

            builder.Entity<Post>().Navigation(p => p.PostMedia).AutoInclude();

            base.OnModelCreating(builder);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts{ get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
