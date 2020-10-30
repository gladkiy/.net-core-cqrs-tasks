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

    public class SignInShould
    {
        public SignInShould()
        {
            _logger = new Mock<ILogger<AccountController>>();
            _dispatcher = new Mock<IDispatcher>();

            _controller = new AccountController(_logger.Object, _dispatcher.Object, null);
        }

        private AccountController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<AccountController>> _logger { get; set; }

        private ICommandHandlerAsync<SignInCmd> _cmdHandler { get; set; }

        private readonly SignInCmd _cmd = new SignInCmd
        {
            UserName = "asd@asd.asd",
            Password = "123456"
        };

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.SignIn(this._cmd, _cmdHandler) as JsonResult;

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.Null(((Json)result.Value).Data);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}