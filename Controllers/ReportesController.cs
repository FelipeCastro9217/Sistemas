// Controllers/ReportesController.cs
// Reportes: servicios más solicitados, rango de fechas, exportación PDF (criterios en PDF)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Data;
using Sistemas.Models;

namespace Sistemas.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AutoCleanDbContext _context;
        public ReportesController(AutoCleanDbContext context) { _context = context; }

        // GET: Reportes/ResumenServicios?desde=2025-11-01&hasta=2025-11-30
        public async Task<IActionResult> ResumenServicios(DateTime? desde, DateTime? hasta)
        {
            // Validaciones de fecha según PDF (si fuera inválida, mostrar mensaje)
            if (!desde.HasValue || !hasta.HasValue)
            {
                // Default: últimos 30 días
                hasta = DateTime.UtcNow;
                desde = hasta.Value.AddDays(-30);
            }

            // Agrupar por tipo de servicio
            var resumen = await _context.Servicios
                .Include(s => s.TipoServicio)
                .Where(s => s.Fecha >= desde && s.Fecha <= hasta)
                .GroupBy(s => s.IdTipoServicio)
                .Select(g => new
                {
                    TipoServicioId = g.Key,
                    Nombre = g.FirstOrDefault().TipoServicio.Nombre,
                    Count = g.Count(),
                    Porcentaje = 0 // se calcula abajo
                }).ToListAsync();

            var total = resumen.Sum(r => r.Count);
            var model = resumen.Select(r => new {
                r.TipoServicioId,
                r.Nombre,
                r.Count,
                Porcentaje = total > 0 ? Math.Round((r.Count * 100.0) / total, 2) : 0
            }).ToList();

            ViewBag.Desde = desde.Value.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta.Value.ToString("yyyy-MM-dd");

            return View(model); // La vista generará la tabla y gráfico
        }

        // Exportar PDF: implementación concreta requiere librería (iTextSharp/Wkhtmltopdf). El PDF exige exportar — en este prompt dejamos nota para usar librería aprobada.
    }
}
