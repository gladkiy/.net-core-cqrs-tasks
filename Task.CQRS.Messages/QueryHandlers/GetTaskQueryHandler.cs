using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task.CQRS.Impl;
using Task.CQRS.Messages.Queries;
using Task.CrossCutting.Enums;
using Task.CrossCutting.Exceptions;
using Task.DBModels.Entities;
using Task.DTOModels.ViewModels;

namespace Task.CQRS.Messages.QueryHandlers
{
    public class GetTaskQueryHandler : IQueryHandlerAsync<GetTaskQuery, GetTaskQuery.Response>
    {
        private readonly IDataContext Repository;

        public GetTaskQueryHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public async Task<GetTaskQuery.Response> HandleAsync(GetTaskQuery message)
        {
            var task = await this.Repository.Tasks.FirstOrDefaultAsync(x => x.Id == message.Id);
            if (task != null)
            {
                var taskVm = new TaskVm(task);
                return new GetTaskQuery.Response(taskVm);
            }

            throw new CustomException((int)ExceptionCode.NotFound, "Task doesn't exist");
        }
    }
}