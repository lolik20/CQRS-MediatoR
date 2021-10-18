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
    public class GetTasksHandler : IRequestHandler<GetTasksRequest, IReadOnlyCollection<GetTasksResponse>>
    {
        private readonly CQRSContext _context;
        private readonly IMapper _mapper;
        public GetTasksHandler(CQRSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<GetTasksResponse>> Handle(GetTasksRequest request, CancellationToken cancellationToken)
        {

            var expiredTasks = _context.Tasks
                .Where(x => x.DateTime < DateTime.Now
                &&x.Status!=Common.Entities.Domain.Enums.Status.Expired);

            if (expiredTasks != null)
            {
                foreach (var item in expiredTasks)
                {
                    item.Status = Common.Entities.Domain.Enums.Status.Expired;
                }
            }

            _context.SaveChanges();

            var tasks = _context.Tasks
                .Skip(request.Skip)
                .Take(request.Take);

            var result = _mapper.Map<IReadOnlyCollection<GetTasksResponse>>(tasks);
           
            return result;
        }
    }
}
