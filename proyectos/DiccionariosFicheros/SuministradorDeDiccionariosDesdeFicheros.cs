namespace DiccionariosFicheros;

using DiccionariosApi;

public class SuministradorDeDiccionariosDesdeFicheros : ISuministradorDeDiccionarios
{
    // Para que el programa sea más o menos eficiente, vamos a hacer una carga de diccionarios 
    // en modo perezoso (lazy loading de los diccionarios)
    // Es decir, no vamos a cargar todos los diccionarios al iniciar el programa
    // Los vamos cargando a medida que se vayan pidiendo y los guardamos en una cache!

    private readonly string _rutaCarpetaDiccionarios;
    private readonly Dictionary<string, WeakReference<IDiccionario>> _cacheDiccionarios = new();

    // Constructor que admita la ruta de la carpeta donde están los archivos de los diccionarios
    public SuministradorDeDiccionariosDesdeFicheros(string rutaCarpetaDiccionarios)
    {
        _rutaCarpetaDiccionarios = rutaCarpetaDiccionarios;
    }

    public IDiccionario? DameDiccionarioDe(string idioma)
    {
        if (!TienesDiccionarioDe(idioma))
            return null;
        if (!TengoUnIdiomaEnCache(idioma))
            MeterDiccionarioEnCache(idioma);
        return SacarDiccionarioDeCache(idioma);
    }

    public bool TienesDiccionarioDe(string idioma)
    {
        return TengoUnIdiomaEnCache(idioma) || Utilidades.TengoUnArchivoParaElIdioma(idioma, _rutaCarpetaDiccionarios);
    }

    private bool TengoUnIdiomaEnCache(string idioma)
    {
        return _cacheDiccionarios.TryGetValue(idioma, out var referenciaDebilAlDiccionario) &&
               referenciaDebilAlDiccionario.TryGetTarget(out var diccionarioEnCache);
    }

    private void MeterDiccionarioEnCache(string idioma)
    {
        var palabrasYDefiniciones = LeerElFicheroDePalabrasYProcesarlo(idioma);
        crearYMeterDiccionarioEnCache(idioma, palabrasYDefiniciones);
    }

    private Dictionary<string, IList<string>> LeerElFicheroDePalabrasYProcesarlo(string idioma)
    {
        Dictionary<string, IList<string>> palabrasYDefiniciones = new(); // Esto es lo que voy a leer del fichero
        var rutaFicheroDiccionario = Path.Combine(_rutaCarpetaDiccionarios, $"{idioma}.txt"); // (1)
                                                                                              // Leo el fichero y Voy procesando cada linea.
        foreach (var linea in File.ReadLines(rutaFicheroDiccionario))
            procesarPalabraYSignificados(linea, palabrasYDefiniciones);
        return palabrasYDefiniciones;
    }

    private void crearYMeterDiccionarioEnCache(string idioma, Dictionary<string, IList<string>> palabrasYDefiniciones)
    {
        // Una vez que tengo todas las palabras, con sus deficiones, 
        var diccionarioADevolver = new DiccionarioDesdeFichero(idioma, palabrasYDefiniciones); // crearemos un objeto DiccionarioDesdeFichero
        _cacheDiccionarios[idioma] = new WeakReference<IDiccionario>(diccionarioADevolver); // Y lo guardaremos en la cache

    }

    private void procesarPalabraYSignificados(string linea, Dictionary<string, IList<string>> palabrasYDefiniciones)
    {
            var palabraYDefiniciones = linea.Split('=', 2); // El 2 es para que haga como máximo 2 trozos
                                                            // De cada linea, lo que haya antes del = es la palabra... 
            var palabra = palabraYDefiniciones[0].Trim();
            // Lo que haya después del = son las definiciones. 
            var definicionesJuntas = palabraYDefiniciones[1];
            // Puede haber varias. En ese caso las vamos a separar por |
            var definicionesSeparadas = definicionesJuntas.Split('|', StringSplitOptions.RemoveEmptyEntries);
            // Que las necesito como una lista
            var listaDefiniciones = new List<string>();
            foreach (var definicion in definicionesSeparadas)
            {
                listaDefiniciones.Add(definicion.Trim());
            }
            // Guardo la entrada en esa estructura de datos que habiamos preparado (1)
            palabrasYDefiniciones[palabra] = listaDefiniciones;
    }

    private IDiccionario SacarDiccionarioDeCache(string idioma)
    {
        _cacheDiccionarios.TryGetValue(idioma, out var referenciaDebilAlDiccionario);
        referenciaDebilAlDiccionario.TryGetTarget(out var diccionarioEnCache); // Esta función la ofrece el WeakReference. Intenta ver si aún tiene en memoria el objeto
        return diccionarioEnCache;
    }
    
}
