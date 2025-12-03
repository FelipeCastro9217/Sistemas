using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public ClientesController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // LISTAR + BUSCAR CLIENTES (HU0304)
        // ============================================================
        public async Task<IActionResult> Index(string search)
        {
            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.Nombre.Contains(search) ||
                    c.Apellido.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.Telefono.Contains(search));
            }

            ViewBag.Search = search;
            return View(await query.ToListAsync());
        }

        // ============================================================
        // CREAR CLIENTE
        // ============================================================
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // EDITAR CLIENTE
        // ============================================================
        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return NotFound();

            if (!ModelState.IsValid)
                return View(cliente);

            _context.Update(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // ELIMINAR CLIENTE (GET)
        // ============================================================
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // ============================================================
        // ELIMINAR CLIENTE (POST) — 100% FUNCIONAL
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int IdCliente)
        {
            var cliente = await _context.Clientes.FindAsync(IdCliente);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Clientes/Buscar (HU0304)
        public async Task<IActionResult> Buscar(string criterio)
        {
            ViewBag.Criterio = criterio;

            var query = _context.Clientes.AsQueryable();

            if (!string.IsNullOrEmpty(criterio))
            {
                query = query.Where(c =>
                    c.Nombre.Contains(criterio) ||
                    c.Apellido.Contains(criterio) ||
                    c.Email.Contains(criterio) ||
                    c.Telefono.Contains(criterio));
            }

            var lista = await query.OrderBy(c => c.Nombre).ToListAsync();
            return View(lista);
        }

        // GET: Clientes/Historial/5 (HU0305)
        public async Task<IActionResult> Historial(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            var historial = await _context.Servicios
                .Include(s => s.TipoServicio)
                .Include(s => s.Empleado)
                .Where(s => s.IdCliente == id)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            ViewBag.Cliente = cliente;

            return View(historial);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

    }
}
