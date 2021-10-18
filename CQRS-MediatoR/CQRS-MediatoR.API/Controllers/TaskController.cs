using CQRS_MediatoR.Common.Entities.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS_MediatoR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController:ControllerBase
    {
        private readonly IMediator _mediator;
        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult AddTask([FromBody] AddTaskRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);
        }
        public IActionResult StartTask([FromQuery]StartTaskRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);

        }
    }
}
