using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KallpaNexus_API.Data;
using KallpaNexus_API.Models;
using Npgsql;

namespace KallpaNexus_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateClubSurveyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PrivateClubSurveyController(AppDbContext context)
        {
            _context = context;
        }

        public class SurveyDto
        {
            public int InterestScore { get; set; }
            public int VisitIntentScore { get; set; }
            public string? Comment { get; set; }
            public string? ContactPhone { get; set; }
            public bool ConfirmedAdult { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] SurveyDto? dto)
        {
            if (dto is null)
                return BadRequest("Cuerpo JSON inválido o vacío.");

            if (!dto.ConfirmedAdult)
                return BadRequest("Debes confirmar que eres mayor de edad.");

            if (dto.InterestScore is < 1 or > 5 || dto.VisitIntentScore is < 1 or > 5)
                return BadRequest("Las valoraciones deben estar entre 1 y 5.");

            var comment = string.IsNullOrWhiteSpace(dto.Comment) ? null : dto.Comment.Trim();
            if (comment != null && comment.Length > 2000)
                comment = comment[..2000];

            var phone = string.IsNullOrWhiteSpace(dto.ContactPhone) ? null : dto.ContactPhone.Trim();
            if (phone != null && phone.Length > 40)
                phone = phone[..40];

            var row = new PrivateClubInterestResponse
            {
                InterestScore = dto.InterestScore,
                VisitIntentScore = dto.VisitIntentScore,
                Comment = comment,
                ContactPhone = phone,
                ConfirmedAdult = true,
                CreatedAtUtc = DateTime.UtcNow,
            };
            _context.PrivateClubInterestResponses.Add(row);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == PostgresErrorCodes.UndefinedTable)
            {
                return StatusCode(
                    503,
                    new
                    {
                        error = "La tabla de encuestas no existe en PostgreSQL.",
                        hint = "En la carpeta del proyecto API ejecuta: dotnet ef database update",
                    });
            }

            return Ok(new { id = row.Id });
        }

        [HttpGet("recent")]
        [Authorize]
        public async Task<IActionResult> Recent([FromQuery] int take = 40)
        {
            take = Math.Clamp(take, 1, 100);
            var list = await _context.PrivateClubInterestResponses
                .OrderByDescending(r => r.CreatedAtUtc)
                .Take(take)
                .Select(r => new
                {
                    r.Id,
                    r.InterestScore,
                    r.VisitIntentScore,
                    r.Comment,
                    r.ContactPhone,
                    r.CreatedAtUtc,
                })
                .ToListAsync();
            return Ok(list);
        }
    }
}
