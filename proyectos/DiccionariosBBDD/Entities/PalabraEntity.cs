namespace DiccionariosBBDD.Entities;

public class PalabraEntity
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    // TextoNormalizado eliminado - usamos índice funcional UPPER(Texto) en su lugar
    public int DiccionarioId { get; set; }
    
    // Navigation properties
    public DiccionarioEntity Diccionario { get; set; } = null!;
    public ICollection<SignificadoEntity> Significados { get; set; } = new List<SignificadoEntity>();
    // Esta funcion se traduce en una query a la BBDD: SELECT * FROM Significados WHERE PalabraId = {Id}
}