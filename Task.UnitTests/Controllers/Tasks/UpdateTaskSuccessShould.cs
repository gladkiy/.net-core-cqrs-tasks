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

    public class UpdateTaskSuccessShould
    {
        public UpdateTaskSuccessShould()
        {
            _logger = new Mock<ILogger<TasksController>>();
            _dispatcher = new Mock<IDispatcher>();

            _dispatcher.Setup(r => r.PushAsync(_cmdHandler, this._cmd))
                    .Callback((ICommandHandlerAsync<UpdateTaskCmd> h, UpdateTaskCmd c) => c.Result = this._response).Returns(Task.FromResult((object)null));

            _controller = new TasksController(_logger.Object, _dispatcher.Object, null);
        }

        private TasksController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<TasksController>> _logger { get; set; }

        private ICommandHandlerAsync<UpdateTaskCmd> _cmdHandler { get; set; }

        private readonly UpdateTaskCmd _cmd = new UpdateTaskCmd
        {
            Description = "new description"
        };

        private readonly UpdateTaskCmd.Response _response = new UpdateTaskCmd.Response(1);

        private readonly int _id = 1;

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.Put(this._id, this._cmd, _cmdHandler) as JsonResult;
            var expectedResult = new Json(this._response);

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.NotNull(((Json)result.Value).Data);
            Assert.Equal(((UpdateTaskCmd.Response)((Json)result.Value).Data).Id, ((UpdateTaskCmd.Response)expectedResult.Data).Id);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}