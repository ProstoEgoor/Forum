using System;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.Configure {
    class TagFrequencyDbConfig : IEntityTypeConfiguration<TagFrequencyDbDTO> {
        public void Configure(EntityTypeBuilder<TagFrequencyDbDTO> builder) {
            builder.ToView("tag_frequency");

            builder.HasKey(tag => tag.TagName);

            builder.Property(tag => tag.TagName)
                .HasColumnName("tag_name");
        }
    }
}
