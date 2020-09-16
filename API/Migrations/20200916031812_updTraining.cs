using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Training_TB_M_User_UserId",
                table: "TB_M_Training");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Training_TB_M_Employee_UserId",
                table: "TB_M_Training",
                column: "UserId",
                principalTable: "TB_M_Employee",
                principalColumn: "EmpId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Training_TB_M_Employee_UserId",
                table: "TB_M_Training");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Training_TB_M_User_UserId",
                table: "TB_M_Training",
                column: "UserId",
                principalTable: "TB_M_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
