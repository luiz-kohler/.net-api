using Domain.Entities;
using Infra.Database.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.Mappings
{
    public class ChoreMapping : BaseMapping<Chore>
    {
        public override string TableName => "CHORES";

        protected override void MapearEntidade(EntityTypeBuilder<Chore> builder)
        {
            builder.Property(chore => chore.Title)
                .HasColumnName("TITLE")
                .IsRequired();

            builder.Property(chore => chore.Description)
                .HasColumnName("DESCRIPTION")
                .IsRequired();

            builder.Property(chore => chore.Status)
                .HasColumnName("STATUS")
                .IsRequired();
        }
    }
}
