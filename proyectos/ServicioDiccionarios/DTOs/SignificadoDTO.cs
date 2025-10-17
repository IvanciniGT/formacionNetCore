namespace ServicioDiccionarios.DTOs;

/// <summary>
/// DTO para representar un significado en la capa de servicio
/// </summary>
public class SignificadoDTO
{
    public string Texto { get; set; } = string.Empty;
    public string Diccionario { get; set; } = string.Empty;
}