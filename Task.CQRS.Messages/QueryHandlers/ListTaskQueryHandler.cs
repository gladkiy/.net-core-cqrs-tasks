namespace Task.CQRS.Messages.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Queries;
    using Task.DBModels.Entities;
    using Task.DTOModels.ViewModels;
    using Task = Task.DBModels.Entities.Task;

    public class ListTaskQueryHandler : IQueryHandler<ListTaskQuery, ListTaskQuery.Response>
    {
        private readonly IDataContext Repository;

        public ListTaskQueryHandler(IDataContext dataContext)
        {
            this.Repository = dataContext;
        }

        public ListTaskQuery.Response Handle(ListTaskQuery message)
        {
            if (message.PageSize > 100)
            {
                message.PageSize = 100;
            }

            IQueryable<Task> tasks = this.Repository.Tasks;

            if (message.UserId != 0)
            {
                tasks = tasks.Where(x => x.UserId == message.UserId);
            }

            if (!string.IsNullOrEmpty(message.Description))
            {
                tasks = tasks.Where(x => x.Description.ToLower().Contains(message.Description.ToLower()));
            }

            if (message.Completed.HasValue)
            {
                tasks = tasks.Where(x => x.Completed == message.Completed.Value);
            }

            if (string.IsNullOrEmpty(message.Sort))
            {
                message.Sort = "id asc";
            }

            var sortArr = message.Sort.Split(' ');
            var sortField = sortArr[0];
            var sortDir = "asc";
            if (sortArr.Length == 2)
            {
                sortDir = sortArr[1];
            }

            var tasksTemp = tasks.AsEnumerable();

            foreach (var prop in typeof(Task).GetProperties())
            {
                if (sortField.IndexOf(prop.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tasksTemp = sortDir == "desc"
                        ? tasksTemp.OrderByDescending(x => prop.GetValue(x, null))
                        : tasksTemp.OrderBy(x => prop.GetValue(x, null));
                }
            }

            var total = tasksTemp.Count();

            tasksTemp = tasksTemp.Skip((message.PageNumber - 1) * message.PageSize);
            var restCount = total - (message.PageNumber - 1) * message.PageSize;
            if (restCount >= message.PageSize)
            {
                tasksTemp = tasksTemp.Take(message.PageSize);
            }

            var tasksVm = new List<TaskVm>();
            foreach (var task in tasksTemp)
            {
                var taskVm = new TaskVm(task);
                tasksVm.Add(taskVm);
            }

            return new ListTaskQuery.Response(tasksVm, new Meta(total, message.PageSize, message.PageNumber));
        }
    }
}