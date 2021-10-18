using CQRS_MediatoR.Common.Entities.Domain.Enums;
using CQRS_MediatoR.Common.Entities.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.RequestModels
{
    public class AddTaskRequest : IRequest<AddTaskResponse>
    {
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
    }
}
