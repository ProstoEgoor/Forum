using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumDbContext.Model.Configure {
    class UserDbConfig : IEntityTypeConfiguration<UserDbDTO> {
        public void Configure(EntityTypeBuilder<UserDbDTO> builder) {
            builder.Property(user => user.FirstName)
                .HasColumnType("nvarchar(256)")
                .HasColumnName("first_name");

            builder.Property(user => user.LastName)
                .HasColumnType("nvarchar(256)")
                .HasColumnName("last_name");
        }
    }
}
