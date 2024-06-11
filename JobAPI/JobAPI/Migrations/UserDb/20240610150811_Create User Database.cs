using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobAPI.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class CreateUserDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "004c7e80-7dfc-44be-8952-2c7130898655", "004c7e80-7dfc-44be-8952-2c7130898655", "Read", "READ" },
                    { "71e282d3-76ca-485e-b094-eff019287fa5", "71e282d3-76ca-485e-b094-eff019287fa5", "Write", "WRITE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "004c7e80-7dfc-44be-8952-2c7130898655");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71e282d3-76ca-485e-b094-eff019287fa5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "1", "User", "USER" },
                    { "2", "2", "Admin", "ADMIN" }
                });
        }
    }
}
