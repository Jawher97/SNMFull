using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.BrandManagement.Infrastructure.Migrations
{
    public partial class UpdatePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total_count",
                table: "Reactions");

            migrationBuilder.AddColumn<int>(
                name: "TotalCountReactions",
                table: "Post",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCountReactions",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "Total_count",
                table: "Reactions",
                type: "int",
                nullable: true);
        }
    }
}
