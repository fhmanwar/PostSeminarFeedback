using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class initTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_M_Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_TypeTraining",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_TypeTraining", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    VerifyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Employee",
                columns: table => new
                {
                    EmpId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NIK = table.Column<string>(nullable: true),
                    AssignmentSite = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Employee", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_TB_M_Employee_TB_M_User_EmpId",
                        column: x => x.EmpId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Training",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Schedule = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Training_TB_M_TypeTraining_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TB_M_TypeTraining",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_M_Training_TB_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_UserRole", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_TB_M_UserRole_TB_M_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TB_M_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_M_UserRole_TB_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionDesc = table.Column<string>(nullable: true),
                    TrainingId = table.Column<int>(nullable: false),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_M_Question_TB_M_Training_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "TB_M_Training",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_Trans_Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Review = table.Column<string>(nullable: true),
                    Rate = table.Column<float>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Trans_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_Trans_Feedback_TB_M_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "TB_M_Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_Trans_Feedback_TB_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Question_TrainingId",
                table: "TB_M_Question",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Training_TypeId",
                table: "TB_M_Training",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Training_UserId",
                table: "TB_M_Training",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_UserRole_RoleId",
                table: "TB_M_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Trans_Feedback_QuestionId",
                table: "TB_Trans_Feedback",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Trans_Feedback_UserId",
                table: "TB_Trans_Feedback",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_M_Employee");

            migrationBuilder.DropTable(
                name: "TB_M_UserRole");

            migrationBuilder.DropTable(
                name: "TB_Trans_Feedback");

            migrationBuilder.DropTable(
                name: "TB_M_Role");

            migrationBuilder.DropTable(
                name: "TB_M_Question");

            migrationBuilder.DropTable(
                name: "TB_M_Training");

            migrationBuilder.DropTable(
                name: "TB_M_TypeTraining");

            migrationBuilder.DropTable(
                name: "TB_M_User");
        }
    }
}
