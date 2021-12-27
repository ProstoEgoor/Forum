using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDbContext.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question",
                columns: table => new
                {
                    question_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    change_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    author_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    topic = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    question_text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question", x => x.question_id);
                    table.CheckConstraint("CK_question_change_date", "[change_date] >= [create_date]");
                });

            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    answer_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    change_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    author_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    answer_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vote_positive = table.Column<int>(type: "int", nullable: false),
                    vote_negative = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([vote_positive]-[vote_negative])", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer", x => x.answer_id);
                    table.CheckConstraint("CK_answer_change_date", "[change_date] >= [create_date]");
                    table.CheckConstraint("CK_answer_vote_pos", "[vote_positive] >= 0");
                    table.CheckConstraint("CK_answer_vote_neg", "[vote_negative] >= 0");
                    table.ForeignKey(
                        name: "FK_answer_question_question_id",
                        column: x => x.question_id,
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_in_question",
                columns: table => new
                {
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    tag_name = table.Column<string>(type: "nvarchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_in_question", x => new { x.question_id, x.tag_name });
                    table.ForeignKey(
                        name: "FK_tag_in_question_question_question_id",
                        column: x => x.question_id,
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answer_question_id",
                table: "answer",
                column: "question_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "tag_in_question");

            migrationBuilder.DropTable(
                name: "question");
        }
    }
}
