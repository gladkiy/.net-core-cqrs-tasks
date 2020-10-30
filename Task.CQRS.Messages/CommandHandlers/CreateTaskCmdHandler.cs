namespace Task.CQRS.Messages.CommandHandlers
{
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class CreateTaskCmdHandler : ICommandHandlerAsync<CreateTaskCmd>
    {
        private readonly IDataContext Repository;

        public CreateTaskCmdHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public async Task HandleAsync(CreateTaskCmd command)
        {
            var task = command.ToTask();
            this.Repository.Tasks.Add(task);
            await this.Repository.SaveChangesAsync();

            command.Result = new CreateTaskCmd.Response(task.Id);
        }
    }
}