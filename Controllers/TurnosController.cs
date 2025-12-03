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

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .OrderByDescending(t => t.Ftrn)
                .ToListAsync();

            return View(turnos);
        }

        // DETALLES
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

        // CREAR GET
        public IActionResult Crear()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            return View();
        }

        // CREAR POST (CORREGIDO)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Turno model)
        {
            // Remover validaciones de navegación
            ModelState.Remove("Cliente");
            ModelState.Remove("Empleado");

            if (ModelState.IsValid)
            {
                model.Estr = "Pendiente";
                _context.Turnos.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Turno registrado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", model.Ccli);
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre", model.Cemp);
            return View(model);
        }

        // EDITAR GET
        public async Task<IActionResult> Editar(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", turno.Ccli);
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre", turno.Cemp);
            return View(turno);
        }

        // EDITAR POST (CORREGIDO)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Turno model)
        {
            ModelState.Remove("Cliente");
            ModelState.Remove("Empleado");

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", model.Ccli);
                ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre", model.Cemp);
                return View(model);
            }

            var turno = await _context.Turnos.FindAsync(model.Ctur);
            if (turno == null)
                return NotFound();

            turno.Ccli = model.Ccli;
            turno.Ftrn = model.Ftrn;
            turno.Htrn = model.Htrn;
            turno.Cbah = model.Cbah;
            turno.Cemp = model.Cemp;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Turno actualizado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // ASIGNAR EMPLEADO GET
        public async Task<IActionResult> Asignar(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(t => t.Ctur == id);

            if (turno == null)
                return NotFound();

            ViewBag.Empleados = new SelectList(_context.Empleados.Where(e => e.Disponibilidad == "Disponible"),
                "IdEmpleado", "Nombre");

            return View(turno);
        }

        // ASIGNAR EMPLEADO POST
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
            TempData["Success"] = "Empleado asignado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // CAMBIAR ESTADO GET
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // CAMBIAR ESTADO POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(Turno model)
        {
            var turno = await _context.Turnos.FindAsync(model.Ctur);
            if (turno == null)
                return NotFound();

            turno.Estr = model.Estr;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Estado actualizado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // CANCELAR GET
        public async Task<IActionResult> Cancelar(int id)
        {
            var turno = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Empleado)
                .FirstOrDefaultAsync(t => t.Ctur == id);

            if (turno == null)
                return NotFound();

            return View(turno);
        }

        // CANCELAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
                return NotFound();

            turno.Cemp = null;
            turno.Estr = "Cancelado";

            await _context.SaveChangesAsync();
            TempData["Success"] = "Turno cancelado exitosamente";

            return RedirectToAction(nameof(Index));
        }
    }
}