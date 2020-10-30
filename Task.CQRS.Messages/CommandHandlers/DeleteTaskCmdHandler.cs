namespace Task.CQRS.Messages.CommandHandlers
{
    using Microsoft.EntityFrameworkCore;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Enums;
    using Task.CrossCutting.Exceptions;
    using Task.DBModels.Entities;
    using Task = System.Threading.Tasks.Task;

    public class DeleteTaskCmdHandler : ICommandHandlerAsync<DeleteTaskCmd>
    {
        private readonly IDataContext Repository;

        public DeleteTaskCmdHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public async Task HandleAsync(DeleteTaskCmd command)
        {
            var task = await this.Repository.Tasks.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (task == null)
            {
                throw new CustomException((int)ExceptionCode.NotFound, "Task doesn't exist");
            }
            this.Repository.Tasks.Remove(task);

            await this.Repository.SaveChangesAsync();
        }
    }
}