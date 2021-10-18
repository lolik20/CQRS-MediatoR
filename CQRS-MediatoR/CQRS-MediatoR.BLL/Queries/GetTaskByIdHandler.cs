using AutoMapper;
using CQRS_MediatoR.Common.Entities.RequestModels;
using CQRS_MediatoR.Common.Entities.ResponseModels;
using CQRS_MediatoR.DAL.SqlContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_MediatoR.BLL.Queries
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdRequest, GetTaskByIdResponse>
    {
        private readonly CQRSContext _context;
        private readonly IMapper _mapper;
        public GetTaskByIdHandler(CQRSContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetTaskByIdResponse> Handle(GetTaskByIdRequest request, CancellationToken cancellationToken)
        {
            var task = _context.Tasks
                .FirstOrDefault(x => x.Id == request.TaskId);

            var result = _mapper.Map<GetTaskByIdResponse>(task);
            return result;
        }
    }
}
