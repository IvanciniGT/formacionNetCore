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
        
        // Crear múltiples diccionarios por idioma con códigos específicos
        var diccionarios = new[]
        {
            // Diccionarios en Español
            new DiccionarioEntity { IdiomaId = idiomas[0].Id, Nombre = "Diccionario de la Real Academia Española", Codigo = "ES_RAE" },
            new DiccionarioEntity { IdiomaId = idiomas[0].Id, Nombre = "Diccionario Larousse Español", Codigo = "ES_LAROUSSE" },
            
            // Diccionarios en Inglés  
            new DiccionarioEntity { IdiomaId = idiomas[1].Id, Nombre = "Oxford English Dictionary", Codigo = "EN_OXFORD" },
            new DiccionarioEntity { IdiomaId = idiomas[1].Id, Nombre = "Merriam-Webster Dictionary", Codigo = "EN_MERRIAM" },
            
            // Diccionarios en Francés
            new DiccionarioEntity { IdiomaId = idiomas[2].Id, Nombre = "Dictionnaire Larousse", Codigo = "FR_LAROUSSE" },
            new DiccionarioEntity { IdiomaId = idiomas[2].Id, Nombre = "Le Petit Robert", Codigo = "FR_ROBERT" }
        };
        
        await _context.Diccionarios.AddRangeAsync(diccionarios);
        await _context.SaveChangesAsync();
        
        // Crear palabras - Referencias a diccionarios por índice
        var esRae = diccionarios[0];        // ES_RAE
        var esLarousse = diccionarios[1];   // ES_LAROUSSE
        var enOxford = diccionarios[2];     // EN_OXFORD
        var enMerriam = diccionarios[3];    // EN_MERRIAM
        var frLarousse = diccionarios[4];   // FR_LAROUSSE
        var frRobert = diccionarios[5];     // FR_ROBERT
        
        var palabras = new List<PalabraEntity>();
        
        // ===== PALABRAS EN ESPAÑOL =====
        // Palabras COMUNES en ambos diccionarios españoles
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "casa", DiccionarioId = esRae.Id },
            new PalabraEntity { Texto = "casa", DiccionarioId = esLarousse.Id },
            new PalabraEntity { Texto = "agua", DiccionarioId = esRae.Id },
            new PalabraEntity { Texto = "agua", DiccionarioId = esLarousse.Id },
            new PalabraEntity { Texto = "libro", DiccionarioId = esRae.Id },
            new PalabraEntity { Texto = "libro", DiccionarioId = esLarousse.Id }
        });
        
        // Palabras ESPECÍFICAS del RAE
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "hidalgo", DiccionarioId = esRae.Id },
            new PalabraEntity { Texto = "quijote", DiccionarioId = esRae.Id },
            new PalabraEntity { Texto = "castillo", DiccionarioId = esRae.Id }
        });
        
        // Palabras ESPECÍFICAS del Larousse Español
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "boulevard", DiccionarioId = esLarousse.Id },
            new PalabraEntity { Texto = "champán", DiccionarioId = esLarousse.Id },
            new PalabraEntity { Texto = "boutique", DiccionarioId = esLarousse.Id }
        });
        
        // ===== PALABRAS EN INGLÉS =====
        // Palabras COMUNES en ambos diccionarios ingleses
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "house", DiccionarioId = enOxford.Id },
            new PalabraEntity { Texto = "house", DiccionarioId = enMerriam.Id },
            new PalabraEntity { Texto = "water", DiccionarioId = enOxford.Id },
            new PalabraEntity { Texto = "water", DiccionarioId = enMerriam.Id },
            new PalabraEntity { Texto = "book", DiccionarioId = enOxford.Id },
            new PalabraEntity { Texto = "book", DiccionarioId = enMerriam.Id }
        });
        
        // Palabras ESPECÍFICAS del Oxford (británico)
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "posh", DiccionarioId = enOxford.Id },
            new PalabraEntity { Texto = "bloke", DiccionarioId = enOxford.Id },
            new PalabraEntity { Texto = "lorry", DiccionarioId = enOxford.Id }
        });
        
        // Palabras ESPECÍFICAS del Merriam-Webster (americano)
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "elevator", DiccionarioId = enMerriam.Id },
            new PalabraEntity { Texto = "truck", DiccionarioId = enMerriam.Id },
            new PalabraEntity { Texto = "apartment", DiccionarioId = enMerriam.Id }
        });
        
        // ===== PALABRAS EN FRANCÉS =====
        // Palabras COMUNES en ambos diccionarios franceses
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "maison", DiccionarioId = frLarousse.Id },
            new PalabraEntity { Texto = "maison", DiccionarioId = frRobert.Id },
            new PalabraEntity { Texto = "eau", DiccionarioId = frLarousse.Id },
            new PalabraEntity { Texto = "eau", DiccionarioId = frRobert.Id },
            new PalabraEntity { Texto = "livre", DiccionarioId = frLarousse.Id },
            new PalabraEntity { Texto = "livre", DiccionarioId = frRobert.Id }
        });
        
        // Palabras ESPECÍFICAS del Larousse Francés
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "ordinateur", DiccionarioId = frLarousse.Id },
            new PalabraEntity { Texto = "weekend", DiccionarioId = frLarousse.Id },
            new PalabraEntity { Texto = "parking", DiccionarioId = frLarousse.Id }
        });
        
        // Palabras ESPECÍFICAS del Robert
        palabras.AddRange(new[]
        {
            new PalabraEntity { Texto = "élégant", DiccionarioId = frRobert.Id },
            new PalabraEntity { Texto = "raffiné", DiccionarioId = frRobert.Id },
            new PalabraEntity { Texto = "sophistiqué", DiccionarioId = frRobert.Id }
        });
        
        await _context.Palabras.AddRangeAsync(palabras);
        await _context.SaveChangesAsync();
        
        // Crear significados para las palabras - necesitamos encontrar las palabras por texto y diccionario
        var significadosList = new List<SignificadoEntity>();
        
        // ===== SIGNIFICADOS PALABRAS COMUNES ESPAÑOL =====
        var casaRae = palabras.First(p => p.Texto == "casa" && p.DiccionarioId == esRae.Id);
        var casaLarousse = palabras.First(p => p.Texto == "casa" && p.DiccionarioId == esLarousse.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "Edificio para habitar", PalabraId = casaRae.Id },
            new SignificadoEntity { Texto = "Familia o linaje", PalabraId = casaRae.Id },
            new SignificadoEntity { Texto = "Vivienda familiar", PalabraId = casaLarousse.Id },
            new SignificadoEntity { Texto = "Hogar doméstico", PalabraId = casaLarousse.Id }
        });
        
        var aguaRae = palabras.First(p => p.Texto == "agua" && p.DiccionarioId == esRae.Id);
        var aguaLarousse = palabras.First(p => p.Texto == "agua" && p.DiccionarioId == esLarousse.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "Líquido inodoro, incoloro e insípido", PalabraId = aguaRae.Id },
            new SignificadoEntity { Texto = "Sustancia química H2O", PalabraId = aguaLarousse.Id }
        });
        
        var libroRae = palabras.First(p => p.Texto == "libro" && p.DiccionarioId == esRae.Id);
        var libroLarousse = palabras.First(p => p.Texto == "libro" && p.DiccionarioId == esLarousse.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "Conjunto de hojas de papel impresas", PalabraId = libroRae.Id },
            new SignificadoEntity { Texto = "Obra literaria, científica o de otra índole", PalabraId = libroRae.Id },
            new SignificadoEntity { Texto = "Volumen impreso o manuscrito", PalabraId = libroLarousse.Id }
        });
        
        // ===== SIGNIFICADOS PALABRAS ESPECÍFICAS ESPAÑOL =====
        var hidalgo = palabras.First(p => p.Texto == "hidalgo" && p.DiccionarioId == esRae.Id);
        significadosList.Add(new SignificadoEntity { Texto = "Persona de noble linaje", PalabraId = hidalgo.Id });
        
        var champan = palabras.First(p => p.Texto == "champán" && p.DiccionarioId == esLarousse.Id);
        significadosList.Add(new SignificadoEntity { Texto = "Vino espumoso francés", PalabraId = champan.Id });
        
        // ===== SIGNIFICADOS PALABRAS COMUNES INGLÉS =====
        var houseOxford = palabras.First(p => p.Texto == "house" && p.DiccionarioId == enOxford.Id);
        var houseMerriam = palabras.First(p => p.Texto == "house" && p.DiccionarioId == enMerriam.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "A building for human habitation", PalabraId = houseOxford.Id },
            new SignificadoEntity { Texto = "A dwelling place", PalabraId = houseMerriam.Id }
        });
        
        var waterOxford = palabras.First(p => p.Texto == "water" && p.DiccionarioId == enOxford.Id);
        var waterMerriam = palabras.First(p => p.Texto == "water" && p.DiccionarioId == enMerriam.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "A colorless, transparent, odorless liquid", PalabraId = waterOxford.Id },
            new SignificadoEntity { Texto = "The liquid that descends from clouds as rain", PalabraId = waterMerriam.Id }
        });
        
        // ===== SIGNIFICADOS PALABRAS ESPECÍFICAS INGLÉS =====
        var posh = palabras.First(p => p.Texto == "posh" && p.DiccionarioId == enOxford.Id);
        significadosList.Add(new SignificadoEntity { Texto = "Elegant or stylishly luxurious", PalabraId = posh.Id });
        
        var elevator = palabras.First(p => p.Texto == "elevator" && p.DiccionarioId == enMerriam.Id);
        significadosList.Add(new SignificadoEntity { Texto = "A platform or compartment housed in a shaft for raising and lowering people", PalabraId = elevator.Id });
        
        // ===== SIGNIFICADOS PALABRAS COMUNES FRANCÉS =====
        var maisonLarousse = palabras.First(p => p.Texto == "maison" && p.DiccionarioId == frLarousse.Id);
        var maisonRobert = palabras.First(p => p.Texto == "maison" && p.DiccionarioId == frRobert.Id);
        significadosList.AddRange(new[]
        {
            new SignificadoEntity { Texto = "Bâtiment destiné à l'habitation", PalabraId = maisonLarousse.Id },
            new SignificadoEntity { Texto = "Demeure, habitation", PalabraId = maisonRobert.Id }
        });
        
        // ===== SIGNIFICADOS PALABRAS ESPECÍFICAS FRANCÉS =====
        var ordinateur = palabras.First(p => p.Texto == "ordinateur" && p.DiccionarioId == frLarousse.Id);
        significadosList.Add(new SignificadoEntity { Texto = "Machine électronique de traitement numérique de l'information", PalabraId = ordinateur.Id });
        
        var elegant = palabras.First(p => p.Texto == "élégant" && p.DiccionarioId == frRobert.Id);
        significadosList.Add(new SignificadoEntity { Texto = "Qui a de la grâce, de la distinction", PalabraId = elegant.Id });
        
        await _context.Significados.AddRangeAsync(significadosList);
        await _context.SaveChangesAsync();
    }
}