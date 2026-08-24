using Microsoft.EntityFrameworkCore;
using obrazo.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar o Banco de Dados (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. IMPORTANTE: Adicionar suporte aos Controllers
builder.Services.AddControllers();

// 3. (Opcional) Se quiser usar o Swagger/OpenAPI depois, deixe isso:
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 4. IMPORTANTÍSSIMO: Dizer ao app para escutar as rotas dos Controllers
app.MapControllers();

// (Apague todo o código do WeatherForecast daqui para baixo, ou deixe, mas ele vai responder em /weatherforecast)

app.Run();