using DiccionariosApi;
using DiccionariosAppConsolaConDI;
using Microsoft.Extensions.DependencyInjection;

// Mensaje de bienvenida
Console.WriteLine("🔍 ¡Bienvenido al Diccionario App CON DI! 📚⚙️");
Console.WriteLine("═══════════════════════════════════════════");

// Validar argumentos
if (args.Length != 2)
{
    Console.WriteLine("❌ Error: Debes proporcionar exactamente 2 argumentos:");
    Console.WriteLine("   Uso: DiccionariosAppConsolaConDI <idioma> <palabra>");
    Console.WriteLine("   Ejemplo: DiccionariosAppConsolaConDI ES melón");
    MostrarDespedida();
    return;
}

var idioma = args[0];
var palabra = args[1];

Console.WriteLine($"🔎 Buscando '{palabra}' en el diccionario de idioma '{idioma}'...");
Console.WriteLine("⚙️ Configurando inyección de dependencias...");
Console.WriteLine();

// Configurar ServiceProvider (Inyección de Dependencias)
using var serviceProvider = DependencyInjectionConfig.ConfigurarServicios();

// Resolver ISuministradorDeDiccionarios usando DI
var suministrador = serviceProvider.GetRequiredService<ISuministradorDeDiccionarios>();

Console.WriteLine("✅ ¡Inyección de dependencias configurada! 🎯");

// Verificar si existe diccionario para el idioma
if (!suministrador.TienesDiccionarioDe(idioma))
{
    Console.WriteLine($"❌ Lo siento, no tengo un diccionario para el idioma '{idioma}' 😞");
    Console.WriteLine($"💡 Asegúrate de que existe el archivo '{idioma}.txt' en la carpeta Diccionarios");
    MostrarDespedida();
    return;
}

// Obtener el diccionario
var diccionario = suministrador.DameDiccionarioDe(idioma);
if (diccionario == null)
{
    Console.WriteLine($"❌ Error interno: No se pudo cargar el diccionario de '{idioma}' 😞");
    MostrarDespedida();
    return;
}

Console.WriteLine($"✅ Diccionario de '{diccionario.Idioma}' cargado correctamente! 📖");

// Verificar si la palabra existe
if (!diccionario.Existe(palabra))
{
    Console.WriteLine($"❌ La palabra '{palabra}' no existe en el diccionario de {diccionario.Idioma} 😞");
    Console.WriteLine("💡 Prueba con otra palabra o revisa la ortografía");
    MostrarDespedida();
    return;
}

// Obtener y mostrar los significados
var significados = diccionario.GetSignificados(palabra);
if (significados == null || significados.Count == 0)
{
    Console.WriteLine($"❌ No se encontraron significados para '{palabra}' 😞");
    MostrarDespedida();
    return;
}

Console.WriteLine($"🎉 ¡Encontré la palabra '{palabra}' usando DI! Aquí están sus significados:");
Console.WriteLine();
for (int i = 0; i < significados.Count; i++)
{
    Console.WriteLine($"   {i + 1}. {significados[i]} ✨");
}

MostrarDespedida();

static void MostrarDespedida()
{
    Console.WriteLine();
    Console.WriteLine("═══════════════════════════════════════════");
    Console.WriteLine("🙏 ¡Gracias por usar Diccionario App CON DI!");
    Console.WriteLine("⚙️ ¡Inyección de dependencias funcionando! 🎯");
    Console.WriteLine("📚 ¡Que tengas un buen día! ✨");
}
