using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class TareasController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public TareasController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTAR TAREAS (HU0402)
        public async Task<IActionResult> Index()
        {
            var tareas = await _context.Tareas
                .Include(t => t.Empleado)
                .OrderByDescending(t => t.FechaAsignacion)
                .ToListAsync();

            return View(tareas);
        }

        // GET: CREAR TAREA
        public IActionResult Crear()
        {
            CargarEmpleadosDisponibles();
            return View();
        }


        // POST: CREAR TAREA (HU0402)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Tarea model)
        {
            if (!ModelState.IsValid)
            {
                CargarEmpleadosDisponibles();
                return View(model);
            }

            // Validar disponibilidad del empleado
            var empleado = await _context.Empleados.FindAsync(model.IdEmpleado);
            if (empleado == null || empleado.Disponibilidad != "Disponible")
            {
                ModelState.AddModelError("", "El empleado no está disponible.");
                TempData["Error"] = "El empleado seleccionado no está disponible.";
                CargarEmpleadosDisponibles();
                return View(model);
            }

            // Verificar conflictos de horario
            var conflicto = await _context.Tareas
                .AnyAsync(t => t.IdEmpleado == model.IdEmpleado
                    && t.Estado != "Completada"
                    && t.Estado != "Cancelada"
                    && t.FechaAsignacion.Date == model.FechaAsignacion.Date);

            if (conflicto)
            {
                ModelState.AddModelError("", "El empleado ya tiene una tarea asignada en esta fecha.");
                TempData["Error"] = "El empleado ya tiene una tarea asignada en esta fecha.";
                CargarEmpleadosDisponibles();
                return View(model);
            }

            _context.Tareas.Add(model);
            await _context.SaveChangesAsync();

            // Registrar auditoría
            await RegistrarAuditoria("Tarea", model.IdTarea, "Crear", null,
                $"Tarea asignada a {empleado.Nombre}");

            TempData["Success"] = "Tarea asignada exitosamente a " + empleado.Nombre;
            return RedirectToAction(nameof(Index));
        }

        // GET: EDITAR TAREA
        public async Task<IActionResult> Editar(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            CargarEmpleadosDisponibles();
            return View(tarea);
        }

        // POST: EDITAR TAREA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Tarea model)
        {
            if (!ModelState.IsValid)
            {
                CargarEmpleadosDisponibles();
                return View(model);
            }

            var tareaOriginal = await _context.Tareas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.IdTarea == model.IdTarea);

            _context.Update(model);
            await _context.SaveChangesAsync();

            // Registrar auditoría
            await RegistrarAuditoria("Tarea", model.IdTarea, "Editar",
                $"Estado: {tareaOriginal?.Estado}",
                $"Estado: {model.Estado}");

            TempData["Success"] = "Tarea actualizada exitosamente";
            return RedirectToAction(nameof(Index));
        }

        // GET: ELIMINAR TAREA
        public async Task<IActionResult> Eliminar(int id)
        {
            var tarea = await _context.Tareas
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(t => t.IdTarea == id);

            if (tarea == null) return NotFound();
            return View(tarea);
        }

       
        // POST: ELIMINAR TAREA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                await RegistrarAuditoria("Tarea", id, "Eliminar",
                    $"Tarea eliminada", null);

                TempData["Success"] = "Tarea eliminada exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var tarea = await _context.Tareas
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(t => t.IdTarea == id);

            if (tarea == null) return NotFound();
            return View(tarea);
        }

        // COMBOS
        private void CargarEmpleadosDisponibles()
        {
            ViewBag.Empleados = new SelectList(
                _context.Empleados.Where(e => e.Disponibilidad == "Disponible"),
                "IdEmpleado", "Nombre");
        }

        // AUDITORÍA
        private async Task RegistrarAuditoria(string tipoEntidad, int idEntidad,
            string accion, string? datosAnteriores, string? datosNuevos)
        {
            var auditoria = new AuditoriaRegistro
            {
                TipoEntidad = tipoEntidad,
                IdEntidad = idEntidad,
                Accion = accion,
                UsuarioResponsable = HttpContext.Session.GetString("Usuario") ?? "Sistema",
                DatosAnteriores = datosAnteriores,
                DatosNuevos = datosNuevos
            };

            _context.AuditoriasRegistros.Add(auditoria);
            await _context.SaveChangesAsync();
        }
    }
}