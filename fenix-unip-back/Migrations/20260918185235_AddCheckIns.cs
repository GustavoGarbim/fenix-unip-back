using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fenix_unip_back.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckIns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    AdministradorId = table.Column<int>(type: "int", nullable: true),
                    DataHoraCheckIn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckIns_Administradores_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Administradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CheckIns_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckIns_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_AdministradorId",
                table: "CheckIns",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_EventoId",
                table: "CheckIns",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_UsuarioId_EventoId",
                table: "CheckIns",
                columns: new[] { "UsuarioId", "EventoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns");
        }
    }
}
