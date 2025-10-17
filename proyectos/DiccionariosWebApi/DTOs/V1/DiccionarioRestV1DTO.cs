namespace DiccionariosWebApi.DTOs.V1;

/// <summary>
/// DTO para representar un diccionario en la API REST v1
/// </summary>
public class DiccionarioRestV1DTO
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
}