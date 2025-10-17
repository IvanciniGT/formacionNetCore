using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ServicioDiccionarios.Implementacion.Mappers;

namespace ServicioDiccionarios.Implementacion;

public static class AutoMapperRegistration
{
    public static IServiceCollection AddDiccionariosMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DiccionariosMapperProfile));
        return services;
    }
}
