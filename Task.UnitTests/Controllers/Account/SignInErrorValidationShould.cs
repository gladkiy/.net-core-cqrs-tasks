namespace Task.UnitTests.Controllers.Account
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Exceptions;
    using Xunit;

    public class SignInErrorValidationShould
    {
        public SignInErrorValidationShould()
        {
            _controller = new AccountController(null, null, null);

            _controller.ControllerContext = new ControllerContext();

            _controller.ModelState.AddModelError("UserName", this._errors);
        }

        private AccountController _controller { get; set; }

        private readonly SignInCmd _cmd = new SignInCmd
        {
            UserName = "dfg,dfgdfg,dqwe,egh,rtyjh,tyj,tyjtyjtyj,tyjtyj,tyjtwqw,wefas"
        };

        private readonly string _errors = "The field UserName must be a string with a maximum length of '50'.";

        [Fact]
        public async Task Execute()
        {
            var result = await Assert.ThrowsAsync<CustomException>(async () => await _controller.SignIn(this._cmd, null));
            Assert.Equal(this._errors, result.Message);
        }
    }
}