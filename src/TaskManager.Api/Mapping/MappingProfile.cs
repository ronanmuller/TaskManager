using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
    }
}