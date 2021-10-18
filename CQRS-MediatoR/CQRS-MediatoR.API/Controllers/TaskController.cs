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
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] AddTaskRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("StartTask")]
        public IActionResult StartTask([FromBody]StartTaskRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("AllTasks")]
        public IActionResult GetTasks ([FromQuery]GetTasksRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("GetTask")]
        public IActionResult GetTaskById([FromQuery] GetTaskByIdRequest request)
        {
            var response = _mediator.Send(request);
            return Ok(response);
        }
    }
}
