using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KallpaNexus_API.Data;
using KallpaNexus_API.Models;


namespace KallpaNexus_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeadsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Leads (Para la pre-inscripción desde el Home)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarLead(Lead lead)
        {
            lead.Date = DateTime.UtcNow;
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Registro exitoso", id = lead.Id });
        }

        // GET: api/Leads (Para tu tabla de administración)
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            return await _context.Leads.OrderByDescending(l => l.Date).ToListAsync();
        }
    }
}
