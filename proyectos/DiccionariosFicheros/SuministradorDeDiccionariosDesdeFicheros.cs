namespace DiccionariosFicheros;

using DiccionariosApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class SuministradorDeDiccionariosDesdeFicheros : ISuministradorDeDiccionarios
{
    // Para que el programa sea más o menos eficiente, vamos a hacer una carga de diccionarios 
    // en modo perezoso (lazy loading de los diccionarios)
    // Es decir, no vamos a cargar todos los diccionarios al iniciar el programa
    // Los vamos cargando a medida que se vayan pidiendo y los guardamos en una cache!

    private readonly string _rutaCarpetaDiccionarios;
    private readonly Dictionary<string, WeakReference<IDiccionario>> _cacheDiccionarios = new();
    private readonly ILogger<SuministradorDeDiccionariosDesdeFicheros> _logger;

    // Constructor que recibe configuración y logger - lee sus parámetros directamente
    public SuministradorDeDiccionariosDesdeFicheros(IConfiguration configuration, ILogger<SuministradorDeDiccionariosDesdeFicheros> logger)
    {
        _logger = logger;
        
        try
        {
            // Leer configuración directamente
            var rutaRelativa = configuration["DiccionariosConfig:RutaCarpetaDiccionarios"];
            
            if (string.IsNullOrWhiteSpace(rutaRelativa))
            {
                throw new InvalidOperationException("No se encontró la configuración 'DiccionariosConfig:RutaCarpetaDiccionarios' en appsettings.json");
            }

            _rutaCarpetaDiccionarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaRelativa);
            
            _logger.LogInformation("SuministradorDeDiccionariosDesdeFicheros inicializado con ruta: {RutaCarpeta}", _rutaCarpetaDiccionarios);
            
            // Verificar que la carpeta existe
            if (!Directory.Exists(_rutaCarpetaDiccionarios))
            {
                _logger.LogWarning("La carpeta de diccionarios no existe: {RutaCarpeta}", _rutaCarpetaDiccionarios);
                // Pararmos la ejecución
                throw new InvalidOperationException($"La carpeta de diccionarios no existe: {_rutaCarpetaDiccionarios}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar SuministradorDeDiccionariosDesdeFicheros");
            throw;
        }
    }

    // Constructor legacy para mantener compatibilidad con pruebas existentes
    public SuministradorDeDiccionariosDesdeFicheros(string rutaCarpetaDiccionarios)
    {
        _rutaCarpetaDiccionarios = rutaCarpetaDiccionarios;
        _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<SuministradorDeDiccionariosDesdeFicheros>.Instance;
    }

    public IDiccionario? DameDiccionarioDe(string idioma)
    {
        _logger.LogDebug("Solicitando diccionario para idioma: {Idioma}", idioma);
        
        if (!TienesDiccionarioDe(idioma))
        {
            _logger.LogWarning("No se encontró diccionario para el idioma: {Idioma}", idioma);
            return null;
        }
        
        if (!TengoUnIdiomaEnCache(idioma))
        {
            _logger.LogInformation("Cargando diccionario para idioma: {Idioma}", idioma);
            MeterDiccionarioEnCache(idioma);
        }
        else
        {
            _logger.LogDebug("Diccionario para idioma {Idioma} encontrado en cache", idioma);
        }
        
        return SacarDiccionarioDeCache(idioma);
    }

    public bool TienesDiccionarioDe(string idioma)
    {
        return TengoUnIdiomaEnCache(idioma) || Utilidades.TengoUnArchivoParaElIdioma(idioma, _rutaCarpetaDiccionarios);
    }

    //Funciones de gestión de la cache!
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

    private void crearYMeterDiccionarioEnCache(string idioma, Dictionary<string, IList<string>> palabrasYDefiniciones)
    {
        // Una vez que tengo todas las palabras, con sus deficiones, 
        var diccionarioADevolver = new DiccionarioDesdeFichero(idioma, palabrasYDefiniciones); // crearemos un objeto DiccionarioDesdeFichero
        _cacheDiccionarios[idioma] = new WeakReference<IDiccionario>(diccionarioADevolver); // Y lo guardaremos en la cache

    }

    private IDiccionario SacarDiccionarioDeCache(string idioma)
    {
        _cacheDiccionarios.TryGetValue(idioma, out var referenciaDebilAlDiccionario);
        referenciaDebilAlDiccionario.TryGetTarget(out var diccionarioEnCache); // Esta función la ofrece el WeakReference. Intenta ver si aún tiene en memoria el objeto
        return diccionarioEnCache;
    }


    // Funciones de lectura del fichero

    // APLICAMOS UN ENFOQUE MAS MODERNO: PROGRAMACION FUNCIONAL: LINQ
    // LINQ: Language Integrated Query
    // Nos ofrece una sintaxis "INSPIRADA" en SQL para hacer consultas sobre colecciones de datos: Enumerado, Listas, Arrays, Diccionarios...
    // Inspirada es mucho decir.. No se parece en NADA!... Solo tiene 3 palabras clave parecidas: from, where, select
    // Lo que si está inspirado es en un modelo de programación llamado MapReduce
    // MapReduce es un modelo de programación funcional, que se usa mucho en BigData
    // Básicamente me permite ir haciendo transformaciones sobre colecciones de datos, mediante
    // funciones, a las que les vamos a pasar funciones como parámetros

    private Dictionary<string, IList<string>> LeerElFicheroDePalabrasYProcesarlo(string idioma)
    {
        var rutaFicheroDiccionario = Path.Combine(_rutaCarpetaDiccionarios, $"{idioma}.txt");

        return File.ReadLines(rutaFicheroDiccionario)                                                   // Leo las lineas del fichero
            .Where(  linea => !string.IsNullOrWhiteSpace(linea) && !linea.TrimStart().StartsWith('#'))  // Me quedo solo con las que no están vacías y no son comentarios
            .Select( linea => linea.Split('=', 2))                                                      // Parto cada linea en 2 partes, usando el = como separador
            .ToDictionary(                                                                              // Convierto el resultado en una tabla clave-valor
                    partes => Utilidades.NormalizarPalabra(partes[0]),                          // Donde la clave es la palabra NORMALIZADA (la primera parte de cada linea)
                    partes => (IList<string>)partes[1].Split('|', StringSplitOptions.RemoveEmptyEntries).ToList() // Y el valor es la lista de definiciones
            );
    }

    /*
        private Dictionary<string, IList<string>> LeerElFicheroDePalabrasYProcesarlo(string idioma)
        {
            var rutaFicheroDiccionario = Path.Combine(_rutaCarpetaDiccionarios, $"{idioma}.txt");

            return File.ReadLines(rutaFicheroDiccionario)
                .Where(linea => !string.IsNullOrWhiteSpace(linea) && !linea.TrimStart().StartsWith('#'))
                // La función where me permite filtrar cosas de una colección.
                // Le digo con qué cosas me quiero quedar de esa colección.
                // Cómo se lo digo? Suministrando una función que dado un elemento de esa colección devuelva true si me lo quiero quedar.
                .Select(linea => linea.Split('=', 2))
                // La función select me permite transformar cada elemento de una colección en otro elemento.
                // Le paso una función de transformación, y Select aplica esa función a cada elemento de la colección
                // Y genera una nueva colección con los resultados de aplicar la función de transformación a cada elemento de la colección original
                // en nuestro caso, qué devuelve select? Un enumerado, de arrays de strings.
                .ToDictionary(
                        partes => partes[0].Trim(),               // Función que extrae la clave (la palabra)
                        partes => (IList<string>)partes[1].Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()// Debo generar una Lista con los significados
                );
            // me permite convertir una colección de datos en un Dictionary (una tabla clave-valor);
            // A esta función le pasaremos 2 funciones:
            // La primera es una función que recibe el cada dato de la colección original y devuelve lo que se usará como clave
            // La segunda es una función que recibe cada dato de la colección original y devuelve lo que se usará como valor
        }
        */

    /* 
        ENFOQUE CLASICO: PROMAGRAMACION IMPERATIVA

        private Dictionary<string, IList<string>> LeerElFicheroDePalabrasYProcesarlo(string idioma)
        {
            Dictionary<string, IList<string>> palabrasYDefiniciones = new(); // Esto es lo que voy a leer del fichero
            var rutaFicheroDiccionario = Path.Combine(_rutaCarpetaDiccionarios, $"{idioma}.txt"); // (1)
                                                                                                  // Leo el fichero y Voy procesando cada linea.
            foreach (var linea in File.ReadLines(rutaFicheroDiccionario))
                procesarPalabraYSignificados(linea, palabrasYDefiniciones);
            return palabrasYDefiniciones;
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
        }*/
}
