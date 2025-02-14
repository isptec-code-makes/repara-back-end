using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repara.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PecaPedidos_Montagens_MontagemId",
                table: "PecaPedidos");

            migrationBuilder.DropIndex(
                name: "IX_PecaPedidos_MontagemId",
                table: "PecaPedidos");

            migrationBuilder.DropIndex(
                name: "IX_Montagens_EquipamentoId",
                table: "Montagens");

            migrationBuilder.RenameColumn(
                name: "Disponibilidade",
                table: "Funcionarios",
                newName: "Ocupado");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Solicitacoes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEntrega",
                table: "Solicitacoes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "PecaPedidos",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "MontagemId",
                table: "PecaPedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Montagens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Estado",
                table: "Montagens",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Montagens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PecaId",
                table: "Montagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaPedidoId",
                table: "Montagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PecaPedidoId2",
                table: "Montagens",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Diagnosticos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "Estado",
                table: "Diagnosticos",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Diagnosticos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PecaPedidos_MontagemId",
                table: "PecaPedidos",
                column: "MontagemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Montagens_EquipamentoId",
                table: "Montagens",
                column: "EquipamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Montagens_PecaId",
                table: "Montagens",
                column: "PecaId");

            migrationBuilder.CreateIndex(
                name: "IX_Montagens_PecaPedidoId2",
                table: "Montagens",
                column: "PecaPedidoId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Montagens_PecaPedidos_PecaPedidoId2",
                table: "Montagens",
                column: "PecaPedidoId2",
                principalTable: "PecaPedidos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Montagens_Pecas_PecaId",
                table: "Montagens",
                column: "PecaId",
                principalTable: "Pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PecaPedidos_Montagens_MontagemId",
                table: "PecaPedidos",
                column: "MontagemId",
                principalTable: "Montagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Montagens_PecaPedidos_PecaPedidoId2",
                table: "Montagens");

            migrationBuilder.DropForeignKey(
                name: "FK_Montagens_Pecas_PecaId",
                table: "Montagens");

            migrationBuilder.DropForeignKey(
                name: "FK_PecaPedidos_Montagens_MontagemId",
                table: "PecaPedidos");

            migrationBuilder.DropIndex(
                name: "IX_PecaPedidos_MontagemId",
                table: "PecaPedidos");

            migrationBuilder.DropIndex(
                name: "IX_Montagens_EquipamentoId",
                table: "Montagens");

            migrationBuilder.DropIndex(
                name: "IX_Montagens_PecaId",
                table: "Montagens");

            migrationBuilder.DropIndex(
                name: "IX_Montagens_PecaPedidoId2",
                table: "Montagens");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Montagens");

            migrationBuilder.DropColumn(
                name: "PecaId",
                table: "Montagens");

            migrationBuilder.DropColumn(
                name: "PecaPedidoId",
                table: "Montagens");

            migrationBuilder.DropColumn(
                name: "PecaPedidoId2",
                table: "Montagens");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Diagnosticos");

            migrationBuilder.RenameColumn(
                name: "Ocupado",
                table: "Funcionarios",
                newName: "Disponibilidade");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Solicitacoes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEntrega",
                table: "Solicitacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "PecaPedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MontagemId",
                table: "PecaPedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Montagens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Estado",
                table: "Montagens",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Diagnosticos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Estado",
                table: "Diagnosticos",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.CreateIndex(
                name: "IX_PecaPedidos_MontagemId",
                table: "PecaPedidos",
                column: "MontagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Montagens_EquipamentoId",
                table: "Montagens",
                column: "EquipamentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PecaPedidos_Montagens_MontagemId",
                table: "PecaPedidos",
                column: "MontagemId",
                principalTable: "Montagens",
                principalColumn: "Id");
        }
    }
}
