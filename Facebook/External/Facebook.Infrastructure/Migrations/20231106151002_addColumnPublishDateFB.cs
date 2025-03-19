using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNS.Facebook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addColumnPublishDateFB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "FacebookPost",
                type: "datetime(6)",
                nullable: true);

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "FacebookPost");

              }
    }
}
