using AutoMapper;
using DiccionariosApi;
using ServicioDiccionarios.DTOs;

namespace ServicioDiccionarios.Implementacion.Mappers;

/// <summary>
/// Configuración de AutoMapper para mapear entre objetos de la API de diccionarios y DTOs del servicio
/// </summary>
public class DiccionariosMapperProfile : Profile
{
    public DiccionariosMapperProfile()
    {
        // Mapeo de IIdioma a IdiomaDTO
        CreateMap<IIdioma, IdiomaDTO>()
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

        // Mapeo de IDiccionario a DiccionarioDTO
        CreateMap<IDiccionario, DiccionarioDTO>()
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.Idioma, opt => opt.MapFrom(src => src.Idioma))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre)); // Placeholder para nombre
    }
}