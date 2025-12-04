using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;
using System.Text.Json;

namespace Sistemas.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public ServiciosController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTAR SERVICIOS (HU0204)
        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.TipoServicio)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            return View(servicios);
        }

        // GET: CREAR SERVICIO
        public IActionResult Crear()
        {
            CargarCombos();
            return View();
        }

        // POST: CREAR SERVICIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Servicio model)
        {
            if (string.IsNullOrWhiteSpace(model.ResultadoServicio))
                model.ResultadoServicio = "Sin resultado";

            if (string.IsNullOrWhiteSpace(model.Observaciones))
                model.Observaciones = "Sin observaciones";

            if (ModelState.IsValid)
            {
                _context.Servicios.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos();
            return View(model);
        }

        // GET: EDITAR SERVICIO
        public async Task<IActionResult> Editar(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();

            CargarCombos();
            return View(servicio);
        }

        // POST: EDITAR SERVICIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Servicio model)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos();
                return View(model);
            }

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ELIMINAR SERVICIO
        public async Task<IActionResult> Eliminar(int id)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(s => s.IdServicio == id);

            if (servicio == null) return NotFound();
            return View(servicio);
        }

        // POST: ELIMINAR SERVICIO
        [HttpPost]
        [ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Servicio eliminado exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }
        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var servicio = await _context.Servicios
                .Include(s => s.Cliente)
                .Include(s => s.Empleado)
                .Include(s => s.TipoServicio)
                .FirstOrDefaultAsync(s => s.IdServicio == id);

            if (servicio == null) return NotFound();
            return View(servicio);
        }

        // RESUMEN
        public async Task<IActionResult> Resumen()
        {
            var resumen = await _context.Servicios
                .Include(s => s.TipoServicio)
                .GroupBy(s => s.TipoServicio.Nombre)
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            ViewBag.Resumen = resumen;

            ViewBag.Labels = JsonSerializer.Serialize(resumen.Select(r => r.Tipo));
            ViewBag.Data = JsonSerializer.Serialize(resumen.Select(r => r.Cantidad));

            return View(await _context.Servicios
                .Include(s => s.TipoServicio)
                .ToListAsync());
        }

        // COMBOS
        private void CargarCombos()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            ViewBag.Tipos = new SelectList(_context.TipoServicios, "IdTipoServicio", "Nombre");
        }
    }
}
