using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriasRegistros",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoEntidad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdEntidad = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioResponsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DatosAnteriores = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DatosNuevos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriasRegistros", x => x.IdAuditoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Clasificacion = table.Column<string>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Disponibilidad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.IdEmpleado);
                });

            migrationBuilder.CreateTable(
                name: "TipoServicios",
                columns: table => new
                {
                    IdTipoServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServicios", x => x.IdTipoServicio);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Asistencias",
                columns: table => new
                {
                    IdAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraEntrada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraSalida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencias", x => x.IdAsistencia);
                    table.ForeignKey(
                        name: "FK_Asistencias_Empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Desempenos",
                columns: table => new
                {
                    IdDesempeno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    FechaEvaluacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: false),
                    ServiciosCompletados = table.Column<int>(type: "int", nullable: false),
                    TiempoPromedioServicio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PeriodoInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodoFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desempenos", x => x.IdDesempeno);
                    table.ForeignKey(
                        name: "FK_Desempenos_Empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    IdTarea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Prioridad = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.IdTarea);
                    table.ForeignKey(
                        name: "FK_Tareas_Empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Ctur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ccli = table.Column<int>(type: "int", nullable: false),
                    Cemp = table.Column<int>(type: "int", nullable: true),
                    Cbah = table.Column<int>(type: "int", nullable: true),
                    Ftrn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Htrn = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Estr = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Ctur);
                    table.ForeignKey(
                        name: "FK_Turnos_Clientes_Ccli",
                        column: x => x.Ccli,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Turnos_Empleados_Cemp",
                        column: x => x.Cemp,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado");
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    IdServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEmpleado = table.Column<int>(type: "int", nullable: true),
                    IdTipoServicio = table.Column<int>(type: "int", nullable: false),
                    ResultadoServicio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.IdServicio);
                    table.ForeignKey(
                        name: "FK_Servicios_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicios_Empleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado");
                    table.ForeignKey(
                        name: "FK_Servicios_TipoServicios_IdTipoServicio",
                        column: x => x.IdTipoServicio,
                        principalTable: "TipoServicios",
                        principalColumn: "IdTipoServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historiales",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    FechaServicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoServicio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResultadoServicio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historiales", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_Historiales_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historiales_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdServicio = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Servicios_IdServicio",
                        column: x => x.IdServicio,
                        principalTable: "Servicios",
                        principalColumn: "IdServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_IdEmpleado",
                table: "Asistencias",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Desempenos_IdEmpleado",
                table: "Desempenos",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Historiales_IdCliente",
                table: "Historiales",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Historiales_IdVehiculo",
                table: "Historiales",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdCliente",
                table: "Notificaciones",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdServicio",
                table: "Pagos",
                column: "IdServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_IdCliente",
                table: "Servicios",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_IdEmpleado",
                table: "Servicios",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_IdTipoServicio",
                table: "Servicios",
                column: "IdTipoServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_IdEmpleado",
                table: "Tareas",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_Ccli",
                table: "Turnos",
                column: "Ccli");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_Cemp",
                table: "Turnos",
                column: "Cemp");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_IdCliente",
                table: "Vehiculos",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencias");

            migrationBuilder.DropTable(
                name: "AuditoriasRegistros");

            migrationBuilder.DropTable(
                name: "Desempenos");

            migrationBuilder.DropTable(
                name: "Historiales");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "TipoServicios");
        }
    }
}
