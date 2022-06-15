using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadioMeti.Persistance.Migrations
{
    public partial class adduserlikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMusicLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusicId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMusicLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMusicLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMusicLikes_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProdcastLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdcastId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProdcastLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProdcastLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProdcastLikes_Prodcasts_ProdcastId",
                        column: x => x.ProdcastId,
                        principalTable: "Prodcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVideoLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVideoLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVideoLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVideoLikes_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMusicLikes_MusicId",
                table: "UserMusicLikes",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMusicLikes_UserId",
                table: "UserMusicLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProdcastLikes_ProdcastId",
                table: "UserProdcastLikes",
                column: "ProdcastId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProdcastLikes_UserId",
                table: "UserProdcastLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVideoLikes_UserId",
                table: "UserVideoLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVideoLikes_VideoId",
                table: "UserVideoLikes",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMusicLikes");

            migrationBuilder.DropTable(
                name: "UserProdcastLikes");

            migrationBuilder.DropTable(
                name: "UserVideoLikes");
        }
    }
}
