namespace Task.CQRS.Messages.CommandHandlers
{
    using Microsoft.AspNetCore.Identity;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class SignOutCmdHandler : ICommandHandlerAsync<SignOutCmd>
    {
        private readonly SignInManager<User> _signInManager;

        public SignOutCmdHandler(SignInManager<User> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task HandleAsync(SignOutCmd command)
        {
            await this._signInManager.SignOutAsync();
        }
    }
}