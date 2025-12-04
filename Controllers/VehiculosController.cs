using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public VehiculosController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTAR VEHÍCULOS
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _context.Vehiculos
                .Include(v => v.Cliente)
                .OrderBy(v => v.Placa)
                .ToListAsync();

            return View(vehiculos);
        }

        // GET: CREAR VEHÍCULO
        public IActionResult Crear()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
            return View();
        }

        // POST: CREAR VEHÍCULO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Vehiculo model)
        {
            ModelState.Remove("Cliente");

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
                return View(model);
            }

            // Validar placa duplicada
            var placaExiste = await _context.Vehiculos
                .AnyAsync(v => v.Placa == model.Placa);

            if (placaExiste)
            {
                ModelState.AddModelError("Placa", "Ya existe un vehículo con esta placa.");
                TempData["Error"] = "Ya existe un vehículo con esta placa.";
                ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre");
                return View(model);
            }

            _context.Vehiculos.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Vehículo registrado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // GET: EDITAR VEHÍCULO
        public async Task<IActionResult> Editar(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return NotFound();

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", vehiculo.IdCliente);
            return View(vehiculo);
        }

        // POST: EDITAR VEHÍCULO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Vehiculo model)
        {
            ModelState.Remove("Cliente");

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "Nombre", model.IdCliente);
                return View(model);
            }

            _context.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Vehículo actualizado exitosamente";

            return RedirectToAction(nameof(Index));
        }

        // GET: ELIMINAR VEHÍCULO
        public async Task<IActionResult> Eliminar(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.IdVehiculo == id);

            if (vehiculo == null) return NotFound();
            return View(vehiculo);
        }

        // POST: ELIMINAR VEHÍCULO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vehículo eliminado exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }

        // DETALLES
        public async Task<IActionResult> Details(int id)
        {
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.IdVehiculo == id);

            if (vehiculo == null) return NotFound();
            return View(vehiculo);
        }

        // VEHÍCULOS POR CLIENTE
        public async Task<IActionResult> PorCliente(int idCliente)
        {
            var cliente = await _context.Clientes.FindAsync(idCliente);
            if (cliente == null) return NotFound();

            var vehiculos = await _context.Vehiculos
                .Where(v => v.IdCliente == idCliente)
                .ToListAsync();

            ViewBag.Cliente = cliente;
            return View(vehiculos);
        }
    }
}