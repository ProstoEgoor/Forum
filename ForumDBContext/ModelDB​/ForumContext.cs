using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ForumDBContext.ModelDB​
{
    public partial class ForumContext : DbContext
    {
        public ForumContext()
        {
        }

        public ForumContext(DbContextOptions<ForumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<FilterByTag> FilterByTags { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<SearchByText> SearchByTexts { get; set; }
        public virtual DbSet<SortAnswer> SortAnswers { get; set; }
        public virtual DbSet<TagFrequency> TagFrequencies { get; set; }
        public virtual DbSet<TagInQuestion> TagInQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ForumDb_ConnectionString"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_100_CI_AI");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answer");

                entity.Property(e => e.AnswerId).HasColumnName("answer_id");

                entity.Property(e => e.AnswerText)
                    .IsRequired()
                    .HasColumnName("answer_text");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author_name");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating")
                    .HasComputedColumnSql("([vote_positive]-[vote_negative])", false);

                entity.Property(e => e.VoteNegative).HasColumnName("vote_negative");

                entity.Property(e => e.VotePositive).HasColumnName("vote_positive");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__answer__question__2C3393D0");
            });

            modelBuilder.Entity<FilterByTag>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("filter_by_tag");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author_name");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("question_text");

                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("topic");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author_name");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("question_text");

                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("topic");
            });

            modelBuilder.Entity<SearchByText>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("search_by_text");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author_name");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasColumnName("question_text");

                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("topic");
            });

            modelBuilder.Entity<SortAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("sort_answer");

                entity.Property(e => e.AnswerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("answer_id");

                entity.Property(e => e.AnswerText)
                    .IsRequired()
                    .HasColumnName("answer_text");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author_name");

                entity.Property(e => e.CreateDate).HasColumnName("create_date");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.VoteNegative).HasColumnName("vote_negative");

                entity.Property(e => e.VotePositive).HasColumnName("vote_positive");
            });

            modelBuilder.Entity<TagFrequency>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tag_frequency");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("tag");
            });

            modelBuilder.Entity<TagInQuestion>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.Tag })
                    .HasName("PK__tag_in_q__33031489BF332EC7");

                entity.ToTable("tag_in_question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasColumnName("tag");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TagInQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__tag_in_qu__quest__2D27B809");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
