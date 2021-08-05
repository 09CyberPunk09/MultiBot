using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Core.Entites;

namespace Persistence.Sql.EntityConfig
{
	public class NoteEC : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Notes");

            builder.HasKey(e => e.Id);
            
        }
    }
}
