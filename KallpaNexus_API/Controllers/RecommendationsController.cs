using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KallpaNexus_API.Data;
using KallpaNexus_API.Models;

namespace KallpaNexus_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecommendationsController(AppDbContext context)
        {
            _context = context;
        }

        public record RecommendationDto(string Message, string? PagePath);

        /// <summary>Guarda una recomendación anónima (sin nombre, correo ni IP obligatoria).</summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecommendationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message) || dto.Message.Length > 2000)
                return BadRequest("El mensaje es obligatorio y no debe superar 2000 caracteres.");

            var path = string.IsNullOrWhiteSpace(dto.PagePath) ? null : dto.PagePath.Trim();
            if (path != null && path.Length > 500)
                path = path[..500];

            var row = new AnonymousRecommendation
            {
                Message = dto.Message.Trim(),
                PagePath = path,
                CreatedAtUtc = DateTime.UtcNow,
            };
            _context.AnonymousRecommendations.Add(row);
            await _context.SaveChangesAsync();
            return Ok(new { id = row.Id });
        }

        /// <summary>Listado reciente para el panel admin (pretotipo sin auth).</summary>
        [HttpGet("recent")]
        public async Task<IActionResult> Recent([FromQuery] int take = 30)
        {
            take = Math.Clamp(take, 1, 100);
            var list = await _context.AnonymousRecommendations
                .OrderByDescending(r => r.CreatedAtUtc)
                .Take(take)
                .Select(r => new { r.Id, r.Message, r.PagePath, r.CreatedAtUtc })
                .ToListAsync();
            return Ok(list);
        }
    }
}
