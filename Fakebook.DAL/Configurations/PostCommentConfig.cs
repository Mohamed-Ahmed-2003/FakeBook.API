using FakeBook.Domain.Aggregates.PostAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.DAL.Configurations
{
    public class PostCommentConfig : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(c => c.CommentId);
      

            builder.HasOne(pc => pc.Post) // Navigation property on PostComment
          .WithMany(p => p.Comments) // Collection property on Post
          .HasForeignKey(pc => pc.PostId) // Foreign key property
          .OnDelete(DeleteBehavior.Cascade); // Configure cascade delete
        }
    }
}
