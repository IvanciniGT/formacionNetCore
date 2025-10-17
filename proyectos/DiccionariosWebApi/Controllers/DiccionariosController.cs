using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ServicioDiccionarios;
using DiccionariosWebApi.DTOs.V1;
using System.ComponentModel.DataAnnotations;

namespace DiccionariosWebApi.Controllers;

[ApiController]
[Route("api/v1/diccionarios")]
public class DiccionariosController : ControllerBase
{
    private readonly IServicioDiccionarios _servicioDiccionarios;
    private readonly IMapper _mapper;
    private readonly ILogger<DiccionariosController> _logger;

    public DiccionariosController(
        IServicioDiccionarios servicioDiccionarios,
        IMapper mapper,
        ILogger<DiccionariosController> logger)
    {
        _servicioDiccionarios = servicioDiccionarios;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene un diccionario específico por su código
    /// GET /api/v1/diccionarios/{codigoDiccionario}
    /// </summary>
    [HttpGet("{codigoDiccionario}")]
    public ActionResult<DiccionarioRestV1DTO> GetDiccionario(
        [Required] string codigoDiccionario)
    {
        try
        {
            _logger.LogInformation("Obteniendo diccionario {CodigoDiccionario}", codigoDiccionario);
            
            var diccionario = _servicioDiccionarios.GetDiccionario(codigoDiccionario);
            
            if (diccionario == null)
            {
                _logger.LogWarning("Diccionario no encontrado: {CodigoDiccionario}", codigoDiccionario);
                return NotFound($"Diccionario '{codigoDiccionario}' no encontrado");
            }

            var diccionarioRest = _mapper.Map<DiccionarioRestV1DTO>(diccionario);
            
            _logger.LogInformation("Diccionario {CodigoDiccionario} devuelto", codigoDiccionario);
            return Ok(diccionarioRest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener diccionario {CodigoDiccionario}", codigoDiccionario);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los significados de una palabra en un diccionario específico
    /// GET /api/v1/diccionarios/{codigoDiccionario}/significados?palabra={palabra}
    /// </summary>
    [HttpGet("{codigoDiccionario}/significados")]
    public ActionResult<IList<SignificadoRestV1DTO>> GetSignificadosPorDiccionario(
        [Required] string codigoDiccionario,
        [Required] [FromQuery] string palabra)
    {
        try
        {
            _logger.LogInformation("Buscando significados de '{Palabra}' en diccionario {CodigoDiccionario}", 
                palabra, codigoDiccionario);
            
            var significados = _servicioDiccionarios.GetSignificadosEnDiccionario(codigoDiccionario, palabra);
            
            if (significados == null)
            {
                _logger.LogWarning("No se encontraron significados para '{Palabra}' en diccionario {CodigoDiccionario}", 
                    palabra, codigoDiccionario);
                return NotFound($"No se encontraron significados para '{palabra}' en diccionario '{codigoDiccionario}'");
            }

            var significadosRest = _mapper.Map<IList<SignificadoRestV1DTO>>(significados);
            
            _logger.LogInformation("Devueltos {Count} significados para '{Palabra}' en diccionario {CodigoDiccionario}", 
                significadosRest.Count, palabra, codigoDiccionario);
            return Ok(significadosRest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar significados de '{Palabra}' en diccionario {CodigoDiccionario}", 
                palabra, codigoDiccionario);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica si una palabra existe en un diccionario específico
    /// HEAD /api/v1/diccionarios/{codigoDiccionario}/existe?palabra={palabra}
    /// </summary>
    [HttpHead("{codigoDiccionario}/existe")]
    public ActionResult ExistePalabraEnDiccionario(
        [Required] string codigoDiccionario,
        [Required] [FromQuery] string palabra)
    {
        try
        {
            _logger.LogDebug("Verificando existencia de '{Palabra}' en diccionario {CodigoDiccionario}", 
                palabra, codigoDiccionario);
            
            var existe = _servicioDiccionarios.ExistePalabraEnDiccionario(codigoDiccionario, palabra);
            
            if (existe)
            {
                _logger.LogDebug("Palabra '{Palabra}' existe en diccionario {CodigoDiccionario}", 
                    palabra, codigoDiccionario);
                return Ok();
            }
            else
            {
                _logger.LogDebug("Palabra '{Palabra}' no existe en diccionario {CodigoDiccionario}", 
                    palabra, codigoDiccionario);
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de '{Palabra}' en diccionario {CodigoDiccionario}", 
                palabra, codigoDiccionario);
            return StatusCode(500);
        }
    }
}