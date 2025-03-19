using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.Twitter.Infrastructure.Migrations
{
    public partial class addColumnPublishDateTwitter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "TwitterPost");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "TwitterPost",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "TwitterPost");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleTime",
                table: "TwitterPost",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
