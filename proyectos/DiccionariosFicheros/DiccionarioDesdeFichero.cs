namespace DiccionariosFicheros;

using DiccionariosApi;

public class DiccionarioDesdeFichero : IDiccionario
{

    public DiccionarioDesdeFichero(
            string idioma,                                              // Idioma del diccionario
            Dictionary<string, IList<string>> palabrasYDefiniciones     // Definiciones (Tabla clave valor, donde la clave es la palabra y el valor es la lista de definiciones)
        )
    {
        Idioma = idioma;
    }

    public string Idioma { get; }

    public bool Existe(string palabra)
    {
        return false;
    }

    public IList<string>? GetSignificados(string palabra)
    {
        return null;
    }
}
