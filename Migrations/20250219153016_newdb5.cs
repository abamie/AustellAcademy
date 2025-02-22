using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AustellAcademyAdmissions.Migrations
{
    /// <inheritdoc />
    public partial class newdb5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClassRoutines",
                columns: new[] { "Id", "ClassName", "DayOfWeek", "EndTime", "StartTime", "Subject", "Teacher" },
                values: new object[,]
                {
                    { 1, "Nursery", "Monday", new TimeSpan(0, 10, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), "Mathematics", "Mr. Smith" },
                    { 2, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "Science", "Ms. Johnson" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
