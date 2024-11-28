using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class ajoutedatereponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExercicesInstance");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRepondu",
                table: "ReponsesUtilisateur",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRepondu",
                table: "ReponsesUtilisateur");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ExercicesInstance",
                type: "int",
                nullable: true);
        }
    }
}
