using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DiccionariosBBDD.Entities;

namespace DiccionariosBBDD;

/// <summary>
/// Servicio encargado de inicializar la base de datos con datos de ejemplo
/// </summary>
public class DatabaseInitializer
{
    private readonly DiccionariosDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(DiccionariosDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Inicializa la base de datos con datos de ejemplo si no existen
    /// </summary>
    public async Task InicializarSiEsNecesarioAsync()
    {
        try
        {
            _logger.LogDebug("Verificando estado de la base de datos...");
            
            // Crear la base de datos si no existe
            await _context.Database.EnsureCreatedAsync();
            
            // Crear índices funcionales para optimización
            _context.CreateFunctionalIndexes();
            
            // Verificar si ya hay datos
            if (await _context.Idiomas.AnyAsync())
            {
                _logger.LogDebug("La base de datos ya contiene datos");
                return;
            }
            
            _logger.LogInformation("Base de datos vacía, inicializando con datos de ejemplo...");
            
            await CrearDatosDeEjemploAsync();
            
            var totalIdiomas = await _context.Idiomas.CountAsync();
            var totalDiccionarios = await _context.Diccionarios.CountAsync();
            var totalPalabras = await _context.Palabras.CountAsync();
            var totalSignificados = await _context.Significados.CountAsync();
            
            _logger.LogInformation("Base de datos inicializada exitosamente con {IdiomasCount} idiomas, {DiccionariosCount} diccionarios, {PalabrasCount} palabras y {SignificadosCount} significados", 
                totalIdiomas, totalDiccionarios, totalPalabras, totalSignificados);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar la base de datos");
            throw;
        }
    }

    private async Task CrearDatosDeEjemploAsync()
    {
        // Crear idiomas
        var idiomas = new[]
        {
            new IdiomaEntity { Codigo = "ES", Nombre = "Español" },
            new IdiomaEntity { Codigo = "EN", Nombre = "English" },
            new IdiomaEntity { Codigo = "FR", Nombre = "Français" }
        };
        
        await _context.Idiomas.AddRangeAsync(idiomas);
        await _context.SaveChangesAsync();
        
        // Crear diccionarios
        var diccionarios = new[]
        {
            new DiccionarioEntity { IdiomaId = idiomas[0].Id, Nombre = "Diccionario Español" },
            new DiccionarioEntity { IdiomaId = idiomas[1].Id, Nombre = "English Dictionary" },
            new DiccionarioEntity { IdiomaId = idiomas[2].Id, Nombre = "Dictionnaire Français" }
        };
        
        await _context.Diccionarios.AddRangeAsync(diccionarios);
        await _context.SaveChangesAsync();
        
        // Crear palabras de ejemplo en español
        var palabrasEspanol = new[]
        {
            new PalabraEntity { Texto = "casa", DiccionarioId = diccionarios[0].Id },
            new PalabraEntity { Texto = "perro", DiccionarioId = diccionarios[0].Id },
            new PalabraEntity { Texto = "agua", DiccionarioId = diccionarios[0].Id },
            new PalabraEntity { Texto = "libro", DiccionarioId = diccionarios[0].Id }
        };
        
        await _context.Palabras.AddRangeAsync(palabrasEspanol);
        await _context.SaveChangesAsync();
        
        // Crear palabras de ejemplo en inglés
        var palabrasIngles = new[]
        {
            new PalabraEntity { Texto = "house", DiccionarioId = diccionarios[1].Id },
            new PalabraEntity { Texto = "dog", DiccionarioId = diccionarios[1].Id },
            new PalabraEntity { Texto = "water", DiccionarioId = diccionarios[1].Id }
        };
        
        await _context.Palabras.AddRangeAsync(palabrasIngles);
        await _context.SaveChangesAsync();
        
        // Crear significados para las palabras en español
        var significados = new[]
        {
            // Casa
            new SignificadoEntity { Texto = "Edificio para habitar", PalabraId = palabrasEspanol[0].Id },
            new SignificadoEntity { Texto = "Familia o linaje", PalabraId = palabrasEspanol[0].Id },
            
            // Perro
            new SignificadoEntity { Texto = "Animal doméstico carnívoro", PalabraId = palabrasEspanol[1].Id },
            new SignificadoEntity { Texto = "Persona de mal carácter", PalabraId = palabrasEspanol[1].Id },
            
            // Agua
            new SignificadoEntity { Texto = "Líquido inodoro, incoloro e insípido", PalabraId = palabrasEspanol[2].Id },
            
            // Libro
            new SignificadoEntity { Texto = "Conjunto de hojas de papel impresas", PalabraId = palabrasEspanol[3].Id },
            new SignificadoEntity { Texto = "Obra literaria, científica o de otra índole", PalabraId = palabrasEspanol[3].Id },
            
            // House
            new SignificadoEntity { Texto = "A building for human habitation", PalabraId = palabrasIngles[0].Id },
            
            // Dog
            new SignificadoEntity { Texto = "A domesticated carnivorous mammal", PalabraId = palabrasIngles[1].Id },
            
            // Water
            new SignificadoEntity { Texto = "A colorless, transparent, odorless liquid", PalabraId = palabrasIngles[2].Id }
        };
        
        await _context.Significados.AddRangeAsync(significados);
        await _context.SaveChangesAsync();
    }
}