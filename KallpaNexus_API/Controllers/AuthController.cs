using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KallpaNexus_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public record LoginRequest(string Username, string Password);

    /// <summary>Emite JWT para el panel admin (pretotipo). Credenciales en AdminAuth o variables de entorno.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest? body)
    {
        if (body is null || string.IsNullOrWhiteSpace(body.Username) || string.IsNullOrWhiteSpace(body.Password))
            return BadRequest(new { error = "Usuario y contraseña son obligatorios." });

        var expectedUser = _configuration["AdminAuth:Username"] ?? "Benjamin";
        var expectedPass = _configuration["AdminAuth:Password"] ?? "AdminRoot";

        if (!string.Equals(body.Username.Trim(), expectedUser, StringComparison.Ordinal)
            || !string.Equals(body.Password, expectedPass, StringComparison.Ordinal))
            return Unauthorized(new { error = "Credenciales incorrectas." });

        var secret = _configuration["AdminAuth:JwtSigningKey"];
        if (string.IsNullOrEmpty(secret) || secret.Length < 32)
            return StatusCode(500, new { error = "JWT no configurado (AdminAuth:JwtSigningKey, mínimo 32 caracteres)." });

        var issuer = _configuration["AdminAuth:JwtIssuer"] ?? "KallpaNexus";
        var audience = _configuration["AdminAuth:JwtAudience"] ?? "KallpaNexusAdmin";
        var hours = int.TryParse(_configuration["AdminAuth:JwtExpireHours"], out var h) ? Math.Clamp(h, 1, 168) : 8;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(hours);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims: new[] { new Claim(ClaimTypes.Name, expectedUser), new Claim("role", "admin") },
            expires: expires,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = jwt, expiresUtc = expires });
    }
}
