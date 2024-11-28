using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class modificationmodelderoulant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReponseDeroulante_Position",
                table: "Reponses",
                newName: "ReponseTroue_PositionTexte");

            migrationBuilder.AddColumn<string>(
                name: "QuestionTroue_ReponseTexte",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionTroue_ReponseTexte",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ReponseTroue_PositionTexte",
                table: "Reponses",
                newName: "ReponseDeroulante_Position");
        }
    }
}
