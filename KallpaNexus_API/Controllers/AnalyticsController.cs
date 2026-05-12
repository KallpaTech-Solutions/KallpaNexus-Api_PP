using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KallpaNexus_API.Data;
using KallpaNexus_API.Models;

namespace KallpaNexus_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Analytics/track (Recibe cada clic o visita)
        [HttpPost("track")]
        [AllowAnonymous]
        public async Task<IActionResult> TrackEvent(AnalyticsEvent ev)
        {
            ev.Timestamp = DateTime.UtcNow;
            _context.AnalyticsEvents.Add(ev);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // --- ENDPOINTS PARA TUS GRÁFICOS ---

        // 1. Resumen por Sector (Para gráfico de Torta/Pie)
        [HttpGet("summary-sectors")]
        [Authorize]
        public async Task<IActionResult> GetSectorSummary()
        {
            var data = await _context.AnalyticsEvents
                .Where(e => e.Sector != null)
                .GroupBy(e => e.Sector)
                .Select(g => new { Sector = g.Key, Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToListAsync();
            return Ok(data);
        }

        // 2. Visitas diarias (Para gráfico de Líneas)
        [HttpGet("daily-visits")]
        [Authorize]
        public async Task<IActionResult> GetDailyVisits()
        {
            var data = await _context.AnalyticsEvents
                .GroupBy(e => e.Timestamp.Date)
                .Select(g => new { Fecha = g.Key.ToString("yyyy-MM-dd"), Visitas = g.Count() })
                .OrderBy(x => x.Fecha)
                .Take(30) // Últimos 30 días
                .ToListAsync();
            return Ok(data);
        }

        // 3. Top botones clickeados (Para tabla de ranking)
        [HttpGet("top-clicks")]
        [Authorize]
        public async Task<IActionResult> GetTopClicks()
        {
            var data = await _context.AnalyticsEvents
                .Where(e => e.EventType == "CLICK")
                .GroupBy(e => e.TargetName)
                .Select(g => new { Elemento = g.Key, Clicks = g.Count() })
                .OrderByDescending(x => x.Clicks)
                .ToListAsync();
            return Ok(data);
        }

        /// <summary>Rutas con más visitas (eventos tipo VISIT; targetName = ruta).</summary>
        [HttpGet("top-pages")]
        [Authorize]
        public async Task<IActionResult> GetTopPages()
        {
            var data = await _context.AnalyticsEvents
                .Where(e => e.EventType == "VISIT" && e.TargetName != null && e.TargetName != "")
                .GroupBy(e => e.TargetName!)
                .Select(g => new { Ruta = g.Key, Visitas = g.Count() })
                .OrderByDescending(x => x.Visitas)
                .Take(25)
                .ToListAsync();
            return Ok(data);
        }
    }
}
