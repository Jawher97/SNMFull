using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.Instagram.Infrastructure.Migrations
{
    public partial class addColumnPublishDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "ScheduleTime",
                table: "InstagramPosts",
                newName: "PublicationDate");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "InstagramPosts",
                newName: "ScheduleTime");

           
        }
    }
}
