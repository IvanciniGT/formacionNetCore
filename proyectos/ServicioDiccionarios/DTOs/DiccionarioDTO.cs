namespace ServicioDiccionarios.DTOs;

/// <summary>
/// DTO para representar un diccionario en la capa de servicio
/// </summary>
public class DiccionarioDTO
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
}