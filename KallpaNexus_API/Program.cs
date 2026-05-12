using KallpaNexus_API.Data;
using Microsoft.EntityFrameworkCore;

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

// Tras UseRouting y antes de UseAuthorization (orden relevante para CORS)
app.UseCors("OpenPolicy");

app.UseAuthorization();

app.MapControllers();

// Aplica migraciones pendientes (crea/actualiza tablas). Preferible a EnsureCreated si hay carpeta Migrations.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.Run();
