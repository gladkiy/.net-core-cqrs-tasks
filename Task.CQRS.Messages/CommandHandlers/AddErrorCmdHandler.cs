namespace Task.CQRS.Messages.CommandHandlers
{
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class AddErrorCmdHandler : ICommandHandlerAsync<AddErrorCmd>
    {
        private readonly IDataContext Repository;

        public AddErrorCmdHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public async Task HandleAsync(AddErrorCmd command)
        {
            var errorLog = command.ToErrorLog();
            this.Repository.ErrorLogs.Add(errorLog);
            await this.Repository.SaveChangesAsync();
            command.Result = errorLog;
        }
    }
}