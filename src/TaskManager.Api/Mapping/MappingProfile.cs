using AutoMapper;
using TaskManager.Application.Dto;
using TaskManager.Domain.Entities;

namespace TaskManager.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Project para DTOs
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, CreateProjectDto>().ReverseMap();

            // Mapeamento de Tasks para TaskDto e vice-versa
            CreateMap<Tasks, TaskDto>().ReverseMap();

            // Mapeamento de CreateTaskDto para Tasks, inicializando o status como Pending
            CreateMap<CreateTaskDto, Tasks>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Define o status inicial como Pending
                .ForMember(dest => dest.UpdateHistories, opt => opt.Ignore()) // Ignora o histórico de atualizações no mapeamento inicial
                .ForMember(dest => dest.Project, opt => opt.Ignore()); // Project será associado via ProjectId

            CreateMap<Tasks, TaskReportDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Project.UserId))
                .ReverseMap();

            CreateMap<TaskComment, TaskCommentDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Comment)) 
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedAt)) 
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)); //

        }
    }
}