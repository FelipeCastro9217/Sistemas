using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public EmpleadosController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTAR EMPLEADOS
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados.ToListAsync();
            return View(empleados);
        }

        // GET: CREAR EMPLEADO
        public IActionResult Crear()
        {
            return View();
        }

        // POST: CREAR EMPLEADO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Empleado model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Disponibilidad = "Disponible";
            _context.Empleados.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Empleado registrado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // GET: EDITAR EMPLEADO
        public async Task<IActionResult> Editar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // POST: EDITAR EMPLEADO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Empleado model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Empleado actualizado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // GET: ELIMINAR EMPLEADO
        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // POST: ELIMINAR EMPLEADO (CORREGIDO)
        [HttpPost]
        [ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Empleado eliminado exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }
    }
}