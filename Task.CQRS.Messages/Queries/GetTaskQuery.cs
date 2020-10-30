using Task.DTOModels.ViewModels;

namespace Task.CQRS.Messages.Queries
{
    public class GetTaskQuery
    {
        public int Id { get; set; }

        public class Response
        {
            public Response(TaskVm task)
            {
                Task = task;
            }

            public TaskVm Task { get; set; }
        }
    }
}