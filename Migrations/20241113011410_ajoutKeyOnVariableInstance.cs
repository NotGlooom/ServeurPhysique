using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class ajoutKeyOnVariableInstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VariablesInstance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "VariablesInstance");
        }
    }
}
