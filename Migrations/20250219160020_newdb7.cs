using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AustellAcademyAdmissions.Migrations
{
    /// <inheritdoc />
    public partial class newdb7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 1,
                column: "Teacher",
                value: "Mr. Sangita");

            migrationBuilder.UpdateData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Subject", "Teacher" },
                values: new object[] { "English", "Ms. Bindya" });

            migrationBuilder.InsertData(
                table: "ClassRoutines",
                columns: new[] { "Id", "ClassName", "DayOfWeek", "EndTime", "StartTime", "Subject", "Teacher" },
                values: new object[,]
                {
                    { 3, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "Mathmatics", "Ms. Johnson" },
                    { 4, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "EVS", "Ms. Johnson" },
                    { 5, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "GK", "Ms. Johnson" },
                    { 6, "Nursery", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "EVS", "Ms. Johnson" },
                    { 7, "Nursery", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "GK", "Ms. Johnson" },
                    { 8, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "English", "Mrs. Alpana" },
                    { 9, "Prparatory", "Tuesday", new TimeSpan(0, 11, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0), "Mathmatics", "Mrs. Pollabi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 1,
                column: "Teacher",
                value: "Mr. Smith");

            migrationBuilder.UpdateData(
                table: "ClassRoutines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Subject", "Teacher" },
                values: new object[] { "Science", "Ms. Johnson" });
        }
    }
}
