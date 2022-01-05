using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDbContext.Migrations
{
    public partial class AddVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vote",
                columns: table => new
                {
                    answer_id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    vote = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vote", x => new { x.answer_id, x.author_id });
                    table.ForeignKey(
                        name: "FK_vote_answer_answer_id",
                        column: x => x.answer_id,
                        principalTable: "answer",
                        principalColumn: "answer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vote_AspNetUsers_author_id",
                        column: x => x.author_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vote_author_id",
                table: "vote",
                column: "author_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vote");
        }
    }
}
