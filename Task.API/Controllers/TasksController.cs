using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Task.CQRS.Impl;
using Task.CQRS.Messages.Commands;
using Task.CQRS.Messages.Queries;
using Task.CrossCutting.Enums;
using Task.CrossCutting.Exceptions;
using Task.DBModels.Entities;
using Task.DTOModels;

namespace Task.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;

        private readonly IDispatcher _dispatcher;

        public TasksController(ILogger<TasksController> logger,
            IDispatcher dispatcher,
            UserManager<User> userManager)
            : base(userManager)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        // GET: api/tasks
        [HttpGet]
        public IActionResult List(ListTaskQuery query,
            [FromServices] IQueryHandler<ListTaskQuery, ListTaskQuery.Response> handler)
        {
            var result = this._dispatcher.Query(handler, query);
            return Json(new Json(result.Tasks, result.Meta));
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(GetTaskQuery query,
            [FromServices] IQueryHandlerAsync<GetTaskQuery, GetTaskQuery.Response> handler)
        {
            var result = await this._dispatcher.QueryAsync(handler, query);
            return Json(new Json(result.Task));
        }

        // POST api/tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTaskCmd cmd,
            [FromServices] ICommandHandlerAsync<CreateTaskCmd> handler)
        {
            if (!ModelState.IsValid)
            {
                var errors = GetErrors();
                throw new CustomException((int)ExceptionCode.Validation, errors);
            }
            cmd.Validate();
            await this._dispatcher.PushAsync(handler, cmd);

            _logger.LogInformation("Create task");
            return Json(new Json(cmd.Result));
        }

        // PUT api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateTaskCmd cmd,
            [FromServices] ICommandHandlerAsync<UpdateTaskCmd> handler)
        {
            if (!ModelState.IsValid)
            {
                var errors = GetErrors();
                throw new CustomException((int)ExceptionCode.Validation, errors);
            }
            cmd.Validate();
            cmd.Id = id;
            await this._dispatcher.PushAsync(handler, cmd);

            _logger.LogInformation($"Update Task with id = {id}");
            return Json(new Json(cmd.Result));
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteTaskCmd cmd,
            [FromServices] ICommandHandlerAsync<DeleteTaskCmd> handler)
        {
            if (id != cmd.Id)
            {
                throw new CustomException((int)ExceptionCode.Validation, "id mismatch");
            }
            await this._dispatcher.PushAsync(handler, cmd);

            _logger.LogInformation($"Delete Task with id = {id}");
            return Json(new Json());
        }
    }
}
