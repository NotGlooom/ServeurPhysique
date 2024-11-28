using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class suppressionquestioninstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReponsesUtilisateur_QuestionsInstance_QuestionInstanceId",
                table: "ReponsesUtilisateur");

            migrationBuilder.DropTable(
                name: "QuestionsInstance");

            migrationBuilder.RenameColumn(
                name: "QuestionInstanceId",
                table: "ReponsesUtilisateur",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReponsesUtilisateur_QuestionInstanceId",
                table: "ReponsesUtilisateur",
                newName: "IX_ReponsesUtilisateur_QuestionId");

            migrationBuilder.AddColumn<int>(
                name: "ExerciceInstanceId",
                table: "ReponsesUtilisateur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReponsesUtilisateur_ExerciceInstanceId",
                table: "ReponsesUtilisateur",
                column: "ExerciceInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReponsesUtilisateur_ExercicesInstance_ExerciceInstanceId",
                table: "ReponsesUtilisateur",
                column: "ExerciceInstanceId",
                principalTable: "ExercicesInstance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReponsesUtilisateur_Questions_QuestionId",
                table: "ReponsesUtilisateur",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReponsesUtilisateur_ExercicesInstance_ExerciceInstanceId",
                table: "ReponsesUtilisateur");

            migrationBuilder.DropForeignKey(
                name: "FK_ReponsesUtilisateur_Questions_QuestionId",
                table: "ReponsesUtilisateur");

            migrationBuilder.DropIndex(
                name: "IX_ReponsesUtilisateur_ExerciceInstanceId",
                table: "ReponsesUtilisateur");

            migrationBuilder.DropColumn(
                name: "ExerciceInstanceId",
                table: "ReponsesUtilisateur");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "ReponsesUtilisateur",
                newName: "QuestionInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReponsesUtilisateur_QuestionId",
                table: "ReponsesUtilisateur",
                newName: "IX_ReponsesUtilisateur_QuestionInstanceId");

            migrationBuilder.CreateTable(
                name: "QuestionsInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciceInstanceId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsInstance_ExercicesInstance_ExerciceInstanceId",
                        column: x => x.ExerciceInstanceId,
                        principalTable: "ExercicesInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsInstance_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsInstance_ExerciceInstanceId",
                table: "QuestionsInstance",
                column: "ExerciceInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsInstance_QuestionId",
                table: "QuestionsInstance",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReponsesUtilisateur_QuestionsInstance_QuestionInstanceId",
                table: "ReponsesUtilisateur",
                column: "QuestionInstanceId",
                principalTable: "QuestionsInstance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
