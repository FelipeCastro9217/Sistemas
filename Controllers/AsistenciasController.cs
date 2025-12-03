using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class AsistenciasController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public AsistenciasController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTAR ASISTENCIAS (HU0403)
        public async Task<IActionResult> Index(DateTime? fecha)
        {
            var query = _context.Asistencias.Include(a => a.Empleado).AsQueryable();

            if (fecha.HasValue)
            {
                query = query.Where(a => a.Fecha.Date == fecha.Value.Date);
            }
            else
            {
                query = query.Where(a => a.Fecha.Date == DateTime.Today);
            }

            ViewBag.FechaFiltro = fecha?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
            return View(await query.OrderBy(a => a.Empleado.Nombre).ToListAsync());
        }

        // REGISTRAR ENTRADA (HU0403)
        public async Task<IActionResult> RegistrarEntrada()
        {
            CargarEmpleados();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarEntrada(int IdEmpleado)
        {
            // Verificar que no exista ya una entrada para hoy
            var yaRegistrado = await _context.Asistencias
                .AnyAsync(a => a.IdEmpleado == IdEmpleado
                    && a.Fecha.Date == DateTime.Today
                    && a.HoraEntrada != null);

            if (yaRegistrado)
            {
                TempData["Error"] = "Ya se registró la entrada de este empleado hoy.";
                return RedirectToAction(nameof(Index));
            }

            var asistencia = new Asistencia
            {
                IdEmpleado = IdEmpleado,
                Fecha = DateTime.Today,
                HoraEntrada = DateTime.Now,
                Estado = "Presente"
            };

            _context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Entrada registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // REGISTRAR SALIDA
        public async Task<IActionResult> RegistrarSalida(int id)
        {
            var asistencia = await _context.Asistencias
                .Include(a => a.Empleado)
                .FirstOrDefaultAsync(a => a.IdAsistencia == id);

            if (asistencia == null) return NotFound();
            return View(asistencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarSalidaConfirmada(int IdAsistencia)
        {
            var asistencia = await _context.Asistencias.FindAsync(IdAsistencia);
            if (asistencia != null && asistencia.HoraSalida == null)
            {
                asistencia.HoraSalida = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Salida registrada correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // EDITAR ASISTENCIA (ADMIN)
        public async Task<IActionResult> Editar(int id)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);
            if (asistencia == null) return NotFound();

            CargarEmpleados();
            return View(asistencia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Asistencia model)
        {
            if (!ModelState.IsValid)
            {
                CargarEmpleados();
                return View(model);
            }

            var original = await _context.Asistencias
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAsistencia == model.IdAsistencia);

            _context.Update(model);
            await _context.SaveChangesAsync();

            // Auditoría
            await RegistrarAuditoria("Asistencia", model.IdAsistencia, "Editar",
                $"Estado: {original?.Estado}", $"Estado: {model.Estado}");

            return RedirectToAction(nameof(Index));
        }

        // COMBOS
        private void CargarEmpleados()
        {
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
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