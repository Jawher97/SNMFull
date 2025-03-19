using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.LinkedIn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnPublicationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "LinkedInPost");


            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "LinkedInPost",
                type: "datetime(6)",
                nullable: true);

      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "LinkedInPost");

            

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleTime",
                table: "LinkedInPost",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

           
        }
    }
}
