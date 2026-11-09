using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCidadao.API.Migrations
{
    /// <inheritdoc />
    public partial class CorrespondenciaAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorrespondenciasAnimais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OcorrenciaPerdidaId = table.Column<int>(type: "int", nullable: false),
                    OcorrenciaEncontradaId = table.Column<int>(type: "int", nullable: false),
                    PercentualCompatibilidade = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Justificativa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAnalise = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenciasAnimais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrespondenciasAnimais_Ocorrencias_OcorrenciaEncontradaId",
                        column: x => x.OcorrenciaEncontradaId,
                        principalTable: "Ocorrencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorrespondenciasAnimais_Ocorrencias_OcorrenciaPerdidaId",
                        column: x => x.OcorrenciaPerdidaId,
                        principalTable: "Ocorrencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenciasAnimais_OcorrenciaEncontradaId",
                table: "CorrespondenciasAnimais",
                column: "OcorrenciaEncontradaId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenciasAnimais_OcorrenciaPerdidaId",
                table: "CorrespondenciasAnimais",
                column: "OcorrenciaPerdidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrespondenciasAnimais");
        }
    }
}
