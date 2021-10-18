using CQRS_MediatoR.Common.Entities.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.RequestModels
{
    public class StartTaskRequest:IRequest<StartTaskResponse>
    {
        public int TaskId { get; set; }
    }
}
