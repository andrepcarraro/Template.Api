using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Template.Domain;

namespace Template.Infrastructure.Data;
internal class YourEntityConfiguration //: IEntityTypeConfiguration<YourEntity>
{
    //public void Configure(EntityTypeBuilder<YourEntity> modelBuilder)
    //{
    //    modelBuilder.HasKey(e => e.Id);

    //    modelBuilder.Property(e => e.Id)
    //                .ValueGeneratedOnAdd();
    //}
}
