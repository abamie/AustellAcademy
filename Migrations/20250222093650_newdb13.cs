using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AustellAcademyAdmissions.Migrations
{
    /// <inheritdoc />
    public partial class newdb13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Photos");
        }
    }
}
