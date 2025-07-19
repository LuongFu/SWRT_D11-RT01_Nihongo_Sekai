using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JapaneseLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixReportBinding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videos_Courses");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Courses",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Videos_Courses",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos_Courses", x => new { x.VideoId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_Videos_Courses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Videos_Courses_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_Courses_CourseId",
                table: "Videos_Courses",
                column: "CourseId");
        }
    }
}
