using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChatServer.DataAccess.Migrations
{
    public partial class AddColumnImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ImageBytes",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "ImageName",
                "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                "ImagePath",
                "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ImagePath",
                "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                "ImageBytes",
                "AspNetUsers",
                "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ImageName",
                "AspNetUsers",
                "text",
                nullable: true);
        }
    }
}