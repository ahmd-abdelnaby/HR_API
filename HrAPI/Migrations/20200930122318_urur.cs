using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrAPI.Migrations
{
    public partial class urur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(nullable: true),
                    LeaveTypeID = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    AlternativeEmpID = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AlternativeAddress = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_AlternativeEmpID",
                        column: x => x.AlternativeEmpID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeavesTypes_LeaveTypeID",
                        column: x => x.LeaveTypeID,
                        principalTable: "LeavesTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    LeaveRequestID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeaveFiles_LeaveRequests_LeaveRequestID",
                        column: x => x.LeaveRequestID,
                        principalTable: "LeaveRequests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveFiles_LeaveRequestID",
                table: "LeaveFiles",
                column: "LeaveRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_AlternativeEmpID",
                table: "LeaveRequests",
                column: "AlternativeEmpID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeID",
                table: "LeaveRequests",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeID",
                table: "LeaveRequests",
                column: "LeaveTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveFiles");

            migrationBuilder.DropTable(
                name: "LeaveRequests");
        }
    }
}
