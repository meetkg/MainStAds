using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainStAds.Migrations
{
    /// <inheritdoc />
    public partial class AddImageDataToBusinesssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Businesses");
        }
    }
}
