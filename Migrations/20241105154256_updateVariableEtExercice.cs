using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class updateVariableEtExercice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Increment",
                table: "Variables",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Unite",
                table: "Variables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuteurId",
                table: "Exercices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_AuteurId",
                table: "Exercices",
                column: "AuteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercices_Enseignants_AuteurId",
                table: "Exercices",
                column: "AuteurId",
                principalTable: "Enseignants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercices_Enseignants_AuteurId",
                table: "Exercices");

            migrationBuilder.DropIndex(
                name: "IX_Exercices_AuteurId",
                table: "Exercices");

            migrationBuilder.DropColumn(
                name: "Increment",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "Unite",
                table: "Variables");

            migrationBuilder.AlterColumn<int>(
                name: "AuteurId",
                table: "Exercices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
