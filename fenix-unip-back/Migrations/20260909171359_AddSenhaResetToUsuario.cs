using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fenix_unip_back.Migrations
{
    /// <inheritdoc />
    public partial class AddSenhaResetToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SenhaResetExpiraEm",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenhaResetToken",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenhaResetExpiraEm",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SenhaResetToken",
                table: "Usuarios");
        }
    }
}
