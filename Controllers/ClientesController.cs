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

            // Asignar clasificación inicial
            cliente.Clasificacion = "Nuevo";

            _context.Add(cliente);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cliente registrado exitosamente";

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
            TempData["Success"] = "Cliente actualizado exitosamente";

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
        // ELIMINAR CLIENTE (POST)
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
                TempData["Success"] = "Cliente eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // BUSCAR CLIENTE (HU0304)
        // ============================================================
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

        // ============================================================
        // HISTORIAL (HU0305)
        // ============================================================
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

        // ============================================================
        // CLASIFICAR CLIENTE (NUEVO - HU0306)
        // ============================================================
        public async Task<IActionResult> Clasificar(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            // Calcular estadísticas
            var servicios = await _context.Servicios
                .Include(s => s.TipoServicio)
                .Where(s => s.IdCliente == id)
                .ToListAsync();

            var totalServicios = servicios.Count;
            var totalGastado = servicios.Sum(s => s.TipoServicio?.Precio ?? 0);

            ViewBag.Cliente = cliente;
            ViewBag.TotalServicios = totalServicios;
            ViewBag.TotalGastado = totalGastado;

            // Sugerencia automática
            string sugerencia = "Normal";
            if (totalGastado >= 500000) sugerencia = "VIP";
            else if (totalServicios >= 10) sugerencia = "Frecuente";
            else if (totalServicios == 0) sugerencia = "Nuevo";

            ViewBag.Sugerencia = sugerencia;

            return View(new ClasificacionCliente
            {
                IdCliente = id,
                ServiciosRealizados = totalServicios,
                TotalGastado = totalGastado,
                TipoClasificacion = cliente.Clasificacion ?? sugerencia
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clasificar(ClasificacionCliente model)
        {
            ModelState.Remove("Cliente");

            if (!ModelState.IsValid)
            {
                var cliente = await _context.Clientes.FindAsync(model.IdCliente);
                ViewBag.Cliente = cliente;
                return View(model);
            }

            // Actualizar clasificación en el cliente
            var clienteActualizar = await _context.Clientes.FindAsync(model.IdCliente);
            if (clienteActualizar != null)
            {
                clienteActualizar.Clasificacion = model.TipoClasificacion;
                _context.Update(clienteActualizar);
            }

            // Guardar registro de clasificación
            _context.ClasificacionesClientes.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cliente clasificado exitosamente";
            return RedirectToAction(nameof(Details), new { id = model.IdCliente });
        }

        // ============================================================
        // ENVIAR AVISO (NUEVO - HU0307)
        // ============================================================
        public async Task<IActionResult> EnviarAviso(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            ViewBag.Cliente = cliente;

            return View(new Notificacion
            {
                IdCliente = id,
                Estado = "Pendiente"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarAviso(Notificacion model)
        {
            ModelState.Remove("Cliente");

            if (!ModelState.IsValid)
            {
                var cliente = await _context.Clientes.FindAsync(model.IdCliente);
                ViewBag.Cliente = cliente;
                return View(model);
            }

            model.Fecha = DateTime.UtcNow;
            model.Estado = "Enviado";

            _context.Notificaciones.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Aviso enviado exitosamente al cliente";
            return RedirectToAction(nameof(Details), new { id = model.IdCliente });
        }

        // ============================================================
        // VER NOTIFICACIONES DEL CLIENTE
        // ============================================================
        public async Task<IActionResult> Notificaciones(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            var notificaciones = await _context.Notificaciones
                .Where(n => n.IdCliente == id)
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();

            ViewBag.Cliente = cliente;
            return View(notificaciones);
        }
    }
}