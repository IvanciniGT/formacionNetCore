using DiccionariosApi;
using UIAplicacionDeDiccionariosApi;

namespace DiccionariosAppHost;

public class Aplicacion
{
    private readonly ISuministradorDeDiccionarios _suministradorDeDiccionarios;
    private readonly IUIAplicacionDeDiccionarios _ui;

    public Aplicacion(ISuministradorDeDiccionarios suministradorDeDiccionarios, IUIAplicacionDeDiccionarios ui)
    {
        _suministradorDeDiccionarios = suministradorDeDiccionarios;
        _ui = ui;
    }

    public async Task<int> EjecutarAsync(string[] args)
    {
        _ui.MostrarMensajeBienvenida();

        // Validar argumentos
        if (args.Length != 2)
        {
            _ui.MostrarErrorArgumentosIncorrectos();
            _ui.MostrarMensajeDespedida();
            return 1;
        }

        var idioma = args[0];
        var palabra = args[1];

        _ui.MostrarBuscando(palabra, idioma);

        // Verificar si existe diccionario para el idioma
        if (!_suministradorDeDiccionarios.TienesDiccionarioDe(idioma))
        {
            _ui.MostrarErrorNoHayDiccionario(idioma);
            _ui.MostrarMensajeDespedida();
            return 1;
        }

        // Obtener el diccionario
        var diccionario = _suministradorDeDiccionarios.DameDiccionarioDe(idioma);
        if (diccionario == null)
        {
            _ui.MostrarErrorInterno($"No se pudo cargar el diccionario de '{idioma}'");
            _ui.MostrarMensajeDespedida();
            return 1;
        }

        _ui.MostrarDiccionarioCargado(diccionario.Idioma);

        // Verificar si la palabra existe
        if (!diccionario.Existe(palabra))
        {
            _ui.MostrarErrorNoHayPalabra(palabra, diccionario.Idioma);
            _ui.MostrarMensajeDespedida();
            return 1;
        }

        // Obtener y mostrar los significados
        var significados = diccionario.GetSignificados(palabra);
        if (significados == null || significados.Count == 0)
        {
            _ui.MostrarErrorSinSignificados(palabra);
            _ui.MostrarMensajeDespedida();
            return 1;
        }

        _ui.MostrarSignificadosDePalabra(palabra, significados);
        _ui.MostrarMensajeDespedida();
        return 0;
    }
}