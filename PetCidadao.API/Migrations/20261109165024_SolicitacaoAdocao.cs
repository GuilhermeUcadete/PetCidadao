using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCidadao.API.Migrations
{
    /// <inheritdoc />
    public partial class SolicitacaoAdocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitacoesAdocao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdocaoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesAdocao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitacoesAdocao_Adocoes_AdocaoId",
                        column: x => x.AdocaoId,
                        principalTable: "Adocoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitacoesAdocao_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesAdocao_AdocaoId",
                table: "SolicitacoesAdocao",
                column: "AdocaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesAdocao_UsuarioId",
                table: "SolicitacoesAdocao",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacoesAdocao");
        }
    }
}
