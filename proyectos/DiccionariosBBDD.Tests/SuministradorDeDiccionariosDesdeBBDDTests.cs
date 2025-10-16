using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using DiccionariosApi;
using DiccionariosBBDD;
using DiccionariosBBDD.Entities;

namespace DiccionariosBBDD.Tests;

/// <summary>
/// Pruebas TDD para SuministradorDeDiccionariosDesdeBBDD
/// ESTAS PRUEBAS VAN A FALLAR INICIALMENTE - ¡ES LO ESPERADO EN TDD! 🔴
/// </summary>
public class SuministradorDeDiccionariosDesdeBBDDTests : IDisposable
{
    private readonly DiccionariosDbContext _context;
    private readonly ISuministradorDeDiccionarios _suministrador;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuministradorDeDiccionariosDesdeBBDD> _logger;

    public SuministradorDeDiccionariosDesdeBBDDTests()
    {
        // Configurar Entity Framework InMemory para pruebas
        var options = new DbContextOptionsBuilder<DiccionariosDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nueva DB para cada prueba
            .Options;

        _context = new DiccionariosDbContext(options);

        // Configurar IConfiguration fake para pruebas
        var configDict = new Dictionary<string, string>
        {
            {"DiccionariosConfig:ConnectionString", "InMemory"}
        };
        
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict!)
            .Build();

        // Configurar Logger fake
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<SuministradorDeDiccionariosDesdeBBDD>();

        // ¡ESTA LÍNEA VA A FALLAR! - El SuministradorDeDiccionariosDesdeBBDD no existe aún 🔴
        _suministrador = new SuministradorDeDiccionariosDesdeBBDD(_context, _configuration, _logger);

        // Sembrar datos de prueba
        SembrarDatosDePrueba();
    }

    private void SembrarDatosDePrueba()
    {
        // Crear idioma
        var idioma = new IdiomaEntity { Codigo = "ES", Nombre = "Español" };
        _context.Idiomas.Add(idioma); // Internamente esto es lo que se traduce a INSERT
        // INSERT INTO Idiomas (Codigo, Nombre) VALUES ('ES', 'Español');

        // Crear diccionario
        var diccionario = new DiccionarioEntity { IdiomaId = idioma.Id, Nombre = "Diccionario Español" };
        _context.Diccionarios.Add(diccionario);
        // INSERT INTO Diccionarios (IdiomaId, Nombre) VALUES (1, 'Diccionario Español');

        // Crear palabra con significados
        var palabra = new PalabraEntity 
        { 
            Texto = "casa", 
            // TextoNormalizado eliminado - usamos índice funcional UPPER(Texto)
            DiccionarioId = diccionario.Id 
        };
        _context.Palabras.Add(palabra);
        // INSERT INTO Palabras (Texto, DiccionarioId) VALUES ('casa', 1);

        var significado1 = new SignificadoEntity 
        { 
            Texto = "Edificio para habitar", 
            PalabraId = palabra.Id 
        };
        var significado2 = new SignificadoEntity 
        { 
            Texto = "Familia o linaje", 
            PalabraId = palabra.Id 
        };
        
        _context.Significados.AddRange(significado1, significado2);
        
        _context.SaveChanges();
        
        // Crear índices funcionales para aprovechar UPPER() en queries
        _context.CreateFunctionalIndexes();
        
        // Crear índices funcionales para optimización de búsquedas case-insensitive
        _context.CreateFunctionalIndexes();
    }

    public void Dispose()
    {
        _context.Dispose(); // Cerrar contexto y limpiar
    }

    [Fact]
    public void TienesDiccionarioDe_ConIdiomaExistente_DeberiaRetornarTrue()
    {
        // Arrange
        string idioma = "ES";

        // Act
        bool resultado = _suministrador.TienesDiccionarioDe(idioma);

        // Assert
        Assert.True(resultado, "Debería encontrar el diccionario de español");
    }

    [Fact]
    public void TienesDiccionarioDe_ConIdiomaInexistente_DeberiaRetornarFalse()
    {
        // Arrange
        string idioma = "FR";

        // Act
        bool resultado = _suministrador.TienesDiccionarioDe(idioma);

        // Assert
        Assert.False(resultado, "No debería encontrar el diccionario de francés");
    }

    [Fact]
    public void DameDiccionarioDe_ConIdiomaExistente_DeberiaRetornarDiccionario()
    {
        // Arrange
        string idioma = "ES";

        // Act
        var diccionario = _suministrador.DameDiccionarioDe(idioma);

        // Assert
        Assert.NotNull(diccionario);
        Assert.Equal("ES", diccionario.Idioma);
    }

    [Fact]
    public void DameDiccionarioDe_ConIdiomaInexistente_DeberiaRetornarNull()
    {
        // Arrange
        string idioma = "FR";

        // Act
        var diccionario = _suministrador.DameDiccionarioDe(idioma);

        // Assert
        Assert.Null(diccionario);
    }

    [Fact]
    public void DiccionarioRetornado_DeberiaImplementarInterfazCorrectamente()
    {
        // Arrange
        string idioma = "ES";
        var diccionario = _suministrador.DameDiccionarioDe(idioma);

        // Act & Assert
        Assert.NotNull(diccionario);
        
        // Probar que existe una palabra
        bool existeCasa = diccionario.Existe("casa");
        Assert.True(existeCasa, "La palabra 'casa' debería existir");

        // Probar que no existe una palabra inexistente
        bool existeInexistente = diccionario.Existe("palabrainexistente");
        Assert.False(existeInexistente, "Una palabra inexistente no debería existir");

        // Probar obtener significados
        var significados = diccionario.GetSignificados("casa");
        Assert.NotNull(significados);
        Assert.Equal(2, significados.Count);
        Assert.Contains("Edificio para habitar", significados);
        Assert.Contains("Familia o linaje", significados);

        // Probar obtener significados de palabra inexistente
        var significadosInexistentes = diccionario.GetSignificados("palabrainexistente");
        Assert.Null(significadosInexistentes);
    }

}