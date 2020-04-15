using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineChatServer.DataAccess.Migrations
{
    public partial class AddUserModelInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "AboutAs",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Address",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "FirstName",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                "ImageBytes",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ImageName",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "LastName",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "RegisterDate",
                "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "AboutAs",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "Address",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "FirstName",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "ImageBytes",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "ImageName",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "LastName",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "RegisterDate",
                "AspNetUsers");
        }
    }
}