using Microsoft.Extensions.DependencyInjection;
using DiccionariosApi;
using DiccionariosFicheros;

namespace DiccionariosAppConsolaConDI;

public static class DependencyInjectionConfig
{
    public static ServiceProvider ConfigurarServicios()
    {
        // Crear el ServiceCollection
        var services = new ServiceCollection();
        
        // Registrar las implementaciones de las interfaces
        // A la interfaz ISuministradorDeDiccionarios, usar la implementación SuministradorDeDiccionariosDesdeFicheros
        services.AddSingleton<ISuministradorDeDiccionarios>(provider =>
        {
            var rutaDiccionarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Diccionarios");
            return new SuministradorDeDiccionariosDesdeFicheros(rutaDiccionarios);
        });
        
        // Crear el ServiceProvider a partir del ServiceCollection
        return services.BuildServiceProvider();
    }
}