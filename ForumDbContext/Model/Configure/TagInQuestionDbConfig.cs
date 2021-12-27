using System;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.Configure {
    class TagInQuestionDbConfig : IEntityTypeConfiguration<TagInQuestionDbDTO> {
        public void Configure(EntityTypeBuilder<TagInQuestionDbDTO> builder) {
            builder.ToTable("tag_in_question");

            builder.HasKey(tagInQuestion => new {
                tagInQuestion.QuestionId,
                tagInQuestion.TagName
            });

            builder.Property(tagInQuestion => tagInQuestion.QuestionId)
                .IsRequired()
                .HasColumnType("bigint")
                .HasColumnName("question_id");

            builder.Property(tagInQuestion => tagInQuestion.TagName)
                .IsRequired()
                .HasColumnType("nvarchar(256)")
                .HasColumnName("tag_name");

            builder.HasOne(tagInQuestion => tagInQuestion.Question)
                .WithMany(question => question.Tags)
                .HasForeignKey(tagInQuestion => tagInQuestion.QuestionId);
        }
    }
}
