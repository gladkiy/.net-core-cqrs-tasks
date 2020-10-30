namespace Task.UnitTests.Controllers.Account
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

    public class SignUpShould
    {
        public SignUpShould()
        {
            _logger = new Mock<ILogger<AccountController>>();
            _dispatcher = new Mock<IDispatcher>();

            _controller = new AccountController(_logger.Object, _dispatcher.Object, null);
        }

        private AccountController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<AccountController>> _logger { get; set; }

        private ICommandHandlerAsync<SignUpCmd> _cmdHandler { get; set; }

        private readonly SignUpCmd _cmd = new SignUpCmd
        {
            UserName = "asd@asd.asd",
            Password = "123456"
        };

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.SignUp(this._cmd, _cmdHandler) as JsonResult;

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.Null(((Json)result.Value).Data);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}