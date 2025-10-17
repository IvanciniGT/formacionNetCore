using DiccionariosApi;
using DiccionariosBBDD;
using ServicioDiccionarios;
using ServicioDiccionarios.Implementacion;
using DiccionariosWebApi.Mappers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configuración de Entity Framework
builder.Services.AddDbContext<DiccionariosDbContext>(options =>
    options.UseSqlite("Data Source=diccionarios.db"));

// Configuración de inyección de dependencias
builder.Services.AddScoped<ISuministradorDeDiccionarios, SuministradorDeDiccionariosDesdeBBDD>();
builder.Services.AddScoped<IServicioDiccionarios, ServicioDiccionariosImpl>();

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(ServicioDiccionarios.Implementacion.Mappers.DiccionariosMapperProfile));
builder.Services.AddAutoMapper(typeof(RestV1MapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Configurar el routing de controladores
app.MapControllers();

app.Run();
