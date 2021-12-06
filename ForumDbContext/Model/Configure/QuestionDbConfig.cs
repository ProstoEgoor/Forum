using System;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.Configure {
    class QuestionDbConfig : IEntityTypeConfiguration<QuestionDbDTO> {
        public void Configure(EntityTypeBuilder<QuestionDbDTO> builder) {
            builder.ToTable("question");

            builder.HasKey("QuestionId");

            builder.Property(question => question.CreateDate)
                .IsRequired()
                .HasColumnName("create_date")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("getdate()");

            builder.Property(question => question.AuthorName)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasColumnName("author_name");

            builder.Property(question => question.Topic)
                .IsRequired()
                .HasColumnType("nvarchar(1000)")
                .HasColumnName("topic");

            builder.Property(question => question.QuestionText)
                .IsRequired()
                .HasColumnType("nvarchar(max)")
                .HasColumnName("question_text");
        }
    }
}
