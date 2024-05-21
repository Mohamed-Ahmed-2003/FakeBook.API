

using FakeBook.Domain.Aggregates.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fakebook.DAL.Configurations
{
    public class GeneralInfoConfig : IEntityTypeConfiguration<GeneralInfo>
    {
        public void Configure(EntityTypeBuilder<GeneralInfo> builder)
        {
        }
    }
}
