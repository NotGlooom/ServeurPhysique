using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetSynthese.Server.Migrations
{
    /// <inheritdoc />
    public partial class combinedkeyvariableinstance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VariablesInstance",
                table: "VariablesInstance");

            migrationBuilder.DropIndex(
                name: "IX_VariablesInstance_ExerciceInstanceId",
                table: "VariablesInstance");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VariablesInstance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariablesInstance",
                table: "VariablesInstance",
                columns: new[] { "ExerciceInstanceId", "VariableId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VariablesInstance",
                table: "VariablesInstance");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VariablesInstance",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariablesInstance",
                table: "VariablesInstance",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VariablesInstance_ExerciceInstanceId",
                table: "VariablesInstance",
                column: "ExerciceInstanceId");
        }
    }
}
