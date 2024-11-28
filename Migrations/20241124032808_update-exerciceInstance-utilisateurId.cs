using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class updateexerciceInstanceutilisateurId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercicesInstance_Etudiants_UtilisateurId",
                table: "ExercicesInstance");

            migrationBuilder.AlterColumn<string>(
                name: "UtilisateurId",
                table: "ExercicesInstance",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EtudiantId",
                table: "ExercicesInstance",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExercicesInstance_EtudiantId",
                table: "ExercicesInstance",
                column: "EtudiantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercicesInstance_AspNetUsers_UtilisateurId",
                table: "ExercicesInstance",
                column: "UtilisateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExercicesInstance_Etudiants_EtudiantId",
                table: "ExercicesInstance",
                column: "EtudiantId",
                principalTable: "Etudiants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercicesInstance_AspNetUsers_UtilisateurId",
                table: "ExercicesInstance");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercicesInstance_Etudiants_EtudiantId",
                table: "ExercicesInstance");

            migrationBuilder.DropIndex(
                name: "IX_ExercicesInstance_EtudiantId",
                table: "ExercicesInstance");

            migrationBuilder.DropColumn(
                name: "EtudiantId",
                table: "ExercicesInstance");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "ExercicesInstance",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercicesInstance_Etudiants_UtilisateurId",
                table: "ExercicesInstance",
                column: "UtilisateurId",
                principalTable: "Etudiants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
