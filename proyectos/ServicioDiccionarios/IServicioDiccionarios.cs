using ServicioDiccionarios.DTOs;

namespace ServicioDiccionarios;

/// <summary>
/// Interfaz del servicio de diccionarios que encapsula la lógica de negocio
/// y proporciona una API estable independiente de cambios en las capas inferiores
/// </summary>
public interface IServicioDiccionarios
{
    /// <summary>
    /// Obtiene todos los idiomas disponibles
    /// </summary>
    /// <returns>Lista de idiomas disponibles</returns>
    List<IdiomaDTO> GetIdiomas();

    /// <summary>
    /// Obtiene todos los diccionarios disponibles para un idioma específico
    /// </summary>
    /// <param name="codigoIdioma">Código del idioma (ej: ES, EN, FR)</param>
    /// <returns>Lista de diccionarios del idioma o null si no existe el idioma</returns>
    List<DiccionarioDTO>? GetDiccionarios(string codigoIdioma);

    /// <summary>
    /// Obtiene un diccionario específico por su código
    /// </summary>
    /// <param name="codigoDiccionario">Código único del diccionario (ej: ES_RAE, EN_OXFORD)</param>
    /// <returns>Diccionario encontrado o null si no existe</returns>
    DiccionarioDTO? GetDiccionario(string codigoDiccionario);

    /// <summary>
    /// Obtiene los significados de una palabra en un diccionario específico
    /// </summary>
    /// <param name="codigoDiccionario">Código del diccionario</param>
    /// <param name="palabra">Palabra a buscar</param>
    /// <returns>Lista de significados o null si no se encuentra la palabra</returns>
    List<SignificadoDTO>? GetSignificadosEnDiccionario(string codigoDiccionario, string palabra);

    /// <summary>
    /// Obtiene los significados de una palabra en todos los diccionarios de un idioma
    /// </summary>
    /// <param name="codigoIdioma">Código del idioma</param>
    /// <param name="palabra">Palabra a buscar</param>
    /// <returns>Lista de significados de todos los diccionarios del idioma o null si no se encuentra</returns>
    List<SignificadoDTO>? GetSignificadosEnIdioma(string codigoIdioma, string palabra);

    /// <summary>
    /// Verifica si una palabra existe en un diccionario específico
    /// </summary>
    /// <param name="codigoDiccionario">Código del diccionario</param>
    /// <param name="palabra">Palabra a verificar</param>
    /// <returns>True si la palabra existe, false en caso contrario</returns>
    bool ExistePalabraEnDiccionario(string codigoDiccionario, string palabra);

    /// <summary>
    /// Verifica si una palabra existe en al menos uno de los diccionarios de un idioma
    /// </summary>
    /// <param name="codigoIdioma">Código del idioma</param>
    /// <param name="palabra">Palabra a verificar</param>
    /// <returns>True si la palabra existe en algún diccionario del idioma, false en caso contrario</returns>
    bool ExistePalabraEnIdioma(string codigoIdioma, string palabra);
}