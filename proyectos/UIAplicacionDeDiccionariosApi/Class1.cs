namespace UIAplicacionDeDiccionariosApi;

public interface IUIAplicacionDeDiccionarios
{
    void MostrarMensajeBienvenida();
    void MostrarMensajeDespedida();
    void MostrarSignificadosDePalabra(string palabra, IList<string> significados);
    void MostrarErrorNoHayDiccionario(string idioma);
    void MostrarErrorNoHayPalabra(string palabra, string idioma);
    void MostrarErrorArgumentosIncorrectos();
    void MostrarBuscando(string palabra, string idioma);
    void MostrarDiccionarioCargado(string idioma);
    void MostrarErrorInterno(string mensaje);
    void MostrarErrorSinSignificados(string palabra);
}
