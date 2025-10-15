namespace DiccionariosAppHost;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using DiccionariosApi;
using UIAplicacionDeDiccionariosApi;

/// <summary>
/// Extensiones para auto-registro automático de dependencias
/// </summary>
/// 
/// 
// En lugar de todo este código, podríamos usar Scrutor.
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra automáticamente todas las implementaciones disponibles de IUIAplicacionDeDiccionarios e ISuministradorDeDiccionarios
    /// </summary>
    public static IServiceCollection AddAutoDiscoveredDependencies(this IServiceCollection services)
    {
        var logger = CreateTemporaryLogger();
        
        try
        {
            logger.LogInformation("Iniciando auto-registro de dependencias...");
            
            // Auto-registrar UI
            RegisterUIImplementations(services, logger);
            
            // Auto-registrar Suministradores de Diccionarios
            RegisterSuministradorImplementations(services, logger);
            
            logger.LogInformation("Auto-registro de dependencias completado exitosamente");
            
            return services;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error durante el auto-registro de dependencias");
            throw;
        }
    }

    private static void RegisterUIImplementations(IServiceCollection services, ILogger logger)
    {
        logger.LogDebug("Buscando implementaciones de IUIAplicacionDeDiccionarios...");
        
        var uiImplementations = FindImplementations<IUIAplicacionDeDiccionarios>();
        
        if (uiImplementations.Count == 0)
        {
            logger.LogWarning("No se encontraron implementaciones de IUIAplicacionDeDiccionarios");
            throw new InvalidOperationException("No se encontraron implementaciones de IUIAplicacionDeDiccionarios en los assemblies cargados");
        }
        
        // Registrar la primera implementación encontrada
        var selectedUI = uiImplementations.First();
        services.AddSingleton(typeof(IUIAplicacionDeDiccionarios), selectedUI);
        
        logger.LogInformation("Registrada implementación de UI: {UIType}", selectedUI.FullName);
        
        if (uiImplementations.Count > 1)
        {
            logger.LogInformation("Se encontraron {Count} implementaciones de UI. Otras disponibles: {OtherTypes}", 
                uiImplementations.Count, 
                string.Join(", ", uiImplementations.Skip(1).Select(t => t.Name)));
        }
    }

    private static void RegisterSuministradorImplementations(IServiceCollection services, ILogger logger)
    {
        logger.LogDebug("Buscando implementaciones de ISuministradorDeDiccionarios...");
        
        var suministradorImplementations = FindImplementations<ISuministradorDeDiccionarios>();
        
        if (suministradorImplementations.Count == 0)
        {
            logger.LogWarning("No se encontraron implementaciones de ISuministradorDeDiccionarios");
            throw new InvalidOperationException("No se encontraron implementaciones de ISuministradorDeDiccionarios en los assemblies cargados");
        }
        
        // Registrar la primera implementación encontrada
        var selectedSuministrador = suministradorImplementations.First();
        services.AddSingleton(typeof(ISuministradorDeDiccionarios), selectedSuministrador);
        
        logger.LogInformation("Registrada implementación de Suministrador: {SuministradorType}", selectedSuministrador.FullName);
        
        if (suministradorImplementations.Count > 1)
        {
            logger.LogInformation("Se encontraron {Count} implementaciones de Suministrador. Otras disponibles: {OtherTypes}", 
                suministradorImplementations.Count, 
                string.Join(", ", suministradorImplementations.Skip(1).Select(t => t.Name)));
        }
    }

    private static List<Type> FindImplementations<TInterface>()
    {
        var interfaceType = typeof(TInterface);
        var implementations = new List<Type>();
        
        // Cargar assemblies desde el directorio base de la aplicación
        LoadReferencedAssemblies();
        
        // Buscar en todos los assemblies cargados del dominio actual
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                // Buscar tipos que implementen la interfaz
                var types = assembly.GetTypes()
                    .Where(type => 
                        !type.IsInterface && 
                        !type.IsAbstract && 
                        interfaceType.IsAssignableFrom(type))
                    .ToList();
                
                implementations.AddRange(types);
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Algunos assemblies pueden fallar al cargar tipos, pero continuamos
                var logger = CreateTemporaryLogger();
                logger.LogWarning("No se pudieron cargar algunos tipos del assembly {AssemblyName}: {Exception}", 
                    assembly.FullName, ex.Message);
            }
        }
        
        return implementations;
    }

    private static void LoadReferencedAssemblies()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var logger = CreateTemporaryLogger();
        
        try
        {
            // Buscar archivos .dll en el directorio base
            var dllFiles = Directory.GetFiles(baseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
                .Where(file => 
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    // Solo cargar assemblies de nuestro proyecto
                    return fileName.StartsWith("DiccionariosFicheros") || 
                           fileName.StartsWith("UIAplicacionDeDiccionarios") ||
                           fileName.StartsWith("DiccionariosApi");
                });

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(dllFile);
                    
                    // Verificar si ya está cargado
                    if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == assemblyName.FullName))
                    {
                        Assembly.LoadFrom(dllFile);
                        logger.LogDebug("Assembly cargado: {AssemblyName}", assemblyName.Name);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning("No se pudo cargar assembly {DllFile}: {Exception}", dllFile, ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning("Error al buscar assemblies en {BaseDirectory}: {Exception}", baseDirectory, ex.Message);
        }
    }

    private static ILogger CreateTemporaryLogger()
    {
        using var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        return loggerFactory.CreateLogger("AutoRegistration");
    }
}