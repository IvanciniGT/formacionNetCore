namespace DiccionariosBBDD.Entities;

public class SignificadoEntity
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int PalabraId { get; set; }
    
    // Navigation properties
    public PalabraEntity Palabra { get; set; } = null!;
}