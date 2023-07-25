using AutoMapper;
using WebAPI.DTOs.Request;
using WebAPI.DTOs.Response;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // entity to view
            CreateMap<JobTitle, JobTitleRespDto>();
            CreateMap<JobPosition, JobPositionRespDto>();
            CreateMap<JobPosition, JobPositionBaseRespDto>();
            CreateMap<Employee, EmployeeRespDto>();

            // view to entity
            CreateMap<JobTitleReqDto, JobTitle>();
            CreateMap<JobPositionReqDto, JobPosition>().ForPath(jp => jp.JobTitle.Id, act => act.MapFrom(req => req.JobTitleId));
            CreateMap<EmployeeReqDto, Employee>()
                .ForPath(emp => emp.JobPosition.Id, act => act.MapFrom(req => req.JobPositionId))
                .ForPath(emp => emp.JobTitle.Id, act => act.MapFrom(req => req.JobTitleId));
        }
    }
}
