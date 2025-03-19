using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.Instagram.Infrastructure.Migrations
{
    public partial class CreateInstagram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

           
            migrationBuilder.CreateTable(
                name: "InstagramChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChannelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChannelAPI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAccessToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramChannels_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstagramPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Caption = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicationStatus = table.Column<int>(type: "int", nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InstagramChannelId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PostId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    InstagramPostId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramPosts_InstagramChannels_InstagramChannelId",
                        column: x => x.InstagramChannelId,
                        principalTable: "InstagramChannels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstagramPosts_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

           

            migrationBuilder.CreateIndex(
                name: "IX_InstagramChannels_ChannelId",
                table: "InstagramChannels",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramPosts_InstagramChannelId",
                table: "InstagramPosts",
                column: "InstagramChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramPosts_PostId",
                table: "InstagramPosts",
                column: "PostId");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstagramPosts");

            

            migrationBuilder.DropTable(
                name: "InstagramChannels");

        }
    }
}
