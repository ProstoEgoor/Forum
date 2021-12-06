using System;
using System.Linq;
using ForumDbContext.Connection;
using Microsoft.Extensions.Configuration;
using ForumDbContext.Model;
using ForumDbContext.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace ForumDbContext {
    class Program {
        static string connectionString = new ConnectionStringConfiguration().ConnectionString;

        static ForumContext CreateContext() {
            var optionsBuilder = new DbContextOptionsBuilder<ForumContext>();
            var options = optionsBuilder
                    .UseSqlServer(connectionString)
                    .Options;
            return new ForumContext(options);
        }
        static void Main(string[] args) {
            using (var context = CreateContext()) {
                var tags = context.TagsFrequency.ToArray();
                Console.WriteLine(string.Join(", ", tags.Select(tag => tag.TagName)));
            } 
        }
    }
}
