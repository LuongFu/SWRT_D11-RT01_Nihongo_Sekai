using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JapaneseLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageLevelToClassroomTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageLevel",
                table: "ClassroomTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ClassroomInstances",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageLevel",
                table: "ClassroomTemplates");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ClassroomInstances",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
