using System;
using ForumDbContext.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ForumModel;
using ForumConsole.DB.Mapping;
using ForumDbContext.Connection;

namespace ForumConsole.DB {
    static class DBManager {
        static string connectionString = ConnectionManager.GetConnectionString();

        static ForumContext CreateContext() {
            var optionsBuilder = new DbContextOptionsBuilder<ForumContext>();
            var options = optionsBuilder
                    .UseSqlServer(connectionString)
                    .EnableSensitiveDataLogging()
                    .Options;
            return new ForumContext(options);
        }

        public static List<Question> GetQuestions() {
            using var context = CreateContext();

            return context.Question
                .Include(question => question.Answers)
                .Include(question => question.Tags)
                .AsEnumerable()
                .Select(question => QuestionMapper.Map(question))
                .ToList();
        }

        public static void UpdateQuestions(IEnumerable<Question> questions) {
            using var context = CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try {
                var questionsInDb = context.Question
                    .Include(question => question.Answers)
                    .Include(question => question.Tags)
                    .AsNoTracking();


                foreach (var questionInDb in questionsInDb) {
                    var question = questions.FirstOrDefault(question => question.Id == questionInDb.QuestionId);

                    if (question != null) {
                        context.Replace(questionInDb, QuestionMapper.Map(question));

                    } else {
                        context.Remove(questionInDb);
                    }
                }
                context.SaveChanges();

                var questionToAdd = questions
                    .Where(question => question.Id == null/* || context.Question.Find(question.Id) == null*/)
                    .Select(question => QuestionMapper.Map(question))
                    .ToList();

                context.AddRange(questionToAdd);
                context.SaveChanges();

                transaction.Commit();
            } catch (Exception e) {
                Console.Error.WriteLine("Произошла ошибка при сохранении в базу данных: " + e.Message);
                transaction.Rollback();
            }
        }
    }
}
