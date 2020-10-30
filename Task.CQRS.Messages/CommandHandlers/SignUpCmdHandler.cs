namespace Task.CQRS.Messages.CommandHandlers
{
    using Microsoft.AspNetCore.Identity;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class SignUpCmdHandler : ICommandHandlerAsync<SignUpCmd>
    {
        private readonly UserManager<User> _userManager;

        public SignUpCmdHandler(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task HandleAsync(SignUpCmd command)
        {
            var user = command.ToUser();
            var result = await this._userManager.CreateAsync(user, command.Password);
            command.Result = result;
        }
    }
}