using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumDbContext.Migrations
{
    public partial class ChangeDateNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_question_change_date",
                table: "question");

            migrationBuilder.DropCheckConstraint(
                name: "CK_answer_change_date",
                table: "answer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "change_date",
                table: "question",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "change_date",
                table: "answer",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "change_date",
                table: "question",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "change_date",
                table: "answer",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_question_change_date",
                table: "question",
                sql: "[change_date] >= [create_date]");

            migrationBuilder.AddCheckConstraint(
                name: "CK_answer_change_date",
                table: "answer",
                sql: "[change_date] >= [create_date]");
        }
    }
}
