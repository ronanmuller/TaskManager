using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamentos para Projetos
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, CreateProjectDto>().ReverseMap();

            // Mapeamentos para Tarefas
            CreateMap<Tasks, TaskDto>().ReverseMap(); // Mapeia entre Tasks e TaskDto
            CreateMap<CreateTaskDto, Tasks>() // Mapeia de CreateTaskDto para Tasks
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TaskState.Pending)); // Define o status inicial
        }
    }
}