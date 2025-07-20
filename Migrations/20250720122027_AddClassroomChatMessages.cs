using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JapaneseLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomChatMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomInstanceId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Message = table.Column<string>(maxLength: 1000, nullable: false),
                    SentAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomChatMessages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomChatMessages_ClassroomInstances_ClassroomInstanceId",
                        column: x => x.ClassroomInstanceId,
                        principalTable: "ClassroomInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomChatMessages_UserId",
                table: "ClassroomChatMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomChatMessages_ClassroomInstanceId",
                table: "ClassroomChatMessages",
                column: "ClassroomInstanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomChatMessages");
        }
    }
}
