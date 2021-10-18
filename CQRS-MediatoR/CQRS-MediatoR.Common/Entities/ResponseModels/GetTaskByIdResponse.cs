using CQRS_MediatoR.Common.Entities.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.ResponseModels
{
  public  class GetTaskByIdResponse
    {
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
    }
}
