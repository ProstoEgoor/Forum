using System;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace ForumDbContext.Model.Configure {
    class AnswerDbConfig : IEntityTypeConfiguration<AnswerDbDTO> {
        public void Configure(EntityTypeBuilder<AnswerDbDTO> builder) {
            builder.ToTable("answer");

            builder.HasKey("AnswerId");

            builder.Property(answer => answer.AnswerId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("answer_id");

            builder.Property(answer => answer.QuestionId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("question_id");

            builder.Property(answer => answer.CreateDate)
                .IsRequired()
                .HasColumnName("create_date")
                .HasColumnType("datetime2")
                .HasDefaultValueSql("getdate()");

            builder.Property(answer => answer.AuthorName)
                .IsRequired()
                .HasColumnType("nvarchar(100)")
                .HasColumnName("author_name");

            builder.Property(answer => answer.AnswerText)
                .IsRequired()
                .HasColumnType("nvarchar(max)")
                .HasColumnName("answer_text");

            builder.Property(answer => answer.VotePositive)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("vote_positive");

            builder.HasCheckConstraint("CK_answer_vote_pos", "[vote_positive] >= 0");

            builder.Property(answer => answer.VoteNegative)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("vote_negative");

            builder.HasCheckConstraint("CK_answer_vote_neg", "[vote_negative] >= 0");

            builder.Property(answer => answer.Rating)
                .HasColumnType("int")
                .HasColumnName("rating")
                .HasComputedColumnSql("([vote_positive]-[vote_negative])", false);

            builder.HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId);
        }
    }
}
