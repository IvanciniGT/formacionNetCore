using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DiccionariosAppHost;
using DiccionariosBBDD;

// Crear el Host Builder
var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        // Configurar la lectura de appsettings.json
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Configurar Entity Framework con SQLite
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection") ?? 
                              context.Configuration["DiccionariosConfig:ConnectionString"] ?? 
                              "Data Source=diccionarios.db";
        
        services.AddDbContext<DiccionariosDbContext>(options =>
            options.UseSqlite(connectionString));

        // Auto-registro de dependencias: UI y Suministradores de Diccionarios
        services.AddAutoDiscoveredDependencies();

        // Registrar la aplicación (lógica de negocio) como singleton
        services.AddSingleton<Aplicacion>();
    });

// Construir el host
using var host = hostBuilder.Build();

// Obtener la instancia de la aplicación (lógica de negocio) desde el contenedor de DI
var aplicacion = host.Services.GetRequiredService<Aplicacion>();

// Ejecutar la aplicación
var exitCode = await aplicacion.EjecutarAsync(args);

// Terminar con el código de salida apropiado
Environment.Exit(exitCode);
