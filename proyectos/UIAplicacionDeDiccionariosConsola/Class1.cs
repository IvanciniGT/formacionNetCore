using UIAplicacionDeDiccionariosApi;
using DiccionariosApi;

namespace UIAplicacionDeDiccionariosConsola;

public class UIAplicacionDeDiccionariosConsola : IUIAplicacionDeDiccionarios
{
    // La UI ya no necesita dependencias - solo muestra información
    public UIAplicacionDeDiccionariosConsola()
    {
    }

    public void MostrarMensajeBienvenida()
    {
        Console.WriteLine("🔍 ¡Bienvenido al Diccionario App CON HOST! 📚🏗️");
        Console.WriteLine("════════════════════════════════════════════════");
    }

    public void MostrarMensajeDespedida()
    {
        Console.WriteLine();
        Console.WriteLine("════════════════════════════════════════════════");
        Console.WriteLine("🙏 ¡Gracias por usar Diccionario App CON HOST!");
        Console.WriteLine("🏗️ ¡Inversión de control funcionando! ⚙️");
        Console.WriteLine("📚 ¡Que tengas un buen día! ✨");
    }

    public void MostrarSignificadosDePalabra(string palabra, IList<string> significados)
    {
        Console.WriteLine($"🎉 ¡Encontré la palabra '{palabra}' usando HOST e IoC! Aquí están sus significados:");
        Console.WriteLine();
        for (int i = 0; i < significados.Count; i++)
        {
            Console.WriteLine($"   {i + 1}. {significados[i]} ✨");
        }
    }

    public void MostrarErrorNoHayDiccionario(string idioma)
    {
        Console.WriteLine($"❌ Lo siento, no tengo un diccionario para el idioma '{idioma}' 😞");
        Console.WriteLine($"💡 Asegúrate de que existe el archivo '{idioma}.txt' en la carpeta configurada");
    }

    public void MostrarErrorNoHayPalabra(string palabra, string idioma)
    {
        Console.WriteLine($"❌ La palabra '{palabra}' no existe en el diccionario de {idioma} 😞");
        Console.WriteLine("💡 Prueba con otra palabra o revisa la ortografía");
    }

    public void MostrarErrorArgumentosIncorrectos()
    {
        Console.WriteLine("❌ Error: Debes proporcionar exactamente 2 argumentos:");
        Console.WriteLine("   Uso: DiccionariosAppHost <idioma> <palabra>");
        Console.WriteLine("   Ejemplo: DiccionariosAppHost ES melón");
    }

    public void MostrarBuscando(string palabra, string idioma)
    {
        Console.WriteLine($"🔎 Buscando '{palabra}' en el diccionario de idioma '{idioma}'...");
        Console.WriteLine("🏗️ HOST gestionando inversión de control...");
        Console.WriteLine();
    }

    public void MostrarDiccionarioCargado(string idioma)
    {
        Console.WriteLine($"✅ Diccionario de '{idioma}' cargado por HOST! 📖");
    }

    public void MostrarErrorInterno(string mensaje)
    {
        Console.WriteLine($"❌ Error interno: {mensaje} 😞");
    }

    public void MostrarErrorSinSignificados(string palabra)
    {
        Console.WriteLine($"❌ No se encontraron significados para '{palabra}' 😞");
    }

    // Este método ya no debería usarse - la lógica está en la clase Aplicacion
    // El método EjecutarAsync ya no corresponde a la UI - ahora está en la clase Aplicacion
}
