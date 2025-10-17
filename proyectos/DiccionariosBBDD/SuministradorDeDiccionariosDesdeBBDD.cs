using DiccionariosApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DiccionariosBBDD.Entities;

namespace DiccionariosBBDD;

public class SuministradorDeDiccionariosDesdeBBDD : ISuministradorDeDiccionarios
{
    private readonly DiccionariosDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuministradorDeDiccionariosDesdeBBDD> _logger;
    private readonly Lazy<Task> _inicializacionLazy;

    public SuministradorDeDiccionariosDesdeBBDD(
        DiccionariosDbContext context, 
        IConfiguration configuration, 
        ILogger<SuministradorDeDiccionariosDesdeBBDD> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Inicialización lazy para no bloquear el constructor
        _inicializacionLazy = new Lazy<Task>(InicializarBaseDeDatosAsync);
        
        _logger.LogInformation("SuministradorDeDiccionariosDesdeBBDD inicializado correctamente");
    }

    private async Task InicializarBaseDeDatosAsync()
    {
        // Usar un logger null para el inicializador para evitar ruido en logs
        var nullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<DatabaseInitializer>.Instance;
        
        var initializer = new DatabaseInitializer(_context, nullLogger);
        await initializer.InicializarSiEsNecesarioAsync();
    }

    private async Task EnsureInitializedAsync()
    {
        await _inicializacionLazy.Value;
    }

    public bool TienesDiccionarioDe(string idioma)
    {
        if (string.IsNullOrEmpty(idioma)) return false;
        
        // Asegurar que la base de datos esté inicializada
        EnsureInitializedAsync().GetAwaiter().GetResult();
        
        var codigoIdioma = idioma.ToUpperInvariant();
        
        // ✅ OPTIMIZACIÓN: Usar UPPER() para aprovechar el índice funcional IX_Idiomas_Codigo_Upper
        // Esto permite búsquedas case-insensitive sin depender de normalización previa
        var existe = _context.Idiomas
            .Any(i => i.Codigo.ToUpper() == codigoIdioma);
        
        _logger.LogDebug("Idioma '{Idioma}' {Estado} en la base de datos", 
            idioma, existe ? "encontrado" : "no encontrado");
            
        return existe;
    }

    public IDiccionario? DameDiccionarioDe(string idioma)
    {
        if (string.IsNullOrEmpty(idioma)) return null;

        // Asegurar que la base de datos esté inicializada
        EnsureInitializedAsync().GetAwaiter().GetResult();

        var codigoIdioma = idioma.ToUpperInvariant();

        var diccionarioEntity = _context.Diccionarios
            // Estamos trabajando con el Entity Framework, y aquí hacemos una consulta a la BBDD
            .Include(d => d.Idioma)  // INNER JOIN con la tabla Idiomas
            .FirstOrDefault(d => d.Idioma.Codigo.ToUpper() == codigoIdioma); // ✅ Usar UPPER() para aprovechar índice funcional

        // ✅ La query SQL generada usa el ÍNDICE FUNCIONAL:
        // SELECT Diccionarios.Id, Diccionarios.Nombre, Diccionarios.IdiomaId, Idiomas.Id, Idiomas.Codigo, Idiomas.Nombre
        // FROM Diccionarios
        // INNER JOIN Idiomas ON Diccionarios.IdiomaId = Idiomas.Id
        // WHERE UPPER(Idiomas.Codigo) = {codigoIdioma}
        // ↑ Usa el índice funcional IX_Idiomas_Codigo_Upper creado con UPPER(Codigo)

        if (diccionarioEntity == null)
        {
            _logger.LogWarning("No se encontró diccionario para el idioma '{Idioma}'", idioma);
            return null;
        }

        _logger.LogInformation("Diccionario encontrado para idioma '{Idioma}': {Nombre}",
            idioma, diccionarioEntity.Nombre);

        // Crear logger específico para el diccionario
        var diccionarioLogger = _logger as ILogger<DiccionarioDesdeBBDD> ??
            Microsoft.Extensions.Logging.Abstractions.NullLogger<DiccionarioDesdeBBDD>.Instance;

        return new DiccionarioDesdeBBDD(_context, diccionarioEntity, diccionarioLogger);
    }

    public IList<IDiccionario>? GetDiccionarios(string codigoIdioma)
    {
        if (string.IsNullOrEmpty(codigoIdioma)) return null;

        // Asegurar que la base de datos esté inicializada
        EnsureInitializedAsync().GetAwaiter().GetResult();

        var codigoIdiomaUpper = codigoIdioma.ToUpperInvariant();

        var diccionariosEntity = _context.Diccionarios
            .Include(d => d.Idioma)
            .Where(d => d.Idioma.Codigo.ToUpper() == codigoIdiomaUpper)
            .ToList();

        if (!diccionariosEntity.Any())
        {
            _logger.LogWarning("No se encontraron diccionarios para el idioma '{Idioma}'", codigoIdioma);
            return null;
        }

        _logger.LogInformation("Encontrados {Count} diccionarios para el idioma '{Idioma}'", 
            diccionariosEntity.Count, codigoIdioma);

        var diccionarios = new List<IDiccionario>();
        foreach (var diccionarioEntity in diccionariosEntity)
        {
            var diccionarioLogger = _logger as ILogger<DiccionarioDesdeBBDD> ??
                Microsoft.Extensions.Logging.Abstractions.NullLogger<DiccionarioDesdeBBDD>.Instance;
            
            diccionarios.Add(new DiccionarioDesdeBBDD(_context, diccionarioEntity, diccionarioLogger));
        }

        return diccionarios;
    }

    public IDiccionario? GetDiccionarioPorCodigo(string codigoDiccionario)
    {
        if (string.IsNullOrEmpty(codigoDiccionario)) return null;

        // Asegurar que la base de datos esté inicializada
        EnsureInitializedAsync().GetAwaiter().GetResult();

        var codigoUpper = codigoDiccionario.ToUpperInvariant();

        var diccionarioEntity = _context.Diccionarios
            .Include(d => d.Idioma)
            .FirstOrDefault(d => d.Codigo.ToUpper() == codigoUpper);

        if (diccionarioEntity == null)
        {
            _logger.LogWarning("No se encontró diccionario con código '{Codigo}'", codigoDiccionario);
            return null;
        }

        _logger.LogInformation("Diccionario encontrado con código '{Codigo}': {Nombre}",
            codigoDiccionario, diccionarioEntity.Nombre);

        var diccionarioLogger = _logger as ILogger<DiccionarioDesdeBBDD> ??
            Microsoft.Extensions.Logging.Abstractions.NullLogger<DiccionarioDesdeBBDD>.Instance;

        return new DiccionarioDesdeBBDD(_context, diccionarioEntity, diccionarioLogger);
    }

    public IList<IIdioma> GetIdiomas()
    {
        // Asegurar que la base de datos esté inicializada
        EnsureInitializedAsync().GetAwaiter().GetResult();

        var idiomasEntity = _context.Idiomas.ToList();

        if (!idiomasEntity.Any())
        {
            _logger.LogWarning("No se encontraron idiomas en la base de datos");
            return new List<IIdioma>();
        }

        var idiomas = new List<IIdioma>();
        foreach (var idiomaEntity in idiomasEntity)
        {
            idiomas.Add(new IdiomaDesdeBBDD(idiomaEntity));
        }

        _logger.LogInformation("Retornando {Count} idiomas disponibles", idiomas.Count);
        
        return idiomas;
    }
}