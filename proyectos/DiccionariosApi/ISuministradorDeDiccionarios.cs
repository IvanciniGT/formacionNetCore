namespace DiccionariosApi;

public interface ISuministradorDeDiccionarios
{

    // Una función que nos confirme si tenemos disponible un diccionario para un idioma
    bool TienesDiccionarioDe(string idioma);

    [Obsolete("Usa GetDiccionarios en su lugar")]
    // Otra función para obtener el diccionario de un idioma
    IDiccionario? DameDiccionarioDe(string idioma);

    // A todas estas nuevas funciones de mi API, les doy una implementación por defecto, simplemente para asegurarme que el código compila.
    // Así no jodo a nadie! Como estamos generando una nueva versión de la API, si quiero hacer una versión que respecte retrocompatibilidad,
    // puedo añadir nuevas funciones con una implementación por defecto.
    // Aunque la implementación sea LANZA UNA EXCEPTION. El código antiguo seguirá compilando.
    // Y qué pasa si alguien llama a esas nuevas funciones? Le saltará la excepción... y el programa no funciona.
    // Pero eso no pasa... Porque el código antiguo NO ESTABA LLAMANDO a esas nuevas funcione porque no existían.
    IDiccionario? GetDiccionarioPorCodigo(string codigoDiccionario)
    {
        // Lanzar Exception en cualquier caso:
        throw new NotImplementedException("Este método debe ser implementado por la clase que implemente esta interfaz.");
    }

    IList<IDiccionario>? GetDiccionarios(string codigoIdioma)
    {
        if(TienesDiccionarioDe(codigoIdioma))
        {
            var diccionario = DameDiccionarioDe(codigoIdioma);
            if(diccionario != null)
            {
                return new List<IDiccionario> { diccionario };
            }
        }
        return null;
    }

    IIdioma GetIdiomas()
    {
        throw new NotImplementedException("Este método debe ser implementado por la clase que implemente esta interfaz.");
    }

}