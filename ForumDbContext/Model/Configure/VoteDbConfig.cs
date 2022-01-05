using System;
using System.Collections.Generic;
using System.Text;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumDbContext.Model.Configure {
    class VoteDbConfig : IEntityTypeConfiguration<VoteDbDTO> {
        public void Configure(EntityTypeBuilder<VoteDbDTO> builder) {
            builder.ToTable("vote");

            builder.HasKey(vote => new {
                vote.AnswerId,
                vote.AuthorId
            });

            builder.Property(vote => vote.AnswerId)
                .IsRequired()
                .HasColumnType("bigint")
                .HasColumnName("answer_id");

            builder.Property(vote => vote.AuthorId)
                .IsRequired()
                .HasColumnType("nvarchar(450)")
                .HasColumnName("author_id");

            builder.Property(vote => vote.Vote)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("vote");

            builder.HasOne(vote => vote.Answer)
                .WithMany(answer => answer.Votes)
                .HasForeignKey(vote => vote.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vote => vote.Author)
                .WithMany(user => user.Votes)
                .HasForeignKey(vote => vote.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
