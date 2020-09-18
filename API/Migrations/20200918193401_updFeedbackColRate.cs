using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updFeedbackColRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "TB_Trans_Feedback",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rate",
                table: "TB_Trans_Feedback",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
