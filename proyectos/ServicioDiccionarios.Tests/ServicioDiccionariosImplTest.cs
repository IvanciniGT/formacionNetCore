using AutoMapper;
using DiccionariosApi;
using Microsoft.Extensions.Logging;
using Moq;
using ServicioDiccionarios.DTOs;
using ServicioDiccionarios.Implementacion;
using ServicioDiccionarios.Implementacion.Mappers;

namespace ServicioDiccionarios.Tests;

public class ServicioDiccionariosImplTests
{
    private readonly Mock<ISuministradorDeDiccionarios> _mockSuministrador;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ServicioDiccionariosImpl>> _mockLogger;
    private readonly ServicioDiccionariosImpl _servicio;

    public ServicioDiccionariosImplTests()
    {
        _mockSuministrador = new Mock<ISuministradorDeDiccionarios>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<DiccionariosMapperProfile>());
        _mapper = config.CreateMapper();
        
        _mockLogger = new Mock<ILogger<ServicioDiccionariosImpl>>();
        
        _servicio = new ServicioDiccionariosImpl(_mockSuministrador.Object, _mapper, _mockLogger.Object);
    }

    [Fact]
    public void GetIdiomas_DeberiaRetornarListaDeIdiomasDTO()
    {
        // Arrange
        var mockIdiomas = new List<IIdioma>
        {
            CreateMockIdioma("ES", "Español"),
            CreateMockIdioma("EN", "English")
        };
        _mockSuministrador.Setup(x => x.GetIdiomas()).Returns(mockIdiomas);

        // Act
        var resultado = _servicio.GetIdiomas();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("ES", resultado[0].Codigo);
        Assert.Equal("Español", resultado[0].Nombre);
        Assert.Equal("EN", resultado[1].Codigo);
        Assert.Equal("English", resultado[1].Nombre);
    }

    [Fact]
    public void GetDiccionarios_ConIdiomaValido_DeberiaRetornarDiccionarios()
    {
        // Arrange
        var codigoIdioma = "ES";
        var mockDiccionarios = new List<IDiccionario>
        {
            CreateMockDiccionario("ES_RAE", "ES").Object,
            CreateMockDiccionario("ES_LAROUSSE", "ES").Object
        };
        _mockSuministrador.Setup(x => x.GetDiccionarios(codigoIdioma)).Returns(mockDiccionarios);

        // Act
        var resultado = _servicio.GetDiccionarios(codigoIdioma);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("ES_RAE", resultado[0].Codigo);
        Assert.Equal("ES", resultado[0].Idioma);
    }

    [Fact]
    public void GetDiccionarios_ConIdiomaInvalido_DeberiaRetornarNull()
    {
        // Arrange
        var codigoIdioma = "FR";
        _mockSuministrador.Setup(x => x.GetDiccionarios(codigoIdioma)).Returns((IList<IDiccionario>?)null);

        // Act
        var resultado = _servicio.GetDiccionarios(codigoIdioma);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public void GetDiccionario_ConCodigoValido_DeberiaRetornarDiccionario()
    {
        // Arrange
        var codigoDiccionario = "ES_RAE";
        var mockDiccionario = CreateMockDiccionario(codigoDiccionario, "ES");
        _mockSuministrador.Setup(x => x.GetDiccionarioPorCodigo(codigoDiccionario)).Returns(mockDiccionario.Object);

        // Act
        var resultado = _servicio.GetDiccionario(codigoDiccionario);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(codigoDiccionario, resultado.Codigo);
        Assert.Equal("ES", resultado.Idioma);
    }

    [Fact]
    public void ExistePalabraEnDiccionario_PalabraExiste_DeberiaRetornarTrue()
    {
        // Arrange
        var codigoDiccionario = "ES_RAE";
        var palabra = "casa";
        var mockDiccionario = CreateMockDiccionario(codigoDiccionario, "ES");
        mockDiccionario.Setup(x => x.Existe(palabra)).Returns(true);
        _mockSuministrador.Setup(x => x.GetDiccionarioPorCodigo(codigoDiccionario)).Returns(mockDiccionario.Object);

        // Act
        var resultado = _servicio.ExistePalabraEnDiccionario(codigoDiccionario, palabra);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void ExistePalabraEnIdioma_PalabraExisteEnAlgunDiccionario_DeberiaRetornarTrue()
    {
        // Arrange
        var codigoIdioma = "ES";
        var palabra = "casa";
        var mockDiccionario1 = CreateMockDiccionario("ES_RAE", "ES");
        var mockDiccionario2 = CreateMockDiccionario("ES_LAROUSSE", "ES");
        
        mockDiccionario1.Setup(x => x.Existe(palabra)).Returns(false);
        mockDiccionario2.Setup(x => x.Existe(palabra)).Returns(true);
        
        var mockDiccionarios = new List<IDiccionario> { mockDiccionario1.Object, mockDiccionario2.Object };
        _mockSuministrador.Setup(x => x.GetDiccionarios(codigoIdioma)).Returns(mockDiccionarios);

        // Act
        var resultado = _servicio.ExistePalabraEnIdioma(codigoIdioma, palabra);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void GetSignificadosEnDiccionario_PalabraExiste_DeberiaRetornarSignificados()
    {
        // Arrange
        var codigoDiccionario = "ES_RAE";
        var palabra = "casa";
        var significados = new List<string> { "Edificio para habitar", "Hogar familiar" };
        
        var mockDiccionario = CreateMockDiccionario(codigoDiccionario, "ES");
        mockDiccionario.Setup(x => x.GetSignificados(palabra)).Returns(significados);
        _mockSuministrador.Setup(x => x.GetDiccionarioPorCodigo(codigoDiccionario)).Returns(mockDiccionario.Object);

        // Act
        var resultado = _servicio.GetSignificadosEnDiccionario(codigoDiccionario, palabra);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Edificio para habitar", resultado[0].Texto);
        Assert.Equal(codigoDiccionario, resultado[0].Diccionario);
    }

    [Fact]
    public void GetSignificadosEnIdioma_PalabraExisteEnMultiplesDiccionarios_DeberiaRetornarTodosLosSignificados()
    {
        // Arrange
        var codigoIdioma = "ES";
        var palabra = "casa";
        
        var mockDiccionario1 = CreateMockDiccionario("ES_RAE", "ES");
        var mockDiccionario2 = CreateMockDiccionario("ES_LAROUSSE", "ES");
        
        mockDiccionario1.Setup(x => x.GetSignificados(palabra)).Returns(new List<string> { "Edificio para habitar" });
        mockDiccionario2.Setup(x => x.GetSignificados(palabra)).Returns(new List<string> { "Hogar familiar" });
        
        var mockDiccionarios = new List<IDiccionario> { mockDiccionario1.Object, mockDiccionario2.Object };
        _mockSuministrador.Setup(x => x.GetDiccionarios(codigoIdioma)).Returns(mockDiccionarios);

        // Act
        var resultado = _servicio.GetSignificadosEnIdioma(codigoIdioma, palabra);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, s => s.Diccionario == "ES_RAE" && s.Texto == "Edificio para habitar");
        Assert.Contains(resultado, s => s.Diccionario == "ES_LAROUSSE" && s.Texto == "Hogar familiar");
    }

    private static IIdioma CreateMockIdioma(string codigo, string nombre)
    {
        var mock = new Mock<IIdioma>();
        mock.Setup(x => x.Codigo).Returns(codigo);
        mock.Setup(x => x.Nombre).Returns(nombre);
        return mock.Object;
    }

    private static Mock<IDiccionario> CreateMockDiccionario(string codigo, string idioma)
    {
        var mock = new Mock<IDiccionario>();
        mock.Setup(x => x.Codigo).Returns(codigo);
        mock.Setup(x => x.Idioma).Returns(idioma);
        return mock;
    }
}
