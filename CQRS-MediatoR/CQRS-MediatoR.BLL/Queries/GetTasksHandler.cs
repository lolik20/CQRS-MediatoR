using CQRS_MediatoR.Common.Entities.RequestModels;
using CQRS_MediatoR.Common.Entities.ResponseModels;
using CQRS_MediatoR.DAL.SqlContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS_MediatoR.BLL.Queries
{
    public class GetTasksHandler:IRequestHandler<GetTasksRequest,GetTasksResponse>
    {
        private readonly CQRSContext _context;

        public GetTasksHandler(CQRSContext context)
        {
            
        }
    }
}
