namespace DiccionariosFicheros;

using DiccionariosApi;

public class DiccionarioDesdeFichero : IDiccionario
{

    private readonly Dictionary<string, IList<string>> palabrasYDefiniciones;

    public DiccionarioDesdeFichero(
            string idioma,                                              // Idioma del diccionario
            Dictionary<string, IList<string>> palabrasYDefiniciones     // Definiciones (Tabla clave valor, donde la clave es la palabra y el valor es la lista de definiciones)
        )
    {
        Idioma = idioma;
        this.palabrasYDefiniciones = palabrasYDefiniciones;
    }

    public string Idioma { get; }

    public bool Existe(string palabra)
    {
        if (palabra == null) return false;
        
        var palabraNormalizada = Utilidades.NormalizarPalabra(palabra);
        return !string.IsNullOrEmpty(palabraNormalizada) && palabrasYDefiniciones.ContainsKey(palabraNormalizada);
    }

    public IList<string>? GetSignificados(string palabra)
    {
        if (palabra == null) return null;
        
        var palabraNormalizada = Utilidades.NormalizarPalabra(palabra);
        return !string.IsNullOrEmpty(palabraNormalizada) && palabrasYDefiniciones.ContainsKey(palabraNormalizada)
            ? palabrasYDefiniciones[palabraNormalizada]
            : null;
    }
}
