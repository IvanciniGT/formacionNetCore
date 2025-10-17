using AutoMapper;
using ServicioDiccionarios.DTOs;
using DiccionariosWebApi.DTOs.V1;

namespace DiccionariosWebApi.Mappers;

/// <summary>
/// Perfil de AutoMapper para mapear entre DTOs del servicio y DTOs REST v1
/// </summary>
public class RestV1MapperProfile : Profile
{
    public RestV1MapperProfile()
    {
        // Mapeo de IdiomaDTO a IdiomaRestV1DTO
        CreateMap<IdiomaDTO, IdiomaRestV1DTO>();

        // Mapeo de DiccionarioDTO a DiccionarioRestV1DTO
        CreateMap<DiccionarioDTO, DiccionarioRestV1DTO>();

        // Mapeo de SignificadoDTO a SignificadoRestV1DTO
        CreateMap<SignificadoDTO, SignificadoRestV1DTO>();
    }
}