using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainStAds.Migrations
{
    /// <inheritdoc />
    public partial class AddImageDataToBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Businesses");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Businesses",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Businesses");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
