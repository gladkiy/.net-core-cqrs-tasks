namespace Task.UnitTests.Controllers.Account
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Security.Claims;
    using Task.API.Controllers;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task.DTOModels;
    using Xunit;
    using Task = System.Threading.Tasks.Task;

    public class SignOutShould
    {
        public SignOutShould()
        {
            _logger = new Mock<ILogger<AccountController>>();
            _dispatcher = new Mock<IDispatcher>();

            _userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(_userStore.Object, null, null, null, null, null, null, null, null);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                                                             {
                                                                  new Claim(ClaimTypes.NameIdentifier, this._user.Id.ToString()),
                                                                  new Claim(ClaimTypes.Name, this._user.Email)
                                                              }));

            _userManager.Setup(r => r.GetUserAsync(user)).ReturnsAsync(this._user);

            _controller = new AccountController(_logger.Object, _dispatcher.Object, _userManager.Object);

            _controller.ControllerContext = new ControllerContext();

            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        }

        private AccountController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private Mock<ILogger<AccountController>> _logger { get; set; }

        private Mock<IUserStore<User>> _userStore { get; set; }

        private Mock<UserManager<User>> _userManager { get; set; }

        private ICommandHandlerAsync<SignOutCmd> _cmdHandler { get; set; }

        private readonly User _user = new User
        {
            Id = 1,
            Email = "abc@abc.com"
        };

        private readonly SignOutCmd _cmd = new SignOutCmd();

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.SignOut(this._cmd, _cmdHandler) as JsonResult;

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.Null(((Json)result.Value).Data);
            _dispatcher.Verify(r => r.PushAsync(_cmdHandler, this._cmd), Times.Once);
        }
    }
}