using FakeBook.Domain.Aggregates.PostAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Fakebook.DAL.Configurations
{
    public class PostConfig :IEntityTypeConfiguration<Post>
    {
      

        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasMany(p => p.PostMedia)
                          .WithOne()
                          .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Comments)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Interactions)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            // Ensure Post does not delete UserProfile
            builder.HasOne(p => p.userProfile)
                   .WithMany()
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
