using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Task.CQRS.Impl;
using Task.CQRS.Messages.Commands;
using Task.CrossCutting.Enums;
using Task.CrossCutting.Exceptions;
using Task.DBModels.Entities;
using Task.DTOModels;

namespace Task.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IDispatcher _dispatcher;

        public AccountController(ILogger<AccountController> logger,
            IDispatcher dispatcher,
            UserManager<User> userManager)
            : base(userManager)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpCmd cmd,
            [FromServices] ICommandHandlerAsync<SignUpCmd> handler)
        {
            if (!ModelState.IsValid)
            {
                var errors = GetErrors();
                throw new CustomException((int)ExceptionCode.Validation, errors);
            }

            await this._dispatcher.PushAsync(handler, cmd);

            _logger.LogInformation("Sign Up new User");
            return Json(new Json());
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInCmd cmd,
            [FromServices] ICommandHandlerAsync<SignInCmd> handler)
        {
            if (!ModelState.IsValid)
            {
                var errors = GetErrors();
                throw new CustomException((int)ExceptionCode.Validation, errors);
            }

            await this._dispatcher.PushAsync(handler, cmd);

            _logger.LogInformation($"User \"{cmd.UserName}\" signed in");
            return Json(new Json());
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut(SignOutCmd cmd,
           [FromServices] ICommandHandlerAsync<SignOutCmd> handler)
        {
            var user = await GetCurrentUserAsync();
            await this._dispatcher.PushAsync(handler, cmd);
            _logger.LogInformation($"User \"{user.Email}\" signed out");
            return Json(new Json());
        }
    }
}
