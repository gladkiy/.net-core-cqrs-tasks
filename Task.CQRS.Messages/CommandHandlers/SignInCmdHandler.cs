namespace Task.CQRS.Messages.CommandHandlers
{
    using Microsoft.AspNetCore.Identity;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class SignInCmdHandler : ICommandHandlerAsync<SignInCmd>
    {
        private readonly SignInManager<User> _signInManager;

        public SignInCmdHandler(SignInManager<User> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task HandleAsync(SignInCmd command)
        {
            var result = await this._signInManager.PasswordSignInAsync(
                command.UserName,
                command.Password,
                command.RememberMe,
                lockoutOnFailure: false);
            command.Result = result;
        }
    }
}