using AutoMapper;
using CQRS_MediatoR.Common.Entities.RequestModels;
using CQRS_MediatoR.Common.Entities.ResponseModels;
using CQRS_MediatoR.DAL.SqlContext;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_MediatoR.BLL.Commands
{
    public class AddTaskHandler : IRequestHandler<AddTaskRequest, AddTaskResponse>
    {
        private readonly CQRSContext _context;
        private readonly IMapper _mapper;
      
        public AddTaskHandler(CQRSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AddTaskResponse> Handle(AddTaskRequest request, CancellationToken cancellationToken)
        {
            var task = _context.Tasks.Find(request);

            if (task != null)
            {
                var model = _mapper.Map<Common.Entities.Domain.Task>(request);

                _context.Tasks.Add(model);
                _context.SaveChanges();

                var addedResult = new AddTaskResponse()
                {
                    IsSuccessful = true
                };

                return addedResult;

            }

            var result = new AddTaskResponse()
            {
                IsSuccessful = false
            };

            return result;
        }
    }
}
