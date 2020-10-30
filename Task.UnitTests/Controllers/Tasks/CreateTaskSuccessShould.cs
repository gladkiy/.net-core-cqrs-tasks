namespace Task.UnitTests.Controllers.Tasks
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Task.API.Controllers;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DTOModels;
    using Xunit;
    using Task = System.Threading.Tasks.Task;

    public class CreateTaskSuccessShould
    {
        public CreateTaskSuccessShould()
        {
            _logger = new Mock<ILogger<TasksController>>();
            _dispatcher = new Mock<IDispatcher>();

            _dispatcher.Setup(r => r.PushAsync(_cmdHandler, this._cmd))
                    .Callback((ICommandHandlerAsync<CreateTaskCmd> h, CreateTaskCmd c) => c.Result = this._response).Returns(Task.FromResult((object)null));

            _controller = new TasksController(_logger.Object, _dispatcher.Object, null);
        }

        private TasksController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<TasksController>> _logger { get; set; }

        private ICommandHandlerAsync<CreateTaskCmd> _cmdHandler { get; set; }

        private readonly CreateTaskCmd _cmd = new CreateTaskCmd
        {
            Description = "task description"
        };

        private readonly CreateTaskCmd.Response _response = new CreateTaskCmd.Response(1);

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.Post(this._cmd, _cmdHandler) as JsonResult;
            var expectedResult = new Json(this._response);

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.NotNull(((Json)result.Value).Data);
            Assert.Equal(((CreateTaskCmd.Response)((Json)result.Value).Data).Id, ((CreateTaskCmd.Response)expectedResult.Data).Id);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}