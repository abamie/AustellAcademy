using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AustellAcademyAdmissions.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentTable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Contents");
        }
    }
}
