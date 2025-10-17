namespace DiccionariosWebApi.DTOs.V1;

/// <summary>
/// DTO para representar un idioma en la API REST v1
/// </summary>
public class IdiomaRestV1DTO
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
}