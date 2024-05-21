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
            builder.HasOne(c => c.Post)
                .WithMany(p=>p.Comments)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
