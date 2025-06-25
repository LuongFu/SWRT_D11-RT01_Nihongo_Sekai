using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JapaneseLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class InitializeClassroomEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassroomTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomTemplates_AspNetUsers_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    GoogleMeetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomInstances_ClassroomTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ClassroomTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasLeft = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomEnrollments_AspNetUsers_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomEnrollments_ClassroomInstances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "ClassroomInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstanceId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomEvaluations_AspNetUsers_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomEvaluations_ClassroomInstances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "ClassroomInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomInstanceId = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalAssessments_ClassroomInstances_ClassroomInstanceId",
                        column: x => x.ClassroomInstanceId,
                        principalTable: "ClassroomInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalAssessmentId = table.Column<int>(type: "int", nullable: false),
                    LearnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_AspNetUsers_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentSubmissions_FinalAssessments_FinalAssessmentId",
                        column: x => x.FinalAssessmentId,
                        principalTable: "FinalAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_FinalAssessmentId",
                table: "AssessmentSubmissions",
                column: "FinalAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_LearnerId",
                table: "AssessmentSubmissions",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEnrollments_InstanceId",
                table: "ClassroomEnrollments",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEnrollments_LearnerId",
                table: "ClassroomEnrollments",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEvaluations_InstanceId",
                table: "ClassroomEvaluations",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomEvaluations_LearnerId",
                table: "ClassroomEvaluations",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomInstances_TemplateId",
                table: "ClassroomInstances",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomTemplates_PartnerId",
                table: "ClassroomTemplates",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAssessments_ClassroomInstanceId",
                table: "FinalAssessments",
                column: "ClassroomInstanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentSubmissions");

            migrationBuilder.DropTable(
                name: "ClassroomEnrollments");

            migrationBuilder.DropTable(
                name: "ClassroomEvaluations");

            migrationBuilder.DropTable(
                name: "FinalAssessments");

            migrationBuilder.DropTable(
                name: "ClassroomInstances");

            migrationBuilder.DropTable(
                name: "ClassroomTemplates");
        }
    }
}
