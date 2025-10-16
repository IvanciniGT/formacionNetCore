namespace DiccionariosBBDD.Entities;

public class DiccionarioEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdiomaId { get; set; }
    
    // Navigation properties
    public IdiomaEntity Idioma { get; set; } = null!;
    public ICollection<PalabraEntity> Palabras { get; set; } = new List<PalabraEntity>();
}