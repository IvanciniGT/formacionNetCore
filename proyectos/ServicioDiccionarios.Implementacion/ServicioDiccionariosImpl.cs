using AutoMapper;
using DiccionariosApi;
using Microsoft.Extensions.Logging;
using ServicioDiccionarios.DTOs;

namespace ServicioDiccionarios.Implementacion;

/// <summary>
/// Implementación del servicio de diccionarios que encapsula la lógica de negocio
/// y actúa como facade hacia la API de diccionarios
/// </summary>
public class ServicioDiccionariosImpl : IServicioDiccionarios
{
    private readonly ISuministradorDeDiccionarios _suministradorDiccionarios;
    private readonly IMapper _mapper;
    private readonly ILogger<ServicioDiccionariosImpl> _logger;

    public ServicioDiccionariosImpl(
        ISuministradorDeDiccionarios suministradorDiccionarios,
        IMapper mapper,
        ILogger<ServicioDiccionariosImpl> logger)
    {
        _suministradorDiccionarios = suministradorDiccionarios ?? throw new ArgumentNullException(nameof(suministradorDiccionarios));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public List<IdiomaDTO> GetIdiomas()
    {
        _logger.LogInformation("Obteniendo lista de idiomas disponibles");
        
        try
        {
            var idiomas = _suministradorDiccionarios.GetIdiomas();
            var result = _mapper.Map<List<IdiomaDTO>>(idiomas);
            
            _logger.LogInformation("Obtenidos {Count} idiomas disponibles", result.Count);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetIdiomas", success: true)
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de idiomas");
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetIdiomas", success: false, error: ex.Message)
            
            throw;
        }
    }

    public List<DiccionarioDTO>? GetDiccionarios(string codigoIdioma)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma))
        {
            _logger.LogWarning("Código de idioma no puede ser nulo o vacío");
            return null;
        }

        _logger.LogInformation("Obteniendo diccionarios para el idioma {CodigoIdioma}", codigoIdioma);
        
        try
        {
            var diccionarios = _suministradorDiccionarios.GetDiccionarios(codigoIdioma);
            
            if (diccionarios == null)
            {
                _logger.LogWarning("No se encontraron diccionarios para el idioma {CodigoIdioma}", codigoIdioma);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionarios", success: false, detalle: "Idioma no encontrado")
                
                return null;
            }

            var result = _mapper.Map<List<DiccionarioDTO>>(diccionarios);
            
            _logger.LogInformation("Obtenidos {Count} diccionarios para el idioma {CodigoIdioma}", result.Count, codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionarios", success: true, parametros: codigoIdioma)
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener diccionarios para el idioma {CodigoIdioma}", codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionarios", success: false, error: ex.Message)
            
            throw;
        }
    }

    public DiccionarioDTO? GetDiccionario(string codigoDiccionario)
    {
        if (string.IsNullOrWhiteSpace(codigoDiccionario))
        {
            _logger.LogWarning("Código de diccionario no puede ser nulo o vacío");
            return null;
        }

        _logger.LogInformation("Obteniendo diccionario con código {CodigoDiccionario}", codigoDiccionario);
        
        try
        {
            var diccionario = _suministradorDiccionarios.GetDiccionarioPorCodigo(codigoDiccionario);
            
            if (diccionario == null)
            {
                _logger.LogWarning("No se encontró diccionario con código {CodigoDiccionario}", codigoDiccionario);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionario", success: false, detalle: "Diccionario no encontrado")
                
                return null;
            }

            var result = _mapper.Map<DiccionarioDTO>(diccionario);
            
            _logger.LogInformation("Diccionario encontrado: {CodigoDiccionario}", codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionario", success: true, parametros: codigoDiccionario)
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener diccionario con código {CodigoDiccionario}", codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetDiccionario", success: false, error: ex.Message)
            
            throw;
        }
    }

    public List<SignificadoDTO>? GetSignificadosEnDiccionario(string codigoDiccionario, string palabra)
    {
        if (string.IsNullOrWhiteSpace(codigoDiccionario) || string.IsNullOrWhiteSpace(palabra))
        {
            _logger.LogWarning("Código de diccionario y palabra no pueden ser nulos o vacíos");
            return null;
        }

        _logger.LogInformation("Buscando significados de '{Palabra}' en diccionario {CodigoDiccionario}", palabra, codigoDiccionario);
        
        try
        {
            var diccionario = _suministradorDiccionarios.GetDiccionarioPorCodigo(codigoDiccionario);
            
            if (diccionario == null)
            {
                _logger.LogWarning("Diccionario {CodigoDiccionario} no encontrado", codigoDiccionario);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnDiccionario", success: false, detalle: "Diccionario no encontrado")
                
                return null;
            }

            var significados = diccionario.GetSignificados(palabra);
            
            if (significados == null || !significados.Any())
            {
                _logger.LogInformation("No se encontraron significados para '{Palabra}' en {CodigoDiccionario}", palabra, codigoDiccionario);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnDiccionario", success: false, detalle: "Palabra no encontrada")
                
                return null;
            }

            var result = significados.Select(s => new SignificadoDTO 
            { 
                Texto = s, 
                Diccionario = diccionario.Codigo
            }).ToList();
            
            _logger.LogInformation("Encontrados {Count} significados para '{Palabra}' en {CodigoDiccionario}", result.Count, palabra, codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnDiccionario", success: true, parametros: $"{codigoDiccionario}:{palabra}")
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar significados de '{Palabra}' en diccionario {CodigoDiccionario}", palabra, codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnDiccionario", success: false, error: ex.Message)
            
            throw;
        }
    }

    public List<SignificadoDTO>? GetSignificadosEnIdioma(string codigoIdioma, string palabra)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma) || string.IsNullOrWhiteSpace(palabra))
        {
            _logger.LogWarning("Código de idioma y palabra no pueden ser nulos o vacíos");
            return null;
        }

        _logger.LogInformation("Buscando significados de '{Palabra}' en todos los diccionarios del idioma {CodigoIdioma}", palabra, codigoIdioma);
        
        try
        {
            var diccionarios = _suministradorDiccionarios.GetDiccionarios(codigoIdioma);
            
            if (diccionarios == null || !diccionarios.Any())
            {
                _logger.LogWarning("No se encontraron diccionarios para el idioma {CodigoIdioma}", codigoIdioma);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnIdioma", success: false, detalle: "Idioma no encontrado")
                
                return null;
            }

            var todosLosSignificados = new List<SignificadoDTO>();

            foreach (var diccionario in diccionarios)
            {
                var significados = diccionario.GetSignificados(palabra);
                
                if (significados != null && significados.Any())
                {
                    var significadosDTO = significados.Select(s => new SignificadoDTO 
                    { 
                        Texto = s, 
                        Diccionario = diccionario.Codigo
                    });
                    
                    todosLosSignificados.AddRange(significadosDTO);
                }
            }

            if (!todosLosSignificados.Any())
            {
                _logger.LogInformation("No se encontraron significados para '{Palabra}' en ningún diccionario del idioma {CodigoIdioma}", palabra, codigoIdioma);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnIdioma", success: false, detalle: "Palabra no encontrada")
                
                return null;
            }
            
            _logger.LogInformation("Encontrados {Count} significados para '{Palabra}' en {DiccionariosCount} diccionarios del idioma {CodigoIdioma}", 
                todosLosSignificados.Count, palabra, diccionarios.Count, codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnIdioma", success: true, parametros: $"{codigoIdioma}:{palabra}")
            
            return todosLosSignificados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar significados de '{Palabra}' en idioma {CodigoIdioma}", palabra, codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("GetSignificadosEnIdioma", success: false, error: ex.Message)
            
            throw;
        }
    }

    public bool ExistePalabraEnDiccionario(string codigoDiccionario, string palabra)
    {
        if (string.IsNullOrWhiteSpace(codigoDiccionario) || string.IsNullOrWhiteSpace(palabra))
        {
            _logger.LogWarning("Código de diccionario y palabra no pueden ser nulos o vacíos");
            return false;
        }

        _logger.LogDebug("Verificando existencia de '{Palabra}' en diccionario {CodigoDiccionario}", palabra, codigoDiccionario);
        
        try
        {
            var diccionario = _suministradorDiccionarios.GetDiccionarioPorCodigo(codigoDiccionario);
            
            if (diccionario == null)
            {
                _logger.LogWarning("Diccionario {CodigoDiccionario} no encontrado", codigoDiccionario);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnDiccionario", success: false, detalle: "Diccionario no encontrado")
                
                return false;
            }

            var existe = diccionario.Existe(palabra);
            
            _logger.LogDebug("Palabra '{Palabra}' {Estado} en diccionario {CodigoDiccionario}", 
                palabra, existe ? "existe" : "no existe", codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnDiccionario", success: true, parametros: $"{codigoDiccionario}:{palabra}", resultado: existe)
            
            return existe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de '{Palabra}' en diccionario {CodigoDiccionario}", palabra, codigoDiccionario);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnDiccionario", success: false, error: ex.Message)
            
            throw;
        }
    }

    public bool ExistePalabraEnIdioma(string codigoIdioma, string palabra)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma) || string.IsNullOrWhiteSpace(palabra))
        {
            _logger.LogWarning("Código de idioma y palabra no pueden ser nulos o vacíos");
            return false;
        }

        _logger.LogDebug("Verificando existencia de '{Palabra}' en idioma {CodigoIdioma}", palabra, codigoIdioma);
        
        try
        {
            var diccionarios = _suministradorDiccionarios.GetDiccionarios(codigoIdioma);
            
            if (diccionarios == null || !diccionarios.Any())
            {
                _logger.LogWarning("No se encontraron diccionarios para el idioma {CodigoIdioma}", codigoIdioma);
                
                // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnIdioma", success: false, detalle: "Idioma no encontrado")
                
                return false;
            }

            foreach (var diccionario in diccionarios)
            {
                if (diccionario.Existe(palabra))
                {
                    _logger.LogDebug("Palabra '{Palabra}' encontrada en diccionario {CodigoDiccionario} del idioma {CodigoIdioma}", 
                        palabra, diccionario.Codigo, codigoIdioma);
                    
                    // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnIdioma", success: true, parametros: $"{codigoIdioma}:{palabra}", resultado: true)
                    
                    return true;
                }
            }
            
            _logger.LogDebug("Palabra '{Palabra}' no encontrada en ningún diccionario del idioma {CodigoIdioma}", palabra, codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnIdioma", success: true, parametros: $"{codigoIdioma}:{palabra}", resultado: false)
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de '{Palabra}' en idioma {CodigoIdioma}", palabra, codigoIdioma);
            
            // TODO: -----> ServicioDeMonitoreo.RegistrarConsulta("ExistePalabraEnIdioma", success: false, error: ex.Message)
            
            throw;
        }
    }
}