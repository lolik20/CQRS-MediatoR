using CQRS_MediatoR.Common.Entities.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.RequestModels
{
    public class GetTasksRequest : IRequest<GetTasksResponse>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
    }
}
