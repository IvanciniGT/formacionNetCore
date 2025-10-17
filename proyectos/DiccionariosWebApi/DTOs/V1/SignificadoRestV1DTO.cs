namespace DiccionariosWebApi.DTOs.V1;

/// <summary>
/// DTO para representar un significado en la API REST v1
/// </summary>
public class SignificadoRestV1DTO
{
    public string Texto { get; set; } = string.Empty;
    public string Diccionario { get; set; } = string.Empty;
    public string CodigoDiccionario { get; set; } = string.Empty;
}