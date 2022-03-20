using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadioMeti.Persistance.Migrations
{
    public partial class Addmusicvideoprocastentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Djs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramPage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Djs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayListCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PlaysCount = table.Column<int>(type: "int", nullable: false),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Musics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PlaysCount = table.Column<long>(type: "bigint", nullable: false),
                    Poet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Lyrics = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Arrangement = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Photographer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CoverArtist = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusicProducer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LikesCount = table.Column<long>(type: "bigint", nullable: false),
                    IsSingle = table.Column<bool>(type: "bit", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AlbumId = table.Column<long>(type: "bigint", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musics_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArtistAlbums",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<long>(type: "bigint", nullable: false),
                    AlbumId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistAlbums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistAlbums_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistAlbums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prodcasts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PlaysCount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narrator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DjId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodcasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prodcasts_Djs_DjId",
                        column: x => x.DjId,
                        principalTable: "Djs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayListSelectedCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListId = table.Column<long>(type: "bigint", nullable: false),
                    PlayListCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListSelectedCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayListSelectedCategories_PlayListCategories_PlayListCategoryId",
                        column: x => x.PlayListCategoryId,
                        principalTable: "PlayListCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayListSelectedCategories_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistVideos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<long>(type: "bigint", nullable: false),
                    VideoId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistVideos_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistMusics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtistId = table.Column<long>(type: "bigint", nullable: false),
                    MusicId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistMusics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistMusics_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistMusics_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistMusics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MusicId = table.Column<long>(type: "bigint", nullable: false),
                    PlayListId = table.Column<long>(type: "bigint", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistMusics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistMusics_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Musics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistMusics_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAlbums_AlbumId",
                table: "ArtistAlbums",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistAlbums_ArtistId",
                table: "ArtistAlbums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMusics_ArtistId",
                table: "ArtistMusics",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMusics_MusicId",
                table: "ArtistMusics",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistVideos_ArtistId",
                table: "ArtistVideos",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistVideos_VideoId",
                table: "ArtistVideos",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Musics_AlbumId",
                table: "Musics",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMusics_MusicId",
                table: "PlaylistMusics",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistMusics_PlayListId",
                table: "PlaylistMusics",
                column: "PlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListSelectedCategories_PlayListCategoryId",
                table: "PlayListSelectedCategories",
                column: "PlayListCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListSelectedCategories_PlayListId",
                table: "PlayListSelectedCategories",
                column: "PlayListId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodcasts_DjId",
                table: "Prodcasts",
                column: "DjId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistAlbums");

            migrationBuilder.DropTable(
                name: "ArtistMusics");

            migrationBuilder.DropTable(
                name: "ArtistVideos");

            migrationBuilder.DropTable(
                name: "PlaylistMusics");

            migrationBuilder.DropTable(
                name: "PlayListSelectedCategories");

            migrationBuilder.DropTable(
                name: "Prodcasts");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Musics");

            migrationBuilder.DropTable(
                name: "PlayListCategories");

            migrationBuilder.DropTable(
                name: "PlayLists");

            migrationBuilder.DropTable(
                name: "Djs");

            migrationBuilder.DropTable(
                name: "Albums");
        }
    }
}
