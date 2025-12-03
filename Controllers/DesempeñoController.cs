using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class DesempeñoController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public DesempeñoController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // REVISAR DESEMPEÑO (HU0404)
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta)
        {
            if (!desde.HasValue) desde = DateTime.Today.AddMonths(-1);
            if (!hasta.HasValue) hasta = DateTime.Today;

            var empleados = await _context.Empleados.ToListAsync();
            var viewModel = new List<DesempenoViewModel>();

            foreach (var empleado in empleados)
            {
                var servicios = await _context.Servicios
                    .Where(s => s.IdEmpleado == empleado.IdEmpleado
                        && s.Fecha >= desde && s.Fecha <= hasta)
                    .ToListAsync();

                var asistencias = await _context.Asistencias
                    .Where(a => a.IdEmpleado == empleado.IdEmpleado
                        && a.Fecha >= desde && a.Fecha <= hasta
                        && a.Estado == "Presente")
                    .CountAsync();

                viewModel.Add(new DesempenoViewModel
                {
                    Empleado = empleado,
                    ServiciosCompletados = servicios.Count,
                    AsistenciasPuntuales = asistencias,
                    PeriodoInicio = desde.Value,
                    PeriodoFin = hasta.Value
                });
            }

            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");

            return View(viewModel);
        }

        // DETALLES DE DESEMPEÑO
        public async Task<IActionResult> Details(int id, DateTime? desde, DateTime? hasta)
        {
            if (!desde.HasValue) desde = DateTime.Today.AddMonths(-1);
            if (!hasta.HasValue) hasta = DateTime.Today;

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            var servicios = await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.TipoServicio)
                .Where(s => s.IdEmpleado == id && s.Fecha >= desde && s.Fecha <= hasta)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            var asistencias = await _context.Asistencias
                .Where(a => a.IdEmpleado == id && a.Fecha >= desde && a.Fecha <= hasta)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            var tareas = await _context.Tareas
                .Where(t => t.IdEmpleado == id && t.FechaAsignacion >= desde)
                .OrderByDescending(t => t.FechaAsignacion)
                .ToListAsync();

            ViewBag.Empleado = empleado;
            ViewBag.Servicios = servicios;
            ViewBag.Asistencias = asistencias;
            ViewBag.Tareas = tareas;
            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");

            return View();
        }
    }

    public class DesempenoViewModel
    {
        public Empleado Empleado { get; set; }
        public int ServiciosCompletados { get; set; }
        public int AsistenciasPuntuales { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
    }
}