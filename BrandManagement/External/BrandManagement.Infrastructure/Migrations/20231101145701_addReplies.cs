using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.BrandManagement.Infrastructure.Migrations
{
    public partial class addReplies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RepliesId",
                table: "Comment",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RepliesId",
                table: "Comment",
                column: "RepliesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_RepliesId",
                table: "Comment",
                column: "RepliesId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_RepliesId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_RepliesId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "RepliesId",
                table: "Comment");
        }
    }
}
