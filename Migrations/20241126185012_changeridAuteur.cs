using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class changeridAuteur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Enseignants_AuteurId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercices_Enseignants_AuteurId",
                table: "Exercices");

            migrationBuilder.DropIndex(
                name: "IX_Exercices_AuteurId",
                table: "Exercices");

            migrationBuilder.DropIndex(
                name: "IX_Enseignants_UtilisateurId",
                table: "Enseignants");

            migrationBuilder.DropIndex(
                name: "IX_Activities_AuteurId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "AuteurId",
                table: "Exercices");

            migrationBuilder.DropColumn(
                name: "AuteurId",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "AuteurUtilisateurId",
                table: "Exercices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuteurUtilisateurId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Enseignants_UtilisateurId",
                table: "Enseignants",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_AuteurUtilisateurId",
                table: "Exercices",
                column: "AuteurUtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AuteurUtilisateurId",
                table: "Activities",
                column: "AuteurUtilisateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Enseignants_AuteurUtilisateurId",
                table: "Activities",
                column: "AuteurUtilisateurId",
                principalTable: "Enseignants",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercices_Enseignants_AuteurUtilisateurId",
                table: "Exercices",
                column: "AuteurUtilisateurId",
                principalTable: "Enseignants",
                principalColumn: "UtilisateurId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Enseignants_AuteurUtilisateurId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercices_Enseignants_AuteurUtilisateurId",
                table: "Exercices");

            migrationBuilder.DropIndex(
                name: "IX_Exercices_AuteurUtilisateurId",
                table: "Exercices");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Enseignants_UtilisateurId",
                table: "Enseignants");

            migrationBuilder.DropIndex(
                name: "IX_Activities_AuteurUtilisateurId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "AuteurUtilisateurId",
                table: "Exercices");

            migrationBuilder.DropColumn(
                name: "AuteurUtilisateurId",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "AuteurId",
                table: "Exercices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuteurId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercices_AuteurId",
                table: "Exercices",
                column: "AuteurId");

            migrationBuilder.CreateIndex(
                name: "IX_Enseignants_UtilisateurId",
                table: "Enseignants",
                column: "UtilisateurId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AuteurId",
                table: "Activities",
                column: "AuteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Enseignants_AuteurId",
                table: "Activities",
                column: "AuteurId",
                principalTable: "Enseignants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercices_Enseignants_AuteurId",
                table: "Exercices",
                column: "AuteurId",
                principalTable: "Enseignants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
