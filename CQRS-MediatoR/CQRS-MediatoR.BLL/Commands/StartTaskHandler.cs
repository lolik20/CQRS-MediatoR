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

namespace CQRS_MediatoR.BLL.Commands
{
    public class StartTaskHandler : IRequestHandler<StartTaskRequest, StartTaskResponse>
    {
        private readonly CQRSContext _context;
        public StartTaskHandler(CQRSContext context)
        {
            _context = context;
        }
        public async Task<StartTaskResponse> Handle(StartTaskRequest request, CancellationToken cancellationToken)
        {
            var task = _context.Tasks
                .FirstOrDefault(x => x.Id
                .Equals(request.TaskId));

            if (
                task.Status != Common.Entities.Domain.Enums.Status.Expired ||
                task.Status != Common.Entities.Domain.Enums.Status.InProgress)
            {

                task.Status = Common.Entities.Domain.Enums.Status.Done;
                _context.SaveChanges();

                var okResult = new StartTaskResponse()
                {
                    IsSuccessful = true
                };

                return okResult;

            }

            var result = new StartTaskResponse()
            {
                IsSuccessful = false
            };

            return result;
        }
    }
}
