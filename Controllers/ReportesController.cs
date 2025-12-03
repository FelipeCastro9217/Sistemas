using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public ReportesController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // INDEX - Panel de Reportes
        public IActionResult Index()
        {
            return View();
        }

        // HU0501 - MOSTRAR SERVICIOS
        public async Task<IActionResult> MostrarServicios(string? tipo, string? estado, int pagina = 1)
        {
            int porPagina = 20;
            var query = _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.TipoServicio)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(s => s.TipoServicio.Nombre.Contains(tipo));
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(s => s.ResultadoServicio.Contains(estado));
            }

            var total = await query.CountAsync();
            var servicios = await query
                .OrderByDescending(s => s.Fecha)
                .Skip((pagina - 1) * porPagina)
                .Take(porPagina)
                .ToListAsync();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / porPagina);
            ViewBag.TipoFiltro = tipo;
            ViewBag.EstadoFiltro = estado;

            return View(servicios);
        }

        // HU0502 - CONTAR VEHÍCULOS
        public async Task<IActionResult> ContarVehiculos(DateTime? desde, DateTime? hasta)
        {
            if (!desde.HasValue) desde = DateTime.Today.AddDays(-30);
            if (!hasta.HasValue) hasta = DateTime.Today;

            var servicios = await _context.Servicios
                .Include(s => s.Cliente)
                .Where(s => s.Fecha >= desde && s.Fecha <= hasta)
                .ToListAsync();

            var vehiculosUnicos = servicios
                .Select(s => s.IdCliente)
                .Distinct()
                .Count();

            var totalServicios = servicios.Count;

            var porTipo = servicios
                .GroupBy(s => s.TipoServicio.Nombre)
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .ToList();

            ViewBag.VehiculosUnicos = vehiculosUnicos;
            ViewBag.TotalServicios = totalServicios;
            ViewBag.PorTipo = porTipo;
            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");

            return View();
        }

        // HU0503 - VER INGRESOS
        public async Task<IActionResult> VerIngresos(DateTime? desde, DateTime? hasta)
        {
            if (!desde.HasValue) desde = DateTime.Today.AddMonths(-1);
            if (!hasta.HasValue) hasta = DateTime.Today;

            var servicios = await _context.Servicios
                .Include(s => s.TipoServicio)
                .Where(s => s.Fecha >= desde && s.Fecha <= hasta)
                .ToListAsync();

            var totalIngresos = servicios.Sum(s => s.TipoServicio?.Precio ?? 0);

            var ingresosPorTipo = servicios
                .GroupBy(s => s.TipoServicio.Nombre)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Sum(s => s.TipoServicio.Precio),
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            ViewBag.TotalIngresos = totalIngresos;
            ViewBag.IngresosPorTipo = ingresosPorTipo;
            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");

            return View();
        }

        // HU0504 - CONSULTAR EMPLEADOS
        public async Task<IActionResult> ConsultarEmpleados(string? busqueda)
        {
            var query = _context.Empleados.AsQueryable();

            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(e =>
                    e.Nombre.Contains(busqueda) ||
                    e.Apellido.Contains(busqueda) ||
                    e.Puesto.Contains(busqueda));
            }

            var empleados = await query.OrderBy(e => e.Nombre).ToListAsync();

            // Cargar estadísticas por empleado
            var empleadosConEstadisticas = new List<EmpleadoConEstadisticas>();

            foreach (var emp in empleados)
            {
                var servicios = await _context.Servicios
                    .Where(s => s.IdEmpleado == emp.IdEmpleado)
                    .CountAsync();

                var tareas = await _context.Tareas
                    .Where(t => t.IdEmpleado == emp.IdEmpleado && t.Estado == "Completada")
                    .CountAsync();

                empleadosConEstadisticas.Add(new EmpleadoConEstadisticas
                {
                    Empleado = emp,
                    ServiciosRealizados = servicios,
                    TareasCompletadas = tareas
                });
            }

            ViewBag.Busqueda = busqueda;
            return View(empleadosConEstadisticas);
        }

        // HU0505 - IDENTIFICAR SERVICIOS FRECUENTES
        public async Task<IActionResult> ServiciosFrecuentes(DateTime? desde, DateTime? hasta, int top = 5)
        {
            if (!desde.HasValue) desde = DateTime.Today.AddMonths(-3);
            if (!hasta.HasValue) hasta = DateTime.Today;

            var serviciosFrecuentes = await _context.Servicios
                .Include(s => s.TipoServicio)
                .Where(s => s.Fecha >= desde && s.Fecha <= hasta)
                .GroupBy(s => s.IdTipoServicio)
                .Select(g => new
                {
                    TipoServicio = g.FirstOrDefault().TipoServicio.Nombre,
                    Cantidad = g.Count(),
                    PrecioPromedio = g.Average(s => s.TipoServicio.Precio)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(top)
                .ToListAsync();

            ViewBag.ServiciosFrecuentes = serviciosFrecuentes;
            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");
            ViewBag.Top = top;

            return View();
        }
    }

    public class EmpleadoConEstadisticas
    {
        public Empleado Empleado { get; set; }
        public int ServiciosRealizados { get; set; }
        public int TareasCompletadas { get; set; }
    }
}