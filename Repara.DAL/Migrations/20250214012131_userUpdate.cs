using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repara.DAL.Migrations
{
    /// <inheritdoc />
    public partial class userUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_UserId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UserId",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Funcionarios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_UserId",
                table: "Funcionarios",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UserId",
                table: "Clientes",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_UserId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UserId",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Funcionarios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_UserId",
                table: "Funcionarios",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UserId",
                table: "Clientes",
                column: "UserId",
                unique: true);
        }
    }
}
