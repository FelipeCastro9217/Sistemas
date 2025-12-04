using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas.Migrations
{
    /// <inheritdoc />
    public partial class AgregarClasificacionCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Clasificacion",
                table: "Clientes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClasificacionesClientes",
                columns: table => new
                {
                    IdClasificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    TipoClasificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaClasificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiciosRealizados = table.Column<int>(type: "int", nullable: false),
                    TotalGastado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasificacionesClientes", x => x.IdClasificacion);
                    table.ForeignKey(
                        name: "FK_ClasificacionesClientes_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClasificacionesClientes_IdCliente",
                table: "ClasificacionesClientes",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClasificacionesClientes");

            migrationBuilder.DropColumn(
                name: "Clasificacion",
                table: "Clientes");
        }
    }
}
