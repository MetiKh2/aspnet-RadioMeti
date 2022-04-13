using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadioMeti.Persistance.Migrations
{
    public partial class addeventrels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ArtistEvents_ArtistId",
                table: "ArtistEvents",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistEvents_EventId",
                table: "ArtistEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEvents_Artists_ArtistId",
                table: "ArtistEvents",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistEvents_Events_EventId",
                table: "ArtistEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEvents_Artists_ArtistId",
                table: "ArtistEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistEvents_Events_EventId",
                table: "ArtistEvents");

            migrationBuilder.DropIndex(
                name: "IX_ArtistEvents_ArtistId",
                table: "ArtistEvents");

            migrationBuilder.DropIndex(
                name: "IX_ArtistEvents_EventId",
                table: "ArtistEvents");
        }
    }
}
