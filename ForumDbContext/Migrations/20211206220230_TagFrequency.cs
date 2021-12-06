using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDbContext.Migrations
{
    public partial class TagFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"
                CREATE VIEW tag_frequency AS
                SELECT tag_name, COUNT(*) as frequency FROM tag_in_question
	            GROUP BY tag_name;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"
                DROP VIEW tag_frequency;
            ");
        }
    }
}
