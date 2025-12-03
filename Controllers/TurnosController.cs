using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class TurnosController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public TurnosController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // ==========================
        // LISTAR
        // ==========================
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .OrderByDescending(t => t.Ftrn)
                .ToListAsync();

            return View(turnos);
        }

        // ==========================
        // DETALLES
        // ==========================
        public async Task<IActionResult> Details(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(t => t.Ctur == id);

            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // ==========================
        // CREAR GET
        // ==========================
        public IActionResult Crear()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            return View();
        }

        // ==========================
        // CREAR POST
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Turno model)
        {
            if (ModelState.IsValid)
            {
                model.Estr = "Pendiente";
                _context.Turnos.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            return View(model);
        }

        // ==========================
        // EDITAR GET
        // ==========================
        public async Task<IActionResult> Editar(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            return View(turno);
        }

        // ==========================
        // EDITAR POST (CORREGIDO)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Turno model)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join("; ", ModelState.Values
                                                 .SelectMany(v => v.Errors)
                                                 .Select(e => e.ErrorMessage));
                return Content("Errores de validación: " + errores);
            }

            var turno = await _context.Turnos.FindAsync(model.Ctur);
            if (turno == null)
                return NotFound();

            turno.Ccli = model.Ccli;
            turno.Ftrn = model.Ftrn;
            turno.Htrn = model.Htrn;
            turno.Cbah = model.Cbah;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // ==========================
        // ASIGNAR EMPLEADO GET
        // ==========================
        public async Task<IActionResult> Asignar(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(t => t.Ctur == id);

            if (turno == null)
                return NotFound();

            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");

            return View(turno);
        }

        // ==========================
        // ASIGNAR EMPLEADO POST
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(int id, int Cemp)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            turno.Cemp = Cemp;
            turno.Estr = "Asignado";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // CAMBIAR ESTADO GET
        // ==========================
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // ==========================
        // CAMBIAR ESTADO POST
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(Turno model)
        {
            var turno = await _context.Turnos.FindAsync(model.Ctur);
            if (turno == null)
                return NotFound();

            turno.Estr = model.Estr;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cancelar(int id)
        {
            var turno = _context.Turnos
                                .Include(t => t.Cliente)
                                .Include(t => t.Empleado)
                                .FirstOrDefault(t => t.Ctur == id);

            if (turno == null)
                return NotFound();

            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            // Liberar empleado si existe
            turno.Cemp = null;
            turno.Estr = "Cancelado";

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
