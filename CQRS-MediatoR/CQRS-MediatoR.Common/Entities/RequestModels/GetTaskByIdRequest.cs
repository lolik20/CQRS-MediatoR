using CQRS_MediatoR.Common.Entities.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.RequestModels
{
    public class GetTaskByIdRequest : IRequest<GetTaskByIdResponse>
    {
        public int TaskId { get; set; }
    }
}
