using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.BrandManagement.Infrastructure.Migrations
{
    public partial class addReactionsToComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "Reactions",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "TotalCountShares",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Comment",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Comment_CommentId",
                table: "Reactions",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Comment_CommentId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "TotalCountShares",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Comment");
        }
    }
}
