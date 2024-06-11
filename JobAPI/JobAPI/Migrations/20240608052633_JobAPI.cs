using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobAPI.Migrations
{
    /// <inheritdoc />
    public partial class JobAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobID);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffID);
                });

            migrationBuilder.CreateTable(
                name: "JobStaffs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    StaffID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStaffs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_JobStaffs_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobStaffs_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "JobID", "Description", "StartDate", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Thiết kế những fontend đẹp và hoành tráng.", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mới làm", "Designer" },
                    { 2, "Làm ra những trang web hiện tại.", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sắp xong", "IT" }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffID", "Email", "Position", "StaffName" },
                values: new object[,]
                {
                    { 1, "thach04@gmail.com", "Hậu Cần", "Nguyễn Hữu Thạch" },
                    { 2, "long04@gmail.com", "Trưởng Phòng", "Trần Kim Long" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobStaffs_JobID",
                table: "JobStaffs",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_JobStaffs_StaffID",
                table: "JobStaffs",
                column: "StaffID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobStaffs");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Staffs");
        }
    }
}
