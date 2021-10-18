using AutoMapper;
using CQRS_MediatoR.Common.Entities.Domain;
using CQRS_MediatoR.Common.Entities.Domain.Enums;
using CQRS_MediatoR.Common.Entities.RequestModels;
using CQRS_MediatoR.Common.Entities.ResponseModels;

namespace CQRS_MediatoR.Api.Configuration
{
    public class Mapper
    {
        public static void ConfigureMappings(IMapperConfigurationExpression config)
        {

            config.CreateMap<AddTaskRequest, Task>()
                .ForMember(x => x.Id, x => x.Ignore());

        }
    }
}
