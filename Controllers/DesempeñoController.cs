using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // REVISAR DESEMPEÑO (HU0404) - Vista resumida
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

     
        // LISTADO DE EVALUACIONES REGISTRADAS
        
        public async Task<IActionResult> Evaluaciones()
        {
            var evaluaciones = await _context.Desempenos
                .Include(d => d.Empleado)
                .OrderByDescending(d => d.FechaEvaluacion)
                .ToListAsync();

            return View(evaluaciones);
        }

       
        // CREAR EVALUACIÓN DE DESEMPEÑO
       
        public IActionResult Crear()
        {
            CargarEmpleados();
            return View(new Desempeno
            {
                FechaEvaluacion = DateTime.Now,
                PeriodoInicio = DateTime.Today.AddMonths(-1),
                PeriodoFin = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Desempeno model)
        {
            ModelState.Remove("Empleado");

            if (!ModelState.IsValid)
            {
                CargarEmpleados();
                return View(model);
            }

            // Calcular servicios completados automáticamente
            var serviciosCompletados = await _context.Servicios
                .Where(s => s.IdEmpleado == model.IdEmpleado
                    && s.Fecha >= model.PeriodoInicio
                    && s.Fecha <= model.PeriodoFin)
                .CountAsync();

            model.ServiciosCompletados = serviciosCompletados;

            _context.Desempenos.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Evaluación de desempeño registrada exitosamente";
            return RedirectToAction(nameof(Evaluaciones));
        }

      
        // EDITAR EVALUACIÓN
       
        public async Task<IActionResult> Editar(int id)
        {
            var desempeno = await _context.Desempenos.FindAsync(id);
            if (desempeno == null) return NotFound();

            CargarEmpleados();
            return View(desempeno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Desempeno model)
        {
            ModelState.Remove("Empleado");

            if (!ModelState.IsValid)
            {
                CargarEmpleados();
                return View(model);
            }

            _context.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Evaluación actualizada exitosamente";
            return RedirectToAction(nameof(Evaluaciones));
        }

        
        // ELIMINAR EVALUACIÓN
        
        public async Task<IActionResult> Eliminar(int id)
        {
            var desempeno = await _context.Desempenos
                .Include(d => d.Empleado)
                .FirstOrDefaultAsync(d => d.IdDesempeno == id);

            if (desempeno == null) return NotFound();
            return View(desempeno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int IdDesempeno)
        {
            var desempeno = await _context.Desempenos.FindAsync(IdDesempeno);
            if (desempeno != null)
            {
                _context.Desempenos.Remove(desempeno);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Evaluación eliminada exitosamente";
            }

            return RedirectToAction(nameof(Evaluaciones));
        }

        // VER EVALUACIÓN INDIVIDUAL
       
        public async Task<IActionResult> VerEvaluacion(int id)
        {
            var desempeno = await _context.Desempenos
                .Include(d => d.Empleado)
                .FirstOrDefaultAsync(d => d.IdDesempeno == id);

            if (desempeno == null) return NotFound();
            return View(desempeno);
        }

       
        // COMBOS
       
        private void CargarEmpleados()
        {
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
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