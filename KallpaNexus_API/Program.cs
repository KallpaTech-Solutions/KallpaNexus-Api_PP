using System.Text;
using KallpaNexus_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// 1. Configurar la Base de Datos (Postgres)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. CORS: origen del frontend en Render + Vite en local (WithOrigins no admite * con credenciales)
var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
    ?? new[]
    {
        "https://kallpanexus-ui-pp.onrender.com",
        "https://kallpanexus-ui.onrender.com",
        "http://localhost:5173",
        "http://127.0.0.1:5173",
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenPolicy", b =>
        b.WithOrigins(corsOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var jwtSecret = builder.Configuration["AdminAuth:JwtSigningKey"];
if (string.IsNullOrEmpty(jwtSecret) || jwtSecret.Length < 32)
    throw new InvalidOperationException(
        "AdminAuth:JwtSigningKey es obligatorio y debe tener al menos 32 caracteres (appsettings o variable AdminAuth__JwtSigningKey).");

var jwtIssuer = builder.Configuration["AdminAuth:JwtIssuer"] ?? "KallpaNexus";
var jwtAudience = builder.Configuration["AdminAuth:JwtAudience"] ?? "KallpaNexusAdmin";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KallpaNexus API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT en el encabezado Authorization: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            },
            Array.Empty<string>()
        },
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// En local el API suele ser solo HTTP (p. ej. :5062). HttpsRedirection rompe preflight CORS hacia Vite (:5173).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// Tras UseRouting y antes de UseAuthorization (orden relevante para CORS)
app.UseCors("OpenPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// En producción (p. ej. Render) aplica migraciones al arrancar. En local (Development) hazlo a mano: `dotnet ef database update`.
if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.Run();
