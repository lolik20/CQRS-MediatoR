using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.Common.Entities.Domain.Enums
{
    public enum Status
    {
        None = 0,
        InProgress = 1,
        Expired = 2,
        Done=3
    }
}
