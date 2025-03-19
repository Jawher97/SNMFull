using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.BrandManagement.Infrastructure.Migrations
{
    public partial class AddColumnForChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Channel",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SocialAccessToken",
                table: "Channel",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SocialChannelId",
                table: "Channel",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "SocialAccessToken",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "SocialChannelId",
                table: "Channel");
        }
    }
}
