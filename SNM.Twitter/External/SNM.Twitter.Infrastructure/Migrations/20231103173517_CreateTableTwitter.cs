using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SNM.Twitter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableTwitter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

          

            migrationBuilder.CreateTable(
                name: "TwitterChannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TwitterTextAPI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TwitterImageAPI = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAccessToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessTokenSecret = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsumerKey = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConsumerSecret = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChannelId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitterChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TwitterPost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Text = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TwitterPostId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TwitterChannelId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PublicationStatus = table.Column<int>(type: "int", nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PostId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitterPost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TwitterPost_TwitterChannel_TwitterChannelId",
                        column: x => x.TwitterChannelId,
                        principalTable: "TwitterChannel",
                        principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

           

            migrationBuilder.CreateIndex(
                name: "IX_TwitterChannel_ChannelId",
                table: "TwitterChannel",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterPost_PostId",
                table: "TwitterPost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterPost_TwitterChannelId",
                table: "TwitterPost",
                column: "TwitterChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropTable(
                name: "TwitterPost");


            migrationBuilder.DropTable(
                name: "TwitterChannel");

         
        }
    }
}
