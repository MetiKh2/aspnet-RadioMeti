using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadioMeti.Persistance.Migrations
{
    public partial class addlikescounttoprodcasts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Prodcasts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Prodcasts");
        }
    }
}
