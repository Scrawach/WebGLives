using AutoMapper;
using WebGLives.Core;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess;

public class DataAccessMappingProfile : Profile
{
    public DataAccessMappingProfile()
    {
        CreateMap<GamePageEntity, GamePage>().ReverseMap();
    }
}