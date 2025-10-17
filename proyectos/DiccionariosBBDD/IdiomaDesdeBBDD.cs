using DiccionariosApi;
using DiccionariosBBDD.Entities;

namespace DiccionariosBBDD;

/// <summary>
/// Implementación de IIdioma que encapsula información de un idioma desde la base de datos
/// </summary>
public class IdiomaDesdeBBDD : IIdioma
{
    private readonly IdiomaEntity _idiomaEntity;

    public IdiomaDesdeBBDD(IdiomaEntity idiomaEntity)
    {
        _idiomaEntity = idiomaEntity ?? throw new ArgumentNullException(nameof(idiomaEntity));
    }

    public string Nombre => _idiomaEntity.Nombre;
    public string Codigo => _idiomaEntity.Codigo;
}