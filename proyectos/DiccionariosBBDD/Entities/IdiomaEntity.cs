namespace DiccionariosBBDD.Entities;

public class IdiomaEntity
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<DiccionarioEntity> Diccionarios { get; set; } = new List<DiccionarioEntity>();
}