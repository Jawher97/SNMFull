using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.LinkedIn.Infrastructure.Migrations
{
    public partial class CreateLinkedin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

           

            migrationBuilder.CreateTable(
                name: "LinkedInChannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Author_urn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientSecret = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChannelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedInChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkedInChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LinkedInPost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PostId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PostUrn = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicationStatus = table.Column<int>(type: "int", nullable: false),
                    ScheduleTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LinkedInChannelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedInPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkedInPost_LinkedInChannel_LinkedInChannelId",
                        column: x => x.LinkedInChannelId,
                        principalTable: "LinkedInChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkedInPost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

         

            migrationBuilder.CreateIndex(
                name: "IX_LinkedInChannel_ChannelId",
                table: "LinkedInChannel",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedInPost_LinkedInChannelId",
                table: "LinkedInPost",
                column: "LinkedInChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedInPost_PostId",
                table: "LinkedInPost",
                column: "PostId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedInPost");

           
            migrationBuilder.DropTable(
                name: "LinkedInChannel");

            
        }
    }
}
