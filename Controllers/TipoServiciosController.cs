using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class TipoServiciosController : Controller
    {
        private readonly AutoCleanDbContext _context;

        public TipoServiciosController(AutoCleanDbContext context)
        {
            _context = context;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoServicios.ToListAsync());
        }

        // CREAR
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(TipoServicio model)
        {
            if (ModelState.IsValid)
            {
                _context.TipoServicios.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // EDITAR
        public async Task<IActionResult> Editar(int id)
        {
            var tipo = await _context.TipoServicios.FindAsync(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(TipoServicio model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // ELIMINAR
        public async Task<IActionResult> Eliminar(int id)
        {
            var tipo = await _context.TipoServicios.FindAsync(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var tipo = await _context.TipoServicios.FindAsync(id);
            if (tipo != null)
            {
                _context.TipoServicios.Remove(tipo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
