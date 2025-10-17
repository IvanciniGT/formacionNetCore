using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ServicioDiccionarios;
using DiccionariosWebApi.DTOs.V1;
using System.ComponentModel.DataAnnotations;

namespace DiccionariosWebApi.Controllers;

[ApiController]
[Route("api/v1/idiomas")]
public class IdiomasController : ControllerBase
{
    private readonly IServicioDiccionarios _servicioDiccionarios;
    private readonly IMapper _mapper;
    private readonly ILogger<IdiomasController> _logger;

    public IdiomasController(
        IServicioDiccionarios servicioDiccionarios,
        IMapper mapper,
        ILogger<IdiomasController> logger)
    {
        _servicioDiccionarios = servicioDiccionarios;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los idiomas disponibles
    /// GET /api/v1/idiomas
    /// </summary>
    [HttpGet]
    public ActionResult<IList<IdiomaRestV1DTO>> GetIdiomas()
    {
        try
        {
            _logger.LogInformation("Obteniendo lista de idiomas disponibles");
            
            var idiomas = _servicioDiccionarios.GetIdiomas();
            var idiomasRest = _mapper.Map<IList<IdiomaRestV1DTO>>(idiomas);
            
            _logger.LogInformation("Devueltos {Count} idiomas", idiomasRest.Count);
            return Ok(idiomasRest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener idiomas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene todos los diccionarios disponibles para un idioma
    /// GET /api/v1/idiomas/{codigoIdioma}/diccionarios
    /// </summary>
    [HttpGet("{codigoIdioma}/diccionarios")]
    public ActionResult<IList<DiccionarioRestV1DTO>> GetDiccionariosPorIdioma(
        [Required] string codigoIdioma)
    {
        try
        {
            _logger.LogInformation("Obteniendo diccionarios para idioma {CodigoIdioma}", codigoIdioma);
            
            var diccionarios = _servicioDiccionarios.GetDiccionarios(codigoIdioma);
            
            if (diccionarios == null)
            {
                _logger.LogWarning("Idioma no encontrado: {CodigoIdioma}", codigoIdioma);
                return NotFound($"Idioma '{codigoIdioma}' no encontrado");
            }

            var diccionariosRest = _mapper.Map<IList<DiccionarioRestV1DTO>>(diccionarios);
            
            _logger.LogInformation("Devueltos {Count} diccionarios para idioma {CodigoIdioma}", 
                diccionariosRest.Count, codigoIdioma);
            return Ok(diccionariosRest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener diccionarios para idioma {CodigoIdioma}", codigoIdioma);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los significados de una palabra en todos los diccionarios de un idioma
    /// GET /api/v1/idiomas/{codigoIdioma}/significados?palabra={palabra}
    /// </summary>
    [HttpGet("{codigoIdioma}/significados")]
    // Ese atributo es el que configura la ruta en la que exponer el método
    public ActionResult<IList<SignificadoRestV1DTO>> GetSignificadosPorIdioma(
        [Required] string codigoIdioma, // El Required es una validación que indica que este parámetro es obligatorio
                                        // Podríamos poner validaciones adicionales, como un rango de longitud mínima/máxima:
                                        // [MinLength(2), MaxLength(5)]
                                        // Incluso podríamos poner validaciones Custom. Podriamos tener funciones que implementan
                                        // la interfaz IValidationAttributeAdapterProvider para hacer validaciones más complejas    
                                        // O una clase con esas validaciones que implementasen IValidationAttribute
                                        // En caso de usar un DTO propio como parametro del body, podríamos hacer quna clase que implementase IValidatableComponent

        [Required][FromQuery] string palabra)
    {
        try
        {
            _logger.LogInformation("Buscando significados de '{Palabra}' en idioma {CodigoIdioma}", 
                palabra, codigoIdioma);
            
            var significados = _servicioDiccionarios.GetSignificadosEnIdioma(codigoIdioma, palabra);
            
            if (significados == null)
            {
                _logger.LogWarning("No se encontraron significados para '{Palabra}' en idioma {CodigoIdioma}", 
                    palabra, codigoIdioma);
                return NotFound($"No se encontraron significados para '{palabra}' en idioma '{codigoIdioma}'");
            }

            var significadosRest = _mapper.Map<IList<SignificadoRestV1DTO>>(significados);
            
            _logger.LogInformation("Devueltos {Count} significados para '{Palabra}' en idioma {CodigoIdioma}", 
                significadosRest.Count, palabra, codigoIdioma);
            return Ok(significadosRest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar significados de '{Palabra}' en idioma {CodigoIdioma}", 
                palabra, codigoIdioma);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica si una palabra existe en algún diccionario del idioma
    /// HEAD /api/v1/idiomas/{codigoIdioma}/existe?palabra={palabra}
    /// </summary>
    [HttpHead("{codigoIdioma}/existe")]
    public ActionResult ExistePalabraEnIdioma(
        [Required] string codigoIdioma,
        [Required] [FromQuery] string palabra)
    {
        try
        {
            _logger.LogDebug("Verificando existencia de '{Palabra}' en idioma {CodigoIdioma}", 
                palabra, codigoIdioma);
            
            var existe = _servicioDiccionarios.ExistePalabraEnIdioma(codigoIdioma, palabra);
            
            if (existe)
            {
                _logger.LogDebug("Palabra '{Palabra}' existe en idioma {CodigoIdioma}", palabra, codigoIdioma);
                return Ok();
            }
            else
            {
                _logger.LogDebug("Palabra '{Palabra}' no existe en idioma {CodigoIdioma}", palabra, codigoIdioma);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de '{Palabra}' en idioma {CodigoIdioma}", 
                palabra, codigoIdioma);
            return StatusCode(500);
        }
    }
}