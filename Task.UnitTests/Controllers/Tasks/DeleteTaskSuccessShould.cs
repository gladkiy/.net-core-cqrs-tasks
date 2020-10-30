namespace Task.UnitTests.Controllers.Tasks
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DTOModels;
    using Xunit;

    public class DeleteTaskSuccessShould
    {
        public DeleteTaskSuccessShould()
        {
            _logger = new Mock<ILogger<TasksController>>();
            _dispatcher = new Mock<IDispatcher>();

            _controller = new TasksController(_logger.Object, _dispatcher.Object, null);
        }

        private TasksController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<TasksController>> _logger { get; set; }

        private ICommandHandlerAsync<DeleteTaskCmd> _cmdHandler { get; set; }

        private readonly DeleteTaskCmd _cmd = new DeleteTaskCmd
        {
            Id = 1
        };

        private readonly int _id = 1;

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.Delete(this._id, this._cmd, _cmdHandler) as JsonResult;

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.Null(((Json)result.Value).Data);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}