namespace Task.CQRS.Messages.CommandHandlers
{
    using Microsoft.EntityFrameworkCore;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Enums;
    using Task.CrossCutting.Exceptions;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class UpdateTaskCmdHandler : ICommandHandlerAsync<UpdateTaskCmd>
    {
        private readonly IDataContext Repository;

        public UpdateTaskCmdHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public async Task HandleAsync(UpdateTaskCmd command)
        {
            var task = await this.Repository.Tasks.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (task == null)
            {
                throw new CustomException((int)ExceptionCode.NotFound, "Task doesn't exist");
            }

            if (!string.IsNullOrEmpty(command.Description))
            {
                task.Description = command.Description;
            }
            if (command.Completed.HasValue)
            {
                task.Completed = command.Completed.Value;
            }

            await this.Repository.SaveChangesAsync();

            command.Result = new UpdateTaskCmd.Response(task.Id);
        }
    }
}